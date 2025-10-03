using NotificationService.Domain.Entities;
using System.Linq.Expressions;

namespace NotificationService.Domain.Interfaces.Repositories;

/// <summary>
/// Define métodos para criar e atualizar entidades de notificação em um repositório de dados de forma assíncrona.
/// </summary>
/// <remarks>Implementações desta interface devem garantir segurança de thread se acessadas concorrentemente. Os métodos
/// aceitam um token de cancelamento para suportar o cancelamento cooperativo de operações assíncronas.</remarks>
public interface INotificationRepository
{
    /// <summary>
    /// Recupera assíncronamente uma coleção consultável de notificações que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <remarks>O <see cref="IQueryable{Notification}"/> retornado permite composição adicional de consultas
    /// antes da execução. A consulta não é executada até que os resultados sejam enumerados. Este método não garante
    /// segurança de thread para o objeto retornado.</remarks>
    /// <param name="filter">Uma expressão usada para filtrar as notificações a serem recuperadas. Apenas notificações que satisfaçam esta condição
    /// serão incluídas no resultado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um <see
    /// cref="IQueryable{Notification}"/> representando as notificações filtradas.</returns>
    Task<IQueryable<Notification>> GetAllAsync(Expression<Func<Notification, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria assíncronamente uma nova notificação usando os dados especificados.
    /// </summary>
    /// <param name="notification">A notificação a ser criada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de criação.</returns>
    Task CreateAsync(Notification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza assíncronamente a notificação especificada com novos dados.
    /// </summary>
    /// <param name="notification">A notificação a ser atualizada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de atualização.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de atualização.</returns>
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
}
