using FluentValidation;
using HiringService.Application.DataTransferObjects.Configurations;
using HiringService.Application.Services;
using HiringService.Application.Services.Interfaces;
using HiringService.Application.Validations;
using HiringService.Domain.Interfaces.Repositories;
using HiringService.Domain.Interfaces.Services;
using HiringService.Domain.Services;
using HiringService.Infrastructure.Data.Contexts;
using HiringService.Infrastructure.Data.Repositories;
using HiringService.Infrastructure.Services.External;
using Insurence.Platform.Common.Messaging.RabbitMq.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace HiringService.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddHttpClient(configuration);

        services.AddScoped<IContractService, ContractService>();
        services.AddScoped<IContractValidation, ContractValidation>();
        services.AddScoped<ICalculateEffectiveDateEnd, CalculateEffectiveDateEnd>();

        services.AddRabbitMq(configuration);
        services.AddValidatorsFromAssembly(typeof(CreateContractRequestValidation).Assembly);
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HiringDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IContractRepository, ContractRepository>();

        return services;
    }

    private static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EndpointsSettings>(configuration.GetSection("Endpoints"));

        services.AddHttpClient<IProposalClientService, ProposalClientService>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var endpointsSettings = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<EndpointsSettings>>().Value;
                client.BaseAddress = new Uri(endpointsSettings.ProposalServiceBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "HiringServiceClient");
                client.Timeout = TimeSpan.FromSeconds(endpointsSettings.TimeoutInSeconds);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy())
            .AddPolicyHandler(GetTimeoutPolicy())
            .AddPolicyHandler(GetFallbackPolicy());

        services.AddScoped<IProposalClientService, ProposalClientService>();
        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (timespan, retryAttempt, context) =>
            {
                var logger = GetLogger(context);
                logger?.LogWarning("Tentativa {RetryAttempt} falhou. Esperando {Timespan} antes da próxima tentativa.", retryAttempt, timespan);
            });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, breakDelay, context) =>
                {
                    var logger = GetLogger(context);
                    logger?.LogError(
                        "Circuit Breaker ABERTO por {BreakDelay}s. " +
                        "Erro: {Error}",
                        breakDelay.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                },
                onReset: (context) =>
                {
                    var logger = GetLogger(context);
                    logger?.LogInformation("Circuit Breaker RESETADO - Voltando ao normal");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuit Breaker HALF-OPEN - Testando conexão...");
                });
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy
            .TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(10),
                timeoutStrategy: TimeoutStrategy.Pessimistic,
                onTimeoutAsync: async (context, timespan, task) =>
                {
                    var logger = GetLogger(context);
                    logger?.LogWarning(
                        "Timeout de {Timeout}s excedido na requisição",
                        timespan.TotalSeconds);
                    await Task.CompletedTask;
                });
    }


    private static IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<Exception>()
            .FallbackAsync<HttpResponseMessage>(
                fallbackAction: async (cancellationToken) =>
                {
                    var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent(
                            System.Text.Json.JsonSerializer.Serialize(new
                            {
                                error = "Serviço temporariamente indisponível",
                                message = "Tente novamente em alguns instantes"
                            }))
                    };
                    return await Task.FromResult(response);
                },
                onFallbackAsync: async (outcome) =>
                {
                    Console.WriteLine($"Fallback ativado devido a falha na comunicação, {outcome.Exception.Message}");
                    await Task.CompletedTask;
                }
            );
    }


    private static ILogger? GetLogger(Context context)
    {
        if (context.TryGetValue("logger", out var logger))
        {
            return logger as ILogger;
        }
        return null;
    }
}
