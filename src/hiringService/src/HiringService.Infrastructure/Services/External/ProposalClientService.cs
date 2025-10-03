using HiringService.Application.DataTransferObjects.Configurations;
using HiringService.Domain.Entities;
using HiringService.Domain.Interfaces.Services;
using Insurence.Platform.Common.Wrappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RestSharp;
using System.Text.Json;

namespace HiringService.Infrastructure.Services.External;

/// <summary>
/// Fornece operações do lado do cliente para interação com o serviço de propostas, incluindo a recuperação assíncrona do status da proposta.
/// Esta classe implementa estratégias de resiliência como tentativas e circuit breaker para lidar com falhas transitórias ao comunicar-se com endpoints externos.
/// </summary>
/// <remarks>ProposalClientService foi projetado para uso em aplicações que exigem comunicação confiável com um serviço remoto de propostas.
/// Utiliza configurações de endpoint configuráveis e logging estruturado para facilitar diagnósticos e monitoramento operacional.
/// A classe é thread-safe e deve ser registrada como singleton ou serviço scoped em containers de injeção de dependência.
/// Todos os métodos públicos são assíncronos e suportam cancelamento via CancellationToken.</remarks>
public sealed class ProposalClientService : IProposalClientService
{
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<ProposalClientService> _logger;
    private readonly EndpointsSettings _settings;
    private readonly RestClient _restClient;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    /// <summary>
    /// Inicializa uma nova instância da classe ProposalClientService, configurando endpoints e logging para operações do serviço de propostas.
    /// </summary>
    /// <remarks>Este construtor configura opções do cliente HTTP, configurações de serialização JSON e políticas de resiliência incluindo tentativas e circuit breaker para comunicação com o serviço.
    /// O logging é integrado aos eventos de tentativas e circuit breaker para auxiliar no monitoramento e solução de problemas.</remarks>
    /// <param name="settings">As configurações da aplicação contendo a configuração do endpoint para o serviço de propostas. Não pode ser nulo.</param>
    /// <param name="logger">O logger utilizado para registrar informações de diagnóstico e operação para o ProposalClientService. Não pode ser nulo.</param>
    public ProposalClientService(
        IOptions<EndpointsSettings> settings,
        ILogger<ProposalClientService> logger)
    {
        _logger = logger;
        _settings = settings.Value;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var options = new RestClientOptions(_settings.ProposalServiceBaseUrl)
        {
            ThrowOnAnyError = false,
            Timeout = new TimeSpan(0, 0, 10),
            ThrowOnDeserializationError = false
        };

        _restClient = new RestClient(options);

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Tentativa {RetryCount} falhou após {Delay}.", retryCount, timeSpan);
                });

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (exception, breakDelay) =>
                {
                    _logger.LogWarning(exception, "Circuit breaker aberto por {BreakDelay}.", breakDelay);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker fechado. As operações serão retomadas.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker em estado half-open. Testando a próxima operação.");
                });
    }

   /// <summary>
   /// Recupera assincronamente o status atual de uma proposta identificada pelo ID especificado.
   /// </summary>
   /// <remarks>Retorna <see langword="null"/> se a proposta não existir ou se a requisição falhar.
   /// Este método registra informações e erros relacionados ao processo de recuperação.</remarks>
   /// <param name="proposalId">O identificador único da proposta cujo status será recuperado.</param>
   /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
   /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a proposta com seu status atual se encontrada; caso contrário, <see langword="null"/>.</returns>
    public async Task<Proposal?> GetProposalStatusAsync(Guid proposalId, CancellationToken cancellationToken)
    {

        return await ExecuteWithPoliciesAsync(async ct =>
        {
            _logger.LogInformation("Buscando status da proposta com ID {ProposalId}", proposalId);
            var request = new RestRequest($"/api/proposal/{proposalId}/status", Method.Get);
            var response = await _restClient.ExecuteAsync<ResponseDefault<Proposal>>(request, ct);
            if (response is null)
            {
                _logger.LogError("Resposta nula ao buscar proposta {ProposalId}", proposalId);
                return null;
            }
            if (!response.IsSuccessful)
            {
                _logger.LogError("Erro ao buscar proposta {ProposalId}. Status Code: {StatusCode}, Content: {Content}", proposalId, response.StatusCode, response.Content);
                return null;
            }

            var proposalResponse = JsonSerializer.Deserialize<ResponseDefault<Proposal>>(response.Content!, _jsonOptions);
            if (proposalResponse is null)
            {
                _logger.LogError("Falha ao desserializar a resposta da proposta {ProposalId}", proposalId);
                return null;
            }
            if (!proposalResponse.Success)
            {
                _logger.LogWarning("Resposta de falha ao buscar proposta {ProposalId}. Erros: {Errors}", proposalId, string.Join(", ", proposalResponse.Errors?.Select(e => e.Message) ?? Array.Empty<string>()));
                return null;
            }
            if (proposalResponse?.Data is null)
            {
                _logger.LogWarning("Proposta com ID {ProposalId} não encontrada.", proposalId);
                return null;
            }

            _logger.LogInformation("Proposta com ID {ProposalId} encontrada com status {Status}.", proposalId, proposalResponse.Data.Status);
            return proposalResponse.Data;
        }, cancellationToken);
    }

    /// <summary>
    /// Executa a ação assíncrona especificada aplicando as políticas de tentativas e circuit breaker.
    /// </summary>
    /// <remarks>A ação é executada dentro do contexto das políticas de tentativas e circuit breaker.
    /// Se o circuit breaker estiver aberto ou todas as tentativas forem esgotadas, a tarefa retornada pode ser concluída com resultado <see langword="null"/>.
    /// O chamador deve garantir que a ação fornecida observe corretamente o token de cancelamento.</remarks>
    /// <typeparam name="T">O tipo do resultado retornado pela ação assíncrona.</typeparam>
    /// <param name="action">Um delegate representando a operação assíncrona a ser executada. O delegate recebe um <see cref="CancellationToken"/> que deve ser observado para cancelamento.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a execução da operação.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o valor retornado pela ação, ou <see langword="null"/> se a operação falhar e as políticas não permitirem novas tentativas.</returns>
    private async Task<T?> ExecuteWithPoliciesAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken)
    {
        return await _retryPolicy.ExecuteAsync(async ct =>
            await _circuitBreakerPolicy.ExecuteAsync(async innerCt =>
                await action(innerCt), ct), cancellationToken);
    }
}
