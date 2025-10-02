using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Services.Interface;

namespace ProposalService.Domain.Services;

/// <inheritdoc/>
public sealed class RiskAnalysisService : IRiskAnalysisService
{
    /// <inheritdoc/>
    public RiskLevel AnalyzeRiskLevel(Proposal proposal, Client client)
    {
        var score = 0;

        score += AnalyzeByInsuranceType(proposal.InsuranceType);
        score += AnalyzeByCoverageAmount(proposal.Amount);
        score += AnalyzeClientProfile(client);
        score += AnalyzeHistory(client);

        return ClassifyRisk(score);
    }

    /// <inheritdoc/>
    public string GeraneteRecommendation(Proposal proposal, Client client)
    {
        var riskLevel = AnalyzeRiskLevel(proposal, client);

        return riskLevel switch
        {
            RiskLevel.Low => "Aprovar automaticamente a proposta.",
            RiskLevel.Medium => "Revisar a proposta manualmente antes de aprovar.",
            RiskLevel.High => "Rejeitar a proposta automaticamente.",
            _ => "Nível de risco desconhecido. Requer análise adicional."
        };
    }

    /// <summary>
    /// Determina a pontuação de análise associada ao tipo de seguro especificado.
    /// </summary>
    /// <remarks>Se <paramref name="insuranceType"/> não for um valor definido na enumeração <see cref="InsuranceType"/>, uma pontuação padrão de 10 é retornada.</remarks>
    /// <param name="insuranceType">O tipo de seguro a ser analisado. Deve ser um valor válido da enumeração <see cref="InsuranceType"/>.</param>
    /// <returns>Um inteiro representando a pontuação de análise para o tipo de seguro informado.</returns>
    private static int AnalyzeByInsuranceType(InsuranceType insuranceType)
    {
        return insuranceType switch
        {
            InsuranceType.Life => 10,
            InsuranceType.Vehicle => 20,
            InsuranceType.Property => 5,
            InsuranceType.Health => 5,
            _ => 10
        };
    }

    /// <summary>
    /// Determina o período recomendado de análise, em anos, com base no valor da cobertura especificado.
    /// </summary>
    /// <param name="coverageAmount">O valor da cobertura a ser avaliado, em unidades monetárias. Deve ser um valor não negativo.</param>
    /// <returns>Um inteiro representando o número recomendado de anos para análise. Retorna 30 para valores de cobertura superiores a 1.000.000; 20 para valores superiores a 500.000; 10 para valores superiores a 250.000; caso contrário, retorna 5.</returns>
    private static int AnalyzeByCoverageAmount(decimal coverageAmount)
    {
        return coverageAmount switch
        {
            > 1_000_000 => 30,
            > 500_000 => 20,
            > 250_000 => 10,
            _ => 5
        };
    }

    /// <summary>
    /// Avalia o perfil de risco de um cliente com base em sua idade e retorna uma pontuação de risco correspondente.
    /// </summary>
    /// <remarks>Este método atribui pontuações de risco maiores para clientes muito jovens ou muito idosos, conforme determinado pela idade. A pontuação de risco é destinada ao uso em cenários de perfilagem de clientes onde a idade é um fator significativo.</remarks>
    /// <param name="client">O cliente cujo perfil será analisado. Deve possuir uma data de nascimento válida para garantir uma avaliação precisa.</param>
    /// <returns>Um inteiro representando a pontuação de risco do cliente. Valores maiores indicam maior risco com base nos critérios de idade.</returns>
    private static int AnalyzeClientProfile(Client client)
    {
        var age = CalculateAge(client.BirthDate.GetValueOrDefault());
        var points = 0;

        // Clientes muito jovens ou muito idosos têm maior risco
        if (age < 21 || age > 70)
            points += 15;
        else if (age < 25 || age > 65)
            points += 10;

        return points;
    }

    /// <summary>
    /// Analisa o histórico de propostas do cliente especificado e calcula uma pontuação de risco com base na quantidade e status
    /// das propostas.
    /// </summary>
    /// <remarks>Um cliente sem propostas é considerado de maior risco. Cada proposta rejeitada aumenta a
    /// pontuação de risco, enquanto um bom histórico de pelo menos cinco propostas sem rejeições reduz a pontuação.</remarks>
    /// <param name="client">O cliente cujo histórico de propostas será analisado. Não pode ser nulo.</param>
    /// <returns>Um inteiro representando a pontuação de risco calculada para o cliente. A pontuação é zero ou maior; valores mais altos indicam maior risco.</returns>
    private static int AnalyzeHistory(Client client)
    {
        var proposalCount = client.Proposals?.Count ?? 0;
        var rejectedProposals = client.Proposals?
            .Count(p => p.Status == ProposalStatus.Rejected) ?? 0;

        var points = 0;

        // Cliente novo tem maior risco
        if (proposalCount == 0)
            points += 20;

        // Histórico de rejeições aumenta o risco
        if (rejectedProposals > 0)
            points += rejectedProposals * 10;

        // Bom histórico reduz o risco
        if (proposalCount >= 5 && rejectedProposals == 0)
            points -= 10;

        return Math.Max(points, 0);
    }

    /// <summary>
    /// Calcula a idade em anos com base na data de nascimento especificada.
    /// </summary>
    /// <remarks>O cálculo utiliza a data atual em UTC. Se a data de nascimento estiver no futuro, o resultado pode
    /// ser negativo.</remarks>
    /// <param name="birthDate">A data de nascimento para calcular a idade. Deve ser uma data no passado.</param>
    /// <returns>O número de anos completos decorridos desde a data de nascimento informada.</returns>
    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }

    /// <summary>
    /// Classifica o nível de risco com base na pontuação especificada.
    /// </summary>
    /// <param name="score">A pontuação a ser avaliada para classificação de risco. Deve ser um inteiro não negativo.</param>
    /// <returns>Um valor do tipo RiskLevel indicando o risco classificado: Baixo para pontuações menores que 30, Médio para pontuações de 30
    /// a 59, e Alto para pontuações iguais ou superiores a 60.</returns>
    private static RiskLevel ClassifyRisk(int score)
    {
        return score switch
        {
            < 30 => RiskLevel.Low,
            < 60 => RiskLevel.Medium,
            _ => RiskLevel.High
        };
    }
}
