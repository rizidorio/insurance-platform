namespace HiringService.Domain.Enums;

/// <summary>
/// Especifica os possíveis status para uma proposta, como em análise, aprovada ou rejeitada.
/// </summary>
/// <remarks>Use esta enumeração para representar o estado atual de uma proposta em fluxos de trabalho ou lógica de negócios. Os valores correspondem a etapas distintas no processo de revisão da proposta.</remarks>
public enum ProposalStatus
{
    /// <summary>
    /// Indica que o item está atualmente em análise.
    /// </summary>
    InAnalysis = 0,

    /// <summary>
    /// Indica que o item foi aprovado.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Indica que o item foi rejeitado.
    /// </summary>
    Rejected = 2
}