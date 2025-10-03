using ProposalService.Domain.Entities;

namespace ProposalService.Domain.Interfaces.Services;

/// <summary>
/// Define um contrato para validação de objetos Proposal antes de serem processados ou aceitos.
/// </summary>
/// <remarks>
/// Implementações devem realizar todas as verificações necessárias para garantir que a proposta fornecida atende
/// às regras de negócio ou restrições exigidas. Falhas de validação normalmente são indicadas por exceções.
/// Esta interface permite a injeção de lógica de validação personalizada onde a integridade da proposta é crítica.
/// </remarks>
public interface IProposalValidation
{
    /// <summary>
    /// Valida a proposta especificada para garantir que ela atende a todos os critérios exigidos.
    /// </summary>
    /// <param name="proposal">A proposta a ser validada. Não pode ser nula.</param>
    void Validate(Proposal proposal);
}
