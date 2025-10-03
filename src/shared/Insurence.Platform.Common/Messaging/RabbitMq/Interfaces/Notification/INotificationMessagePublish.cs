using Insurence.Platform.Common.Messaging.RabbitMq.Messages;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Interfaces.Notification;

/// <summary>
/// Define um contrato para publicar mensagens de notificação de forma assíncrona.
/// </summary>
/// <remarks>Implementações desta interface são responsáveis por entregar mensagens de notificação aos seus
/// destinatários ou canais pretendidos. A operação de publicação é realizada de forma assíncrona e pode ser
/// cancelada através do token fornecido.</remarks>
public interface INotificationMessagePublish
{
    /// <summary>
    /// Publica a mensagem de notificação especificada de forma assíncrona.
    /// </summary>
    /// <param name="message">A mensagem de notificação a ser publicada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de publicação.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de publicação.</returns>
    Task PublishAsync(NotificationMessage message, CancellationToken cancellationToken = default);
}
