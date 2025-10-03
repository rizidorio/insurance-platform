namespace Insurence.Platform.Common.Notification.Enums;

/// <summary>
/// Especifica o tipo de notificação relacionada a propostas e contratos.
/// </summary>
/// <remarks>Use esta enumeração para identificar o propósito de uma notificação, como criar ou aprovar uma
/// proposta, rejeitar uma proposta ou criar um contrato. Os valores correspondem a eventos distintos de workflow em
/// sistemas de gestão de propostas e contratos.</remarks>
public enum NotificationType
{
    /// <summary>
    /// Representa a ação de criar uma nova proposta.
    /// </summary>
    CreateProposal = 1,

    /// <summary>
    /// Indica que uma proposta foi aprovada.
    /// </summary>
    ApproveProposal = 2,

    /// <summary>
    /// Indica que uma proposta foi rejeitada.
    /// </summary>
    RejectProposal = 3,

    /// <summary>
    /// Indica que a operação é para criar um novo contrato.
    /// </summary>
    CreateContract = 4,
}
