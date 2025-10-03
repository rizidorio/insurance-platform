using Insurence.Platform.Common.Messaging.RabbitMq.Configurations;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces.Notification;
using Insurence.Platform.Common.Messaging.RabbitMq.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Services.Notification;

/// <summary>
/// Fornece funcionalidade para publicar mensagens de notificação em uma fila RabbitMQ usando operações assíncronas.
/// </summary>
/// <remarks>Esta classe gerencia o ciclo de vida das conexões e canais RabbitMQ, garantindo a publicação
/// thread-safe de mensagens de notificação. Implementa o descarte assíncrono para liberar recursos quando não forem mais necessários.</remarks>
/// <param name="connectionFactory">A fábrica utilizada para criar conexões RabbitMQ para publicação de mensagens.</param>
/// <param name="settings">As configurações para a conexão RabbitMQ e fila de destino.</param>
/// <param name="logger">O logger utilizado para registrar informações operacionais e de erro durante a publicação de mensagens.</param>
public sealed class NotificatonMessagePublish(
    IRabbitMqConnectionFactory connectionFactory,
    IOptions<RabbitMqSettings> settings,
    ILogger<NotificatonMessagePublish> logger) : INotificationMessagePublish
{
    private readonly IRabbitMqConnectionFactory _connectionFactory = connectionFactory;
    private readonly RabbitMqSettings _settings = settings.Value;
    private readonly ILogger<NotificatonMessagePublish> _logger = logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Publica uma mensagem de notificação de forma assíncrona na fila de mensagens configurada.
    /// </summary>
    /// <remarks>A mensagem é serializada como JSON e enviada com entrega persistente. Se a operação falhar,
    /// uma exceção é registrada e relançada. Este método é thread-safe.</remarks>
    /// <param name="message">A mensagem de notificação a ser publicada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de publicação.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de publicação.</returns>
    public async Task PublishAsync(NotificationMessage message, CancellationToken cancellationToken = default)
    {
        await EnsureCreateChannelAsync(cancellationToken);

        try
        {
            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            var properties = new BasicProperties
            {
                Persistent = true,
                MessageId = message.CorrelationId.ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                ContentType = "application/json"
            };

            await _channel!.BasicPublishAsync(
                    exchange: "",
                    routingKey: _settings.QueueName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: cancellationToken);

            _logger.LogInformation("Mensagem de notificção publicada com sucesso: {MessageId}", message.CorrelationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar mensagem de notificação: {MessageId}", message.CorrelationId);
            throw;
        }
    }

    /// <summary>
    /// Garante que um canal RabbitMQ seja criado e esteja aberto, estabelecendo uma conexão e declarando a fila se necessário.
    /// </summary>
    /// <remarks>Se o canal já estiver aberto, o método retorna imediatamente. Caso contrário, adquire um
    /// semáforo para garantir segurança de thread durante a criação do canal. O método é seguro para chamadas concorrentes e só irá
    /// criar ou reconectar o canal se necessário.</remarks>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de garantir que o canal seja criado e aberto.</returns>
    private async Task EnsureCreateChannelAsync(CancellationToken cancellationToken = default)
    {
        if (_channel is not null && _channel.IsOpen)
        {
            return;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_channel is not null && _channel.IsOpen)
                return;

            _connection ??= await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            _logger.LogInformation("Channel RabbitMQ criado/reconectado");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Libera assíncronamente todos os recursos utilizados pela instância atual.
    /// </summary>
    /// <remarks>Este método fecha e descarta quaisquer conexões e canais abertos associados à instância.
    /// Deve ser chamado quando a instância não for mais necessária para garantir que todos os recursos sejam devidamente liberados.
    /// Chamadas subsequentes a este método não têm efeito se os recursos já foram descartados.</remarks>
    /// <returns>Um ValueTask que representa a operação assíncrona de descarte.</returns>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection is not null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao liberar recursos do RabbitMQ.");
            throw;
        }
        finally
        {
            _semaphore.Dispose();
        }
    }
}
