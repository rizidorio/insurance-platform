using NotificationService.Application.DataTransferObjects.Requests;
using NotificationService.Application.DataTransferObjects.Responses;

namespace NotificationService.Application.Services.Interfaces;

/// <summary>
/// Define um serviço para envio de notificações de forma assíncrona.
/// </summary>
/// <remarks>
/// Implementações desta interface devem lidar com a entrega de notificações conforme especificado na requisição.
/// A operação é assíncrona e pode ser cancelada via o token de cancelamento fornecido.
/// </remarks>
public interface ISendNotificationService
{
    /// <summary>
    /// Envia uma notificação de forma assíncrona utilizando os parâmetros especificados na requisição.
    /// </summary>
    /// <param name="request">A requisição de notificação contendo os detalhes da mensagem e informações do destinatário. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação de envio. O valor padrão é <see cref="CancellationToken.None"/>.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona de envio. O resultado da tarefa contém um <see cref="SendNotificationResponse"/> indicando o resultado da solicitação de envio da notificação.
    /// </returns>
    Task<SendNotificationResponse> SendAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);
}
