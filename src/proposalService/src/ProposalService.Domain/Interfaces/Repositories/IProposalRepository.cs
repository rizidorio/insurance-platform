using ProposalService.Domain.Entities;
using System.Linq.Expressions;

namespace ProposalService.Domain.Interfaces.Repositories;

/// <summary>
/// Define métodos para acessar e gerenciar entidades de proposta em um repositório de dados.
/// </summary>
/// <remarks>
/// Implementações desta interface fornecem operações assíncronas para consultar, recuperar, adicionar
/// e atualizar propostas. Todos os métodos suportam cancelamento via parâmetro <see cref="CancellationToken"/>.
/// Esta interface é normalmente utilizada para abstrair a lógica de acesso a dados de propostas, permitindo injeção de dependência e testabilidade.
/// </remarks>
public interface IProposalRepository
{
    /// <summary>
    /// Recupera assíncronamente uma coleção consultável de propostas que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <param name="filter">Uma expressão usada para filtrar as propostas a serem recuperadas. Apenas propostas para as quais a expressão retorna
    /// <see langword="true"/> serão incluídas no resultado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um <see
    /// cref="IQueryable{Proposal}"/> representando as propostas filtradas.</returns>
    Task<IQueryable<Proposal>> GetAllAsync(Expression<Func<Proposal, bool>> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera assíncronamente uma proposta associada ao identificador externo especificado.
    /// </summary>
    /// <param name="externalId">O identificador externo usado para localizar a proposta.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a proposta correspondente, se encontrada;
    /// caso contrário, null.</returns>
    Task<Proposal?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona assíncronamente a proposta especificada ao repositório.
    /// </summary>
    /// <param name="proposal">A proposta a ser adicionada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de adição.</returns>
    Task AddAsync(Proposal proposal, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza assíncronamente a proposta especificada no repositório de dados.
    /// </summary>
    /// <param name="proposal">A proposta a ser atualizada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de atualização.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de atualização.</returns>
    Task UpdateAsync(Proposal proposal, CancellationToken cancellationToken);
}