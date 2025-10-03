using ProposalService.Domain.Entities;
using ProposalService.Domain.Extensions;

namespace ProposalService.Application.DataTransferObjects.Responses;

/// <summary>
/// Representa os dados de resposta para uma proposta de seguro de cliente, incluindo detalhes do cliente, status da proposta e
/// timestamps relevantes.
/// </summary>
/// <param name="Id">O identificador único da resposta da proposta.</param>
/// <param name="Client">O cliente associado à proposta. Pode ser <see langword="null"/> se os detalhes do cliente não estiverem disponíveis.</param>
/// <param name="Amount">O valor monetário da proposta de seguro.</param>
/// <param name="InsuranceType">O tipo de seguro proposto, representado como nome de exibição.</param>
/// <param name="Status">O status atual da proposta, representado como nome de exibição.</param>
/// <param name="CreatedAt">A data e hora em que a proposta foi criada.</param>
/// <param name="UpdatedAt">A data e hora em que a proposta foi atualizada pela última vez. Pode ser <see langword="null"/> se nunca foi atualizada.</param>
public sealed record ProposalResponse(
    Guid Id,
    ClientResponse? Client,
    decimal Amount,
    string InsuranceType,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    /// <summary>
    /// Converte uma instância de <see cref="Proposal"/> em <see cref="ProposalResponse"/> implicitamente.
    /// </summary>
    /// <remarks>Este operador permite conversão automática de uma entidade de proposta do domínio para seu modelo de resposta correspondente, mapeando as propriedades relevantes. Se qualquer propriedade relacionada ao cliente for nula, valores padrão são usados na resposta.</remarks>
    /// <param name="proposal">A entidade Proposal a ser convertida. Não pode ser nula.</param>
    public static implicit operator ProposalResponse(Proposal proposal)
        => new(
            proposal.ExternalId,
            (ClientResponse)proposal.Client,
            proposal.Amount,
            proposal.InsuranceType.ToDisplayName(),
            proposal.Status.ToDisplayName(),
            proposal.CreatedAt,
            proposal.UpdatedAt);
}
