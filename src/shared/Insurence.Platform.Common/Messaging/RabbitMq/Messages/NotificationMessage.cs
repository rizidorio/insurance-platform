using Insurence.Platform.Common.Notification.Enums;

namespace Insurence.Platform.Common.Messaging.RabbitMq.Messages;

/// <summary>
/// Representa uma mensagem de notificação contendo metadados, conteúdo e informações opcionais de contato do destinatário para
/// entrega por diversos canais.
/// </summary>
/// <remarks>Use este record para encapsular todas as informações relevantes necessárias para entregar uma notificação pelo
/// canal especificado. Nem todas as propriedades são obrigatórias para cada canal; por exemplo, e-mail e número de telefone são
/// opcionais e devem ser fornecidos apenas quando aplicável.</remarks>
/// <param name="CorrelationId">O identificador único usado para correlacionar esta mensagem de notificação com operações ou eventos relacionados.</param>
/// <param name="NotificationType">O tipo de notificação que está sendo enviada, que determina o propósito ou categoria da mensagem.</param>
/// <param name="NotificationChannel">O canal pelo qual a notificação será entregue, como e-mail, SMS ou notificação push.</param>
/// <param name="Subject">O assunto ou título da mensagem de notificação, normalmente usado como manchete ou resumo.</param>
/// <param name="Body">O conteúdo principal ou corpo da mensagem de notificação.</param>
/// <param name="Email">O endereço de e-mail do destinatário, se a notificação for enviada por e-mail. Pode ser nulo se não aplicável.</param>
/// <param name="PhoneNumber">O número de telefone do destinatário, se a notificação for enviada por SMS ou canais similares. Pode ser nulo se não
/// aplicável.</param>
public sealed record NotificationMessage(
    Guid CorrelationId,
    NotificationType NotificationType,
    NotificationChannel NotificationChannel,
    string Subject,
    string Body,
    string? Email = null,
    string? PhoneNumber = null);
