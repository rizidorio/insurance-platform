using Microsoft.Extensions.Logging;
using NotificationService.Application.DataTransferObjects.Requests;
using NotificationService.Application.DataTransferObjects.Responses;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces.Repositories;

namespace NotificationService.Application.Services;

/// <inheritdoc/>
public sealed class SendNotificationService(
    INotificationRepository notificationRepository,
    ILogger<SendNotificationService> logger) : ISendNotificationService
{
    /// <inheritdoc/>
    public async Task<SendNotificationResponse> SendAsync(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Enviado notificação. CorrelationId: {CorrelationId}, Tipo: {NotificationType}, Canal: {NotificationChannel}",
            request.CorrelationId, request.NotificationType, request.NotificationChannel);

        var notification = Notification.Create(
            type: request.NotificationType,
            channel: request.NotificationChannel,
            subject: request.Subject,
            body: request.Body,
            email: request.Email,
            phoneNumber: request.PhoneNumber);

        // Simula o envio da notificação com base no canal
        bool isSent = request.NotificationChannel switch
        {
            Insurence.Platform.Common.Notification.Enums.NotificationChannel.Email => SimulateEmailSending(request.Email ?? string.Empty),
            Insurence.Platform.Common.Notification.Enums.NotificationChannel.SMS => SimulateSmsSending(request.PhoneNumber ?? string.Empty),
            Insurence.Platform.Common.Notification.Enums.NotificationChannel.PushNotification => SimulatePushSending(),
            Insurence.Platform.Common.Notification.Enums.NotificationChannel.Whatsapp => SimulateWhatsappSending(request.PhoneNumber ?? string.Empty),
            _ => false
        };

        // Simula falha aleatória em 10% das tentativas
        if (isSent && RandomSimulateFailure() == 1)
            isSent = false;

        if (isSent)
        {
            notification.MarkAsSent();
        }
        else
        {
            notification.MarkAsFailed("Falha ao enviar a notificação.");
            logger.LogWarning("Falha ao enviar notificação. CorrelationId: {CorrelationId}, Tipo: {NotificationType}, Canal: {NotificationChannel}",
                request.CorrelationId, request.NotificationType, request.NotificationChannel);
        }

        await notificationRepository.CreateAsync(notification, cancellationToken);

        return new SendNotificationResponse(notification.ExternalId, request.CorrelationId, notification.Status.ToString(), notification.SendError);
    }

    /// <summary>
    /// Simula o processo de envio de um e-mail para o endereço especificado e indica se a operação teria sucesso com base em validação básica.
    /// </summary>
    /// <param name="email">O endereço de e-mail para o qual o e-mail simulado será enviado. Deve ser uma string não vazia contendo o caractere '@'.</param>
    /// <returns>true se o endereço de e-mail especificado passar nas verificações básicas de validação; caso contrário, false.</returns>
    private static bool SimulateEmailSending(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return false;

        return true;
    }

    /// <summary>
    /// Simula o envio de uma mensagem SMS para o número de telefone especificado e indica se a operação teria sucesso com base em validação básica.
    /// </summary>
    /// <param name="phoneNumber">O número de telefone para o qual a mensagem SMS será enviada. Deve ser uma string não vazia contendo pelo menos 10 caracteres.</param>
    /// <returns>true se o número de telefone passar nas verificações básicas de validação; caso contrário, false.</returns>
    private static bool SimulateSmsSending(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10)
            return false;
        return true;
    }

    /// <summary>
    /// Simula o envio de uma notificação push e indica se a operação foi bem-sucedida.
    /// </summary>
    /// <returns>true se a notificação push simulada foi enviada com sucesso; caso contrário, false.</returns>
    private static bool SimulatePushSending()
    {
        return true;
    }

    /// <summary>
    /// Simula o envio de uma mensagem WhatsApp para o número de telefone especificado e indica se a operação teria sucesso com base em validação básica.
    /// </summary>
    /// <param name="phoneNumber">O número de telefone para o qual a mensagem WhatsApp será enviada. Deve ser uma string não vazia contendo pelo menos 10 caracteres.</param>
    /// <returns>true se o número de telefone passar na validação e o envio simulado seria bem-sucedido; caso contrário, false.</returns>
    private static bool SimulateWhatsappSending(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 10)
            return false;
        return true;
    }

    /// <summary>
    /// Gera um número inteiro aleatório entre 1 e 10, inclusive.
    /// </summary>
    /// <returns>Um valor inteiro no intervalo de 1 a 10, representando o resultado simulado.</returns>
    private static int RandomSimulateFailure()
    {
        var random = new Random();
        return random.Next(1, 11);
    }
}
