namespace NotificationService.Domain.Enums;

/// <summary>
/// Especifica o status de uma notificação dentro do ciclo de vida da notificação.
/// </summary>
/// <remarks>Use esta enumeração para determinar ou definir o estado atual de uma notificação, como se está
/// pendente, foi enviada, falhou ao enviar ou foi cancelada. O status pode ser usado para controlar a lógica de fluxo de trabalho
/// ou exibir informações apropriadas aos usuários.</remarks>
public enum NotificationStatus
{
    /// <summary>
    /// Indica que a operação ou solicitação está aguardando conclusão e ainda não foi processada.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Indica que a mensagem foi enviada, mas ainda não foi entregue ou lida.
    /// </summary>
    Sent = 2,

    /// <summary>
    /// Indica que a operação não foi concluída com sucesso.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Indica que a operação foi cancelada antes da conclusão.
    /// </summary>
    Cancelled = 4
}
