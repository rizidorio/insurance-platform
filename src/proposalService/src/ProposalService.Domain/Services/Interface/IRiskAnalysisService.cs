using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;

namespace ProposalService.Domain.Services.Interface;

/// <summary>
/// Define métodos para anunciar níveis de risco, determinar a necessidade de análise manual e gerar recomendações
/// para propostas no contexto de avaliação de risco de clientes.
/// </summary>
/// <remarks>Implementações desta interface são responsáveis por avaliar propostas e clientes para fornecer
/// decisões e orientações relacionadas ao risco. Os métodos normalmente analisam dados da proposta e informações do cliente para apoiar
/// fluxos de trabalho de gestão de risco. A segurança de thread e os critérios específicos de avaliação dependem da
/// implementação concreta.</remarks>
public interface IRiskAnalysisService
{
    /// <summary>
    /// Determina e anuncia o nível de risco associado a uma proposta especificada para um determinado cliente.
    /// </summary>
    /// <param name="proposal">A proposta a ser avaliada quanto ao risco. Não pode ser nula.</param>
    /// <param name="client">O cliente para quem o nível de risco está sendo avaliado. Não pode ser nulo.</param>
    /// <returns>Um valor RiskLevel indicando o risco avaliado para a proposta e cliente especificados.</returns>
    RiskLevel AnalyzeRiskLevel(Proposal proposal, Client client);

    /// <summary>
    /// Gera uma string de recomendação com base na proposta e nas informações do cliente especificados.
    /// </summary>
    /// <param name="proposal">A proposta contendo detalhes a serem avaliados para gerar uma recomendação. Não pode ser nula.</param>
    /// <param name="client">O cliente para quem a recomendação está sendo gerada. Não pode ser nulo.</param>
    /// <returns>Uma string contendo a recomendação gerada. Retorna uma string vazia se não for possível gerar recomendação.</returns>
    string GeraneteRecommendation(Proposal proposal, Client client);
}
