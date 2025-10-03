using Insurence.Platform.Common.Messaging.RabbitMq.Configurations;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces.Notification;
using Insurence.Platform.Common.Messaging.RabbitMq.Services;
using Insurence.Platform.Common.Messaging.RabbitMq.Services.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Extensions;

/// <summary>
/// Fornece métodos de extensão para registrar serviços relacionados ao RabbitMQ em um <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>Esta classe deve ser utilizada na inicialização da aplicação para configurar a conectividade com o RabbitMQ e
/// as capacidades de publicação de mensagens. Os métodos de extensão adicionam as dependências necessárias para integração com RabbitMQ,
/// incluindo fábricas de conexão e publicadores de mensagens, ao contêiner de injeção de dependência.</remarks>
public static class RabbitMqServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços e configurações relacionados ao RabbitMQ à coleção de serviços especificada.
    /// </summary>
    /// <remarks>Registra a fábrica de conexão do RabbitMQ e os serviços de publicação de mensagens de notificação para
    /// injeção de dependência. As configurações do RabbitMQ são obtidas da seção 'RabbitMqSettings' da configuração fornecida.</remarks>
    /// <param name="services">A coleção de serviços à qual os serviços do RabbitMQ serão adicionados. Não pode ser nula.</param>
    /// <param name="configuration">A configuração da aplicação contendo a seção de configurações do RabbitMQ. Não pode ser nula.</param>
    /// <returns>A mesma instância da coleção de serviços, com os serviços e configurações do RabbitMQ registrados.</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        services.AddScoped<INotificationMessagePublish, NotificatonMessagePublish>();
        return services;
    }
}
