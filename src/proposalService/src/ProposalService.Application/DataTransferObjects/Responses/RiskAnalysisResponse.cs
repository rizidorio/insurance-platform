namespace ProposalService.Application.DataTransferObjects.Responses;

public sealed record RiskAnalysisResponse(
    string RiskLevel,
    string Recommendation);
