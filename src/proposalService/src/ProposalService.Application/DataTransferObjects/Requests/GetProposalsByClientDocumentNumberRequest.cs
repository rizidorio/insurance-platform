namespace ProposalService.Application.DataTransferObjects.Requests;

public sealed record GetProposalsByClientDocumentNumberRequest(
    string ClientDocumentNumber);
