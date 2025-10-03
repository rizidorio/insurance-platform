using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces.Repositories;
using NotificationService.Infrastructure.Data.Contexts;
using System.Linq.Expressions;

namespace NotificationService.Infrastructure.Data.Repositories;

/// <inheritdoc/>
public sealed class NotificationRepository(
    NotificationDbContext dbContext) : INotificationRepository
{
    /// <inheritdoc/>
    public async Task CreateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<IQueryable<Notification>> GetAllAsync(Expression<Func<Notification, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(dbContext.Notifications.Where(filter));
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        dbContext.Notifications.Update(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
