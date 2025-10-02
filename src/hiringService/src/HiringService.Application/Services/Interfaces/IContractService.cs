using HiringService.Application.DataTransferObjects.Requests;
using HiringService.Application.DataTransferObjects.Responses;
using Insurence.Platform.Common.Wrappers;

namespace HiringService.Application.Services.Interfaces;

/// <summary>
/// Define operações para recuperar e criar registros de contratos.
/// </summary>
/// <remarks>
/// Implementações desta interface devem fornecer métodos assíncronos para consultar contratos por filtro e criar novos contratos.
/// Todos os métodos são projetados para suportar cancelamento via <see cref="CancellationToken"/>.
/// </remarks>
public interface IContractService
{
    /// <summary>
    /// Recupera assíncronamente uma lista paginada de contratos que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <param name="request">Objeto contendo os parâmetros de filtro a serem aplicados na busca por contratos. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com uma lista paginada de dados de contratos que correspondem aos critérios de filtro.
    /// </returns>
    Task<ResponseDefault<PaginatedResponse<ContractResponse>>> GetByFilterAsync(GetContractsByFilterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Cria assíncronamente um novo contrato usando os dados especificados na requisição.
    /// </summary>
    /// <param name="request">Objeto de requisição contendo os detalhes necessários para criar o contrato. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>
    /// Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma resposta com os detalhes do contrato criado.
    /// </returns>
    Task<ResponseDefault<ContractResponse>> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken);
}
