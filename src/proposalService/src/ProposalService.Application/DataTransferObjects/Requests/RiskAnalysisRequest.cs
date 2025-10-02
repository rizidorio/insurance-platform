namespace ProposalService.Application.DataTransferObjects.Requests;

public sealed record RiskAnalysisRequest(
    Guid ProposalId,
    Guid ClientId);
