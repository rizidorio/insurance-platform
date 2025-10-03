namespace ProposalService.Domain.Enums;

/// <summary>
/// Especifica o nível de risco associado a uma operação ou entidade.
/// </summary>
/// <remarks>Use esta enumeração para indicar a severidade ou probabilidade de risco, como em avaliações de segurança,
/// avaliações financeiras ou decisões operacionais. Os valores representam níveis crescentes de risco, com <see
/// cref="RiskLevel.Low"/> indicando risco mínimo e <see cref="RiskLevel.High"/> indicando risco significativo.</remarks>
public enum RiskLevel
{
    /// <summary>
    /// Indica um nível baixo ou configuração de prioridade baixa.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Representa um nível médio ou valor intermediário dentro da enumeração.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// Representa um nível de prioridade alto.
    /// </summary>
    High = 2
}
