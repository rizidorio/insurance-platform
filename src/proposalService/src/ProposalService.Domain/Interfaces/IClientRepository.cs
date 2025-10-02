using ProposalService.Domain.Entities;
using System.Linq.Expressions;

namespace ProposalService.Domain.Interfaces;

/// <summary>
/// Define métodos para acessar e gerenciar entidades de cliente em um repositório de dados.
/// </summary>
/// <remarks>
/// Implementações desta interface devem fornecer operações assíncronas para recuperar, adicionar e atualizar registros de clientes.
/// Os métodos suportam cancelamento via parâmetro <see cref="CancellationToken"/>.
/// Esta interface é normalmente utilizada para abstrair a lógica de acesso a dados de clientes, permitindo injeção de dependência e testabilidade.
/// </remarks>
public interface IClientRepository
{
    Task<IQueryable<Client>> GetAllAsync(Expression<Func<Client, bool>> filter, CancellationToken cancellationToken);
    /// <summary>
    /// Recupera assíncronamente uma entidade de cliente que corresponde ao número de documento especificado.
    /// </summary>
    /// <param name="documentNumber">O número de documento único utilizado para identificar o cliente. Não pode ser nulo ou vazio.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém a entidade de cliente se encontrada; caso contrário, null.
    /// </returns>
    Task<Client?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera assíncronamente um cliente associado ao identificador externo especificado.
    /// </summary>
    /// <param name="externalId">O identificador externo utilizado para localizar o cliente. Deve ser um <see cref="Guid"/> válido e não vazio.</param>
    /// <param name="cancellationToken">Um token que pode ser utilizado para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o <see cref="Client"/> se encontrado; caso contrário, <see langword="null"/>.
    /// </returns>
    Task<Client?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona assíncronamente o cliente especificado ao repositório de dados.
    /// </summary>
    /// <param name="client">O cliente a ser adicionado. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token para monitorar solicitações de cancelamento. A operação é cancelada se o token for acionado.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona de adição.
    /// </returns>
    Task AddAsync(Client client, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza assíncronamente o registro do cliente especificado.
    /// </summary>
    /// <param name="client">A entidade de cliente a ser atualizada. Não pode ser nula.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação de atualização.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona de atualização.
    /// </returns>
    Task UpdateAsync(Client client, CancellationToken cancellationToken);
}
