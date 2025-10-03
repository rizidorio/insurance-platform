using RabbitMQ.Client;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Interfaces;

/// <summary>
/// Define uma fábrica para criar conexões com um broker de mensagens RabbitMQ.
/// </summary>
/// <remarks>Esta interface fornece um método para criar assincronamente uma conexão com o RabbitMQ. Implementações
/// desta interface são responsáveis por gerenciar o ciclo de vida da conexão e garantir a configuração adequada para
/// conectar ao servidor RabbitMQ.</remarks>
public interface IRabbitMqConnectionFactory
{
    /// <summary>
    /// Cria uma conexão com o RabbitMQ.
    /// </summary>
    /// <returns>Uma instância de <see cref="IConnection"/> representando a conexão com o RabbitMQ.</returns>
    Task<IConnection> CreateConnectionAsync();
}
