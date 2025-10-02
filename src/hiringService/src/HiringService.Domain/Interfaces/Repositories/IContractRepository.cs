using HiringService.Domain.Entities;
using System.Linq.Expressions;

namespace HiringService.Domain.Interfaces.Repositories;

/// <summary>
/// Define métodos para gerenciar e recuperar entidades de contrato dentro do sistema.
/// </summary>
/// <remarks>
/// A interface IContractService fornece operações assíncronas para consultar, criar e atualizar contratos.
/// As implementações devem suportar cancelamento via os parâmetros CancellationToken fornecidos.
/// Métodos que recuperam contratos permitem filtragem e busca por identificadores de proposta ou externos, facilitando
/// integração com sistemas externos e fluxos de trabalho de negócios.
/// </remarks>
public interface IContractRepository
{
    /// <summary>
    /// Recupera assíncronamente todos os contratos que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <param name="filter">Uma expressão usada para filtrar os contratos a serem recuperados. Apenas contratos para os quais a expressão retorna
    /// <see langword="true"/> são incluídos no resultado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma coleção consultável de contratos
    /// que correspondem aos critérios de filtro.</returns>
    Task<IQueryable<Contract>> GetAllAsync(Expression<Func<Contract, bool>> filter, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera assíncronamente o contrato associado ao identificador de proposta especificado.
    /// </summary>
    /// <param name="proposalId">O identificador único da proposta para a qual recuperar o contrato.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o contrato associado ao
    /// identificador de proposta especificado, ou <see langword="null"/> se nenhum contrato for encontrado.</returns>
    Task<Contract?> GetByProposalIdAsync(Guid proposalId, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera assíncronamente um contrato que corresponde ao identificador externo especificado.
    /// </summary>
    /// <param name="externalId">O identificador externo usado para localizar o contrato. Deve ser um <see cref="Guid"/> válido e não vazio.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o contrato associado ao
    /// identificador externo especificado, ou <see langword="null"/> se nenhum contrato correspondente for encontrado.</returns>
    Task<Contract?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Cria assíncronamente um novo contrato usando os detalhes especificados.
    /// </summary>
    /// <param name="contract">O contrato a ser criado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de criação.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de criação.</returns>
    Task CreateAsync(Contract contract, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza assíncronamente o contrato especificado no repositório de dados.
    /// </summary>
    /// <param name="contract">O contrato a ser atualizado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação de atualização.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de atualização.</returns>
    Task UpdateAsync(Contract contract, CancellationToken cancellationToken);
}
