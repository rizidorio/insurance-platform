using Insurence.Platform.Common.Wrappers;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;

namespace ProposalService.Application.Services.Interfaces;

/// <summary>
/// Define métodos para gerenciar entidades de cliente, incluindo a recuperação de clientes por filtro e a criação de novos clientes
/// de forma assíncrona.
/// </summary>
/// <remarks>Implementações desta interface devem garantir segurança em cenários multi-thread.
/// Todos os métodos são assíncronos e suportam cancelamento via <see cref="CancellationToken"/>.</remarks>
public interface IClientService
{
    /// <summary>
    /// Recupera uma lista paginada de clientes que correspondem aos critérios de filtro especificados de forma assíncrona.
    /// </summary>
    /// <param name="request">Objeto contendo os parâmetros de filtro para busca de clientes. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado contém uma resposta com uma lista paginada de
    /// clientes que correspondem ao filtro.</returns>
    Task<ResponseDefault<PaginatedResponse<ClientResponse>>> GetByFilterAsync(GetClientsFilterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Cria um novo cliente de forma assíncrona usando os dados especificados na requisição.
    /// </summary>
    /// <param name="request">Requisição contendo as informações do cliente a ser criado. Não pode ser nula.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado contém uma resposta com os detalhes do
    /// cliente criado.</returns>
    Task<ResponseDefault<ClientResponse>> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken);
}
