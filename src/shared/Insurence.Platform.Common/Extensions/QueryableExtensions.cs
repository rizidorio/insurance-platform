using Insurence.Platform.Common.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Insurence.Platform.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResponse<TEntity>> ToPaginatedListAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var count = await queryable.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return new PaginatedResponse<TEntity>(items, count, pageNumber, pageSize);
    }

    public static Task<PaginatedResponse<TEntity>> ToPaginatedList<TEntity>(
        this IQueryable<TEntity> queryable,
        int pageNumber = 1,
        int pageSize = 10)
        where TEntity : class
    {
        var count = queryable.Count();
        var items = queryable.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var result = new PaginatedResponse<TEntity>(items, count, pageNumber, pageSize);
        return Task.FromResult(result);
    }
}
