using Microsoft.EntityFrameworkCore;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace ProposalService.Infrastructure.Data.Repositories;

/// <inheritdoc/>
internal sealed class ProposalRepository(
    ProposalDbContext context) : IProposalRepository
{
    /// <inheritdoc/>
    public async Task AddAsync(Proposal proposal, CancellationToken cancellationToken)
    {
        context.Proposals.Add(proposal);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IQueryable<Proposal>> GetAllAsync(Expression<Func<Proposal, bool>> filter, CancellationToken cancellationToken)
    {
        return Task.FromResult(context.Proposals
            .Include(x => x.Client)
            .Where(filter));
    }

    /// <inheritdoc/>
    public async Task<Proposal?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await context.Proposals
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Proposal proposal, CancellationToken cancellationToken)
    {
        context.Proposals.Update(proposal);
        return context.SaveChangesAsync(cancellationToken);
    }
}
