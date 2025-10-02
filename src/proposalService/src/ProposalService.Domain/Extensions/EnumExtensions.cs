using ProposalService.Domain.Enums;

namespace ProposalService.Domain.Extensions;

/// <summary>
/// Fornece métodos de extensão para converter valores de enumeração em seus respectivos nomes de exibição.
/// </summary>
/// <remarks>Os nomes de exibição retornados por estes métodos são strings localizadas, destinadas a cenários voltados ao usuário.
/// Essas extensões simplificam o mapeamento de valores enum para rótulos legíveis, o que pode ser útil para apresentação em UI ou
/// relatórios. Se um valor desconhecido ou indefinido for fornecido, os métodos retornam um nome de exibição padrão indicando
/// que o valor é desconhecido.</remarks>
public static class EnumExtensions
{
    /// <summary>
    /// Retorna um nome de exibição amigável para o status de proposta especificado.
    /// </summary>
    /// <param name="proposalStatus">O valor do status da proposta a ser convertido em nome de exibição.</param>
    /// <returns>Uma string contendo o nome de exibição correspondente ao status de proposta especificado. Retorna "Desconhecido" se
    /// o status não for reconhecido.</returns>
    public static string ToDisplayName(this ProposalStatus proposalStatus)
    {
        return proposalStatus switch
        {
            ProposalStatus.InAnalysis => "Em Análise",
            ProposalStatus.Approved => "Aprovada",
            ProposalStatus.Rejected => "Rejeitada",
            _ => "Desconhecido"
        };
    }

    /// <summary>
    /// Retorna um nome de exibição localizado para o nível de risco especificado.
    /// </summary>
    /// <remarks>Os nomes de exibição retornados estão localizados em português. Este método é útil para apresentar
    /// níveis de risco em interfaces de usuário ou relatórios.</remarks>
    /// <param name="riskLevel">O nível de risco a ser convertido em nome de exibição. Deve ser um valor válido da enumeração <see cref="RiskLevel"/>.</param>
    /// <returns>Uma string contendo o nome de exibição para o nível de risco informado. Retorna "Desconhecido" se o nível de risco não for
    /// reconhecido.</returns>
    public static string ToDisplayName(this RiskLevel riskLevel)
    {
        return riskLevel switch
        {
            RiskLevel.Low => "Baixo",
            RiskLevel.Medium => "Médio",
            RiskLevel.High => "Alto",
            _ => "Desconhecido"
        };
    }

    /// <summary>
    /// Retorna o nome de exibição localizado para o tipo de seguro especificado.
    /// </summary>
    /// <param name="insuranceType">O tipo de seguro para o qual recuperar o nome de exibição.</param>
    /// <returns>Uma string contendo o nome de exibição para o tipo de seguro informado. Retorna "Desconhecido" se o tipo de seguro
    /// não for reconhecido.</returns>
    public static string ToDisplayName(this InsuranceType insuranceType)
    {
        return insuranceType switch
        {
            InsuranceType.Life => "Vida",
            InsuranceType.Vehicle => "Veículo",
            InsuranceType.Property => "Propriedade",
            InsuranceType.Health => "Saúde",
            _ => "Desconhecido"
        };
    }
}
