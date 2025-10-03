using ProposalService.Domain.Enums;

namespace ProposalService.Application.DataTransferObjects.Responses;

/// <summary>
/// Representa as informações de status de uma proposta, incluindo seu identificador externo, cliente associado e status atual.
/// </summary>
/// <param name="ExternalId">O identificador externo único da proposta. Usado para correlacionar com sistemas ou referências externas.</param>
/// <param name="ClientId">O identificador único do cliente associado à proposta.</param>
/// <param name="Status">O status atual da proposta.</param>
public sealed record ProposalStatusResponse(
    Guid ExternalId,
    Guid ClientId,
    ProposalStatus Status);
