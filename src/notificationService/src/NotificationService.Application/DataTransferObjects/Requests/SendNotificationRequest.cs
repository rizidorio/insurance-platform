using Insurence.Platform.Common.Notification.Enums;

namespace NotificationService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma solicitação para enviar uma notificação usando um canal e tipo especificados, incluindo o conteúdo da mensagem e
/// informações opcionais do destinatário.
/// </summary>
/// <remarks>Certifique-se de que as informações apropriadas do destinatário sejam fornecidas com base no canal de notificação selecionado.
/// Por exemplo, um endereço de e-mail é obrigatório para notificações por e-mail, enquanto um número de telefone é obrigatório para notificações por SMS.
/// </remarks>
/// <param name="CorrelationId">O identificador único usado para correlacionar esta solicitação de notificação com operações ou eventos relacionados.</param>
/// <param name="NotificationType">O tipo de notificação a ser enviada, que determina a natureza ou categoria da mensagem.</param>
/// <param name="NotificationChannel">O canal pelo qual a notificação será entregue, como e-mail ou SMS.</param>
/// <param name="Subject">A linha de assunto ou título da mensagem de notificação. Obrigatório para canais que suportam assunto, como e-mail.</param>
/// <param name="Body">O conteúdo principal ou corpo da mensagem de notificação.</param>
/// <param name="Email">O endereço de e-mail do destinatário. Obrigatório se o canal de notificação for e-mail; caso contrário, pode ser nulo.</param>
/// <param name="PhoneNumber">O número de telefone do destinatário. Obrigatório se o canal de notificação for SMS ou similar; caso contrário, pode ser nulo.</param>
public sealed record SendNotificationRequest(
    Guid CorrelationId,
    NotificationType NotificationType,
    NotificationChannel NotificationChannel,
    string Subject,
    string Body,
    string? Email = null,
    string? PhoneNumber = null);
