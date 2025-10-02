using ProposalService.Domain.Entities;

namespace ProposalService.Domain.Interfaces.Services;

/// <summary>
/// Define um contrato para validação de uma instância de cliente antes de ser utilizada em operações.
/// </summary>
/// <remarks>
/// Implementações devem realizar verificações para garantir que o cliente fornecido atende a todos os critérios necessários
/// para o processamento subsequente. Falhas de validação podem ser indicadas lançando exceções. Esta interface é normalmente
/// utilizada para impor regras de negócio ou de segurança antes do uso do cliente.
/// </remarks>
public interface IClientValidation
{
    /// <summary>
    /// Valida o cliente especificado para garantir que ele atende a todos os critérios exigidos.
    /// </summary>
    /// <param name="client">A instância do cliente a ser validada. Não pode ser nula.</param>
    void Validate(Client client);
}
