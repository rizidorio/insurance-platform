using FluentValidation;
using Insurence.Platform.Common.Messaging.RabbitMq.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProposalService.Application.Services;
using ProposalService.Application.Services.Interfaces;
using ProposalService.Application.Validations;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Domain.Interfaces.Services;
using ProposalService.Domain.Services;
using ProposalService.Infrastructure.Data.Context;
using ProposalService.Infrastructure.Data.Repositories;

namespace ProposalService.Infrastructure.Extensions;

/// <summary>
/// Fornece métodos de extensão para registrar serviços e dependências de infraestrutura no contêiner de injeção de dependência da aplicação.
/// </summary>
/// <remarks>Esta classe contém métodos para configurar e adicionar serviços relacionados à infraestrutura, como contextos de banco de dados, repositórios e serviços de domínio, à coleção de serviços de uma aplicação. Destina-se a ser utilizada durante a inicialização da aplicação para garantir que todas as dependências necessárias estejam disponíveis para injeção ao longo da aplicação.</remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona serviços relacionados à infraestrutura, incluindo contexto de banco de dados, repositórios e serviços de domínio, à coleção de serviços especificada para injeção de dependência.
    /// </summary>
    /// <remarks>Este método registra o contexto de banco de dados utilizando o provedor PostgreSQL e adiciona implementações com escopo para repositórios, serviços de domínio e componentes de validação. Chame este método durante a inicialização da aplicação para garantir que todos os serviços de infraestrutura necessários estejam disponíveis para injeção de dependência.</remarks>
    /// <param name="services">A coleção de serviços à qual os serviços de infraestrutura serão adicionados. Não pode ser nula.</param>
    /// <param name="configuration">A configuração da aplicação utilizada para recuperar strings de conexão e outras configurações necessárias para o registro dos serviços. Não pode ser nula.</param>
    /// <returns>A mesma instância da coleção de serviços com os serviços de infraestrutura registrados para injeção de dependência.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProposalDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProposalRepository, ProposalRepository>();

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProposalService, Application.Services.ProposalService>();
        services.AddScoped<IClientValidation, ClientValidation>();
        services.AddScoped<IProposalValidation, ProposalValidation>();
        services.AddScoped<IRiskAnalysisService, RiskAnalysisService>();

        services.AddRabbitMq(configuration);
        services.AddValidatorsFromAssembly(typeof(CreateProposalRequestValidation).Assembly);
        return services;
    }
}
