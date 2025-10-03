using Insurence.Platform.Common.Helpers;
using Insurence.Platform.Common.Middlewares;
using ProposalService.Api.Extensions;
using ProposalService.Infrastructure.Extensions;

namespace ProposalService.Api;

public class Program
{
    protected Program()
    {
    }

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerModelConvention());
        });

        builder.Services.AddEndpointsApiExplorer();

        if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
            builder.Services.AddSwaggerDocumentation();

        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseSwaggerDocumentation();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        await app.RunAsync();
    }
}