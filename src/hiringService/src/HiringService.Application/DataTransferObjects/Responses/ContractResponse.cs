using HiringService.Domain.Entities;

namespace HiringService.Application.DataTransferObjects.Responses;

/// <summary>
/// Representa uma resposta contendo detalhes do contrato, incluindo identificadores, datas de vigência e data de criação.
/// </summary>
/// <param name="Id">O identificador único do contrato.</param>
/// <param name="ProposalId">O identificador da proposta associada ao contrato.</param>
/// <param name="ClientId">O identificador do cliente ao qual o contrato se aplica.</param>
/// <param name="EffectiveDateStart">A data e hora em que o contrato entra em vigor.</param>
/// <param name="EffectiveDateEnd">A data e hora em que o contrato expira ou termina.</param>
/// <param name="CreatedAt">A data e hora em que a resposta do contrato foi criada.</param>
public sealed record ContractResponse(
    Guid Id,
    Guid ProposalId,
    Guid ClientId,
    DateTime EffectiveDateStart,
    DateTime EffectiveDateEnd,
    DateTime CreatedAt)
{
    /// <summary>
    /// Converte uma instância de <see cref="Contract"/> em uma <see cref="ContractResponse"/> implicitamente.
    /// </summary>
    /// <remarks>Este operador permite a conversão automática de uma entidade de domínio de contrato para seu modelo de resposta correspondente, normalmente utilizado em respostas de API. Se <paramref name="contract"/> for nulo, pode ocorrer uma <see cref="NullReferenceException"/>.</remarks>
    /// <param name="contract">A entidade de contrato a ser convertida. Não pode ser nula.</param>
    public static implicit operator ContractResponse(Contract contract)
        => new(
            contract.ExternalId,
            contract.ProposalId,
            contract.ClientId,
            contract.EffectiveDateStart,
            contract.EffectiveDateEnd,
            contract.CreatedAt);
}
