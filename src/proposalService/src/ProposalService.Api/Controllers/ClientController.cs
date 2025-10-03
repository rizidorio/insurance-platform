using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;
using ProposalService.Application.Services.Interfaces;
using ProposalService.Application.Validations;
using ProposalService.Domain.Exceptions;


namespace ProposalService.Api.Controllers;

/// <summary>
/// Fornece endpoints de API para gerenciar entidades de cliente, incluindo operações para recuperar e criar clientes.
/// </summary>
/// <remarks>Este controlador expõe endpoints RESTful para gerenciamento de clientes. Todas as ações requerem autorização adequada e podem retornar formatos de resposta padrão da API, incluindo detalhes de erro para operações falhas.</remarks>
/// <param name="clientService">O serviço utilizado para executar operações relacionadas ao cliente, como recuperar e criar registros de clientes.</param>
public sealed class ClientController(
    IClientService clientService) : MainController<ClientController>
{
    /// <summary>
    /// Recupera uma lista paginada de clientes que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <remarks>Se ocorrer um erro ao processar a requisição, uma resposta de erro interno do servidor é retornada. A resposta inclui detalhes de paginação e quaisquer filtros aplicados.</remarks>
    /// <param name="request">Os parâmetros de filtro para aplicar ao recuperar clientes. Pode incluir paginação, termos de busca ou outros critérios. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação.</param>
    /// <returns>Um resultado de ação contendo uma resposta com uma lista paginada de clientes. Retorna um 200 OK com os resultados, ou uma resposta de erro se a operação falhar.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDefault<PaginatedResponse<ClientResponse>>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<PaginatedResponse<ClientResponse>>>> GetAllAsync(
        [FromQuery] GetClientsFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await clientService.GetByFilterAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao listar clientes");
            return CreateResponse(
                ResponseDefault<PaginatedResponse<ClientResponse>>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Cria um novo cliente utilizando os dados especificados na requisição.
    /// </summary>
    /// <remarks>Esta ação retorna um 201 Created em caso de sucesso. Se ocorrer um erro de validação de domínio, retorna um 400 Bad Request com detalhes do erro. Para erros inesperados, retorna um 500 Internal Server Error.</remarks>
    /// <param name="request">O objeto de requisição contendo as informações necessárias para criar o cliente. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação.</param>
    /// <returns>Um ActionResult contendo um ResponseDefault com os detalhes do cliente criado, se bem-sucedido. Retorna uma resposta de bad request se a requisição for inválida, ou uma resposta de erro interno do servidor se ocorrer um erro inesperado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDefault<ClientResponse>), StatusCodes.Status201Created)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<ActionResult<ResponseDefault<ClientResponse>>> CreateAsync(
        [FromBody] CreateClientRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = ValidateRequest<CreateClientRequestValidation, CreateClientRequest>(request);
            if (!validationResult.IsValid)
            {
                Logger.LogWarning("Ocorreram erros de validação ao criar cliente: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                return ValidationResponseError<ClientResponse>(validationResult);
            }

            var response = await clientService.CreateAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (DomainException dex)
        {
            Logger.LogWarning(dex, "Erro de domínio ao criar cliente: {Message}", dex.Message);
            var erros = new List<Error>
            {
                new("DomainError", dex.Message)
            };
            return CreateResponse(ResponseDefault<ClientResponse>.CreateBadRequestResponse(erros));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao criar cliente");
            return CreateResponse(
                ResponseDefault<ClientResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }
}