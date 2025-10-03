using Insurence.Platform.Common.Messaging.RabbitMq.Configurations;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Services;

/// <summary>
/// Fornece uma fábrica para criar conexões RabbitMQ usando as configurações especificadas.
/// </summary>
/// <remarks>Esta classe usa a configuração fornecida em <see cref="RabbitMqSettings"/> para inicializar uma
/// fábrica de conexão RabbitMQ. Ela suporta a criação de conexões de forma assíncrona.</remarks>
public sealed class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly RabbitMqSettings _settings;

    public RabbitMqConnectionFactory(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };
    }

    /// <inheritdoc/>
    public async Task<IConnection> CreateConnectionAsync() => await _connectionFactory.CreateConnectionAsync();
}

