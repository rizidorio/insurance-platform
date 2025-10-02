namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma solicitação para recuperar uma proposta pelo seu identificador único.
/// </summary>
/// <param name="ProposalId">O identificador único da proposta a ser recuperada.</param>
public sealed record GetProposalByIdRequest(Guid ProposalId);
