using System.Text.Json.Serialization;

namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma requisição para rejeitar uma proposta, incluindo o identificador da proposta e o motivo da rejeição.
/// </summary>
/// <param name="ProposalId">O identificador único da proposta a ser rejeitada.</param>
/// <param name="Reason">O motivo da rejeição da proposta. Este valor deve fornecer detalhes suficientes para auditoria ou revisão.</param>
public sealed record RejectProposalRequest(string Reason)
{
    /// <summary>
    /// Obtém ou define o identificador único da proposta.
    /// </summary>
    [JsonIgnore]
    public Guid ProposalId { get; set; }
}
