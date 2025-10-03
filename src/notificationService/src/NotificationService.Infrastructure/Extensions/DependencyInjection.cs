using Insurence.Platform.Common.Messaging.RabbitMq.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Services;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain.Interfaces.Repositories;
using NotificationService.Infrastructure.Data.Contexts;
using NotificationService.Infrastructure.Data.Repositories;

namespace NotificationService.Infrastructure.Extensions;

/// <summary>
/// Fornece métodos de extensão para registrar serviços de infraestrutura e dependências necessárias para a aplicação.
/// </summary>
/// <remarks>Esta classe contém métodos estáticos usados para configurar e adicionar serviços de nível de infraestrutura, como
/// contextos de banco de dados, repositórios e componentes de mensageria, ao contêiner de injeção de dependência da aplicação.
/// Esses métodos são normalmente chamados durante a inicialização da aplicação para garantir que todos os serviços necessários estejam disponíveis para
/// operações em tempo de execução.</remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona serviços de infraestrutura, incluindo contexto de banco de dados, repositórios, serviços de notificação e integração com RabbitMQ
    /// à coleção de serviços especificada.
    /// </summary>
    /// <remarks>Este método registra as dependências necessárias para notificações e mensageria, incluindo
    /// serviços de banco de dados e RabbitMQ. Chame este método durante a inicialização da aplicação para garantir que todos os componentes de infraestrutura
    /// estejam disponíveis para injeção de dependência.</remarks>
    /// <param name="services">A coleção de serviços à qual os serviços de infraestrutura serão adicionados. Não pode ser nula.</param>
    /// <param name="configuration">A configuração da aplicação usada para recuperar strings de conexão e configurações necessárias para a configuração da infraestrutura.
    /// Não pode ser nula.</param>
    /// <returns>A mesma instância da coleção de serviços com os serviços de infraestrutura registrados para injeção de dependência.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISendNotificationService, SendNotificationService>();

        services.AddRabbitMq(configuration);
        return services;
    }
}
