namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma solicitação para aprovar uma proposta identificada por seu ID exclusivo.
/// </summary>
/// <param name="ProposalId">O identificador exclusivo da proposta a ser aprovada. Não deve ser um GUID vazio.</param>
public sealed record ApproveProposalRequest(Guid ProposalId);
