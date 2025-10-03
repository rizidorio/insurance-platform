namespace HiringService.Application.DataTransferObjects.Requests;

public sealed record GetContractsByFilterRequest(
    Guid? ClientId = null,
    Guid? ProposalId = null,
    DateTime? CreatedAtStart = null,
    DateTime? CreatedAtEnd = null,
    bool? IsActive = null,
    int PageNumber = 1,
    int PageSize = 10);
