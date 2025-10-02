using HiringService.Domain.Entities;
using HiringService.Domain.Interfaces.Repositories;
using HiringService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HiringService.Infrastructure.Data.Repositories;

/// <inheritdoc/>
public sealed class ContractRepository(
    HiringDbContext context) : IContractRepository
{
    /// <inheritdoc/>
    public async Task CreateAsync(Contract contract, CancellationToken cancellationToken)
    {
        context.Contracts.Add(contract);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IQueryable<Contract>> GetAllAsync(Expression<Func<Contract, bool>> filter, CancellationToken cancellationToken)
    {
        return Task.FromResult(context.Contracts
            .Where(filter));
    }

    /// <inheritdoc/>
    public async Task<Contract?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await context.Contracts
            .FirstOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Contract?> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken)
    {
        return await context.Contracts
            .FirstOrDefaultAsync(x => x.ProposalId == proposalId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Contract contract, CancellationToken cancellationToken)
    {
        context.Contracts.Update(contract);
        await context.SaveChangesAsync(cancellationToken);
    }
}
