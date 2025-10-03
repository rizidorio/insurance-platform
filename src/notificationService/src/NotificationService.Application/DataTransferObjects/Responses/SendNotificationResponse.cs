namespace NotificationService.Application.DataTransferObjects.Responses;

/// <summary>
/// Representa o resultado de uma operação de envio de notificação, incluindo identificadores, status e informações de erro.
/// </summary>
/// <param name="NotificationId">O identificador único atribuído à notificação enviada.</param>
/// <param name="CorrelationId">O identificador usado para correlacionar esta resposta com a solicitação original de notificação.</param>
/// <param name="Status">O status da operação de envio da notificação. Valores típicos incluem "Success" ou "Failed".</param>
/// <param name="ErrorMessage">Uma mensagem de erro opcional descrevendo o motivo da falha, caso a operação não tenha sido bem-sucedida; caso contrário, <see
/// langword="null"/>.</param>
public sealed record SendNotificationResponse(
    Guid NotificationId,
    Guid CorrelationId,
    string Status,
    string? ErrorMessage = null);
