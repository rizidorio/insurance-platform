using Microsoft.EntityFrameworkCore;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Infrastructure.Data.Context;
using System.Linq.Expressions;

namespace ProposalService.Infrastructure.Data.Repositories;

/// <inheritdoc/>
internal sealed class ClientRepository(
    ProposalDbContext context) : IClientRepository
{
    /// <inheritdoc/>
    public async Task AddAsync(Client client, CancellationToken cancellationToken)
    {
        context.Clients.Add(client);
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IQueryable<Client>> GetAllAsync(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken)
    {
        return Task.FromResult(context.Clients.Where(filter));
    }

    /// <inheritdoc/>
    public async Task<Client?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken)
    {
        return await context.Clients
            .Include(c => c.Proposals)
            .FirstOrDefaultAsync(c => c.DocumentNumber.Value == documentNumber, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Client?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken)
    {
        return await context.Clients
            .Include(c => c.Proposals)
            .FirstOrDefaultAsync(c => c.ExternalId == externalId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Client client, CancellationToken cancellationToken)
    {
        context.Clients.Update(client);
        await context.SaveChangesAsync(cancellationToken);
    }
}
