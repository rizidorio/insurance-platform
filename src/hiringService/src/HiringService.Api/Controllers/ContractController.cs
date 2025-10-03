using HiringService.Application.DataTransferObjects.Requests;
using HiringService.Application.DataTransferObjects.Responses;
using HiringService.Application.Services.Interfaces;
using HiringService.Application.Validations;
using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace HinringService.Api.Controllers;

/// <summary>
/// Fornece endpoints de API para gerenciar contratos, incluindo operações para recuperar e criar registros de contratos.
/// </summary>
/// <remarks>Este controlador expõe endpoints para gerenciamento de contratos, seguindo convenções RESTful. Todas as ações
/// exigem modelos de requisição apropriados e podem retornar respostas de validação ou erro dependendo dos dados de entrada e do processamento.</remarks>
/// <param name="contractService">O serviço utilizado para realizar operações relacionadas a contratos, como consulta e criação de contratos.</param>
public sealed class ContractController(
    IContractService contractService) : MainController<ContractController>
{
    /// <summary>
    /// Recupera uma lista paginada de contratos que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <remarks>Se ocorrer um erro ao processar a requisição, a resposta conterá uma mensagem de erro interno do servidor.</remarks>
    /// <param name="request">Os parâmetros de filtro utilizados para selecionar contratos. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser utilizado para cancelar a operação.</param>
    /// <returns>Uma resposta HTTP contendo uma lista paginada de contratos encapsulada em um objeto de resposta padrão. Retorna status
    /// 200 se bem-sucedido.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDefault<PaginatedResponse<ContractResponse>>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<PaginatedResponse<ContractResponse>>>> GetByFilterAsync([
        FromQuery] GetContractsByFilterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await contractService.GetByFilterAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao listar contratos");
            return CreateResponse(
                ResponseDefault<PaginatedResponse<ContractResponse>>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Cria um novo contrato utilizando os dados especificados na requisição.
    /// </summary>
    /// <remarks>Retorna uma resposta 201 Created em caso de sucesso. Se os dados da requisição forem inválidos, uma resposta de erro de validação
    /// é retornada. Em caso de erro interno do servidor, uma resposta genérica de erro é fornecida.</remarks>
    /// <param name="request">Os detalhes para criação do contrato a serem enviados. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token que pode ser utilizado para cancelar a operação.</param>
    /// <returns>Um ActionResult contendo um ResponseDefault com as informações do contrato criado se bem-sucedido, ou uma resposta de erro
    /// se a validação falhar ou ocorrer um erro interno.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDefault<ContractResponse>), StatusCodes.Status201Created)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<ActionResult<ResponseDefault<ContractResponse>>> CreateAsync(
        [FromBody] CreateContractRequest request, 
        CancellationToken cancellationToken)
    {
        try
        {   
            var validationResult = ValidateRequest<CreateContractRequestValidation, CreateContractRequest>(request);
            if (!validationResult.IsValid)
            {
                Logger.LogWarning("Ocorreram erros de validação ao criar cliente: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                return ValidationResponseError<ContractResponse>(validationResult);
            }

            var response = await contractService.CreateAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao criar contrato");
            return CreateResponse(
                ResponseDefault<ContractResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }
}
