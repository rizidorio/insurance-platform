using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;
using ProposalService.Application.Services.Interfaces;
using ProposalService.Application.Validations;
using ProposalService.Domain.Exceptions;

namespace ProposalService.Api.Controllers;

/// <summary>
/// Fornece endpoints de API para gerenciar propostas, incluindo operações para criar, recuperar, aprovar, rejeitar e
/// analisar propostas.
/// </summary>
/// <remarks>Este controlador expõe endpoints RESTful para gerenciamento de propostas e depende de injeção de dependência
/// para seus serviços. Todas as ações retornam objetos de resposta padronizados para garantir consistência nas respostas da API.
/// Os endpoints suportam filtragem, paginação e validação específica de domínio. O tratamento de erros é
/// implementado para fornecer feedback significativo em caso de erros de validação ou de domínio.</remarks>
/// <param name="proposalService">O serviço utilizado para realizar operações relacionadas a propostas, como recuperação, criação, aprovação, rejeição e análise de risco.</param>
public sealed class ProposalController(
    IProposalService proposalService) : MainController<ProposalController>
{
    /// <summary>
    /// Recupera uma lista paginada de propostas que correspondem aos critérios de filtro especificados.
    /// </summary>
    /// <param name="request">Os parâmetros de filtro usados para selecionar propostas. Pode incluir paginação, termos de busca ou outros critérios. Não pode ser nulo.</param>
    /// <param name="cancellationToken">Um token para monitorar solicitações de cancelamento. Opcional.</param>
    /// <returns>Uma resposta HTTP 200 contendo uma lista paginada de propostas encapsulada em um objeto de resposta padrão. Se nenhuma proposta corresponder ao filtro, a lista estará vazia. Retorna uma resposta de erro se a requisição não puder ser processada.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDefault<PaginatedResponse<ProposalResponse>>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<PaginatedResponse<ProposalResponse>>>> GetAllAsync(
        [FromQuery] GetProposalFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await proposalService.GetByFilterAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao listar clientes");
            return CreateResponse(
                ResponseDefault<PaginatedResponse<ProposalResponse>>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Recupera uma proposta pelo seu identificador externo.
    /// </summary>
    /// <remarks>Retorna uma resposta 200 OK se a proposta for encontrada. Se ocorrer um erro durante o processamento,
    /// retorna um erro 500 Internal Server Error com uma mensagem descritiva.</remarks>
    /// <param name="externalId">O identificador externo único da proposta a ser recuperada.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>Um ActionResult contendo um ResponseDefault com os dados da proposta se encontrada; caso contrário, uma resposta de erro apropriada.</returns>
    [HttpGet("{externalId:guid}")]
    [ProducesResponseType(typeof(ResponseDefault<ProposalResponse>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<ProposalResponse>>> GetByExternalIdAsync(
        [FromRoute] Guid externalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetProposalByIdRequest(externalId);
            var response = await proposalService.GetByExternalIdAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao obter proposta por ID");
            return CreateResponse(
                ResponseDefault<ProposalResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Recupera o status de uma proposta identificada pelo ID externo especificado.
    /// </summary>
    /// <remarks>Este método é normalmente utilizado para consultar o status atual de uma proposta usando seu identificador externo.
    /// A resposta inclui detalhes do status se a proposta existir. Se ocorrer um erro durante o processamento,
    /// uma resposta de erro interno do servidor é retornada.</remarks>
    /// <param name="externalId">O identificador externo único da proposta para recuperar o status.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>Um resultado de ação contendo uma resposta com as informações de status da proposta. Retorna uma resposta 200 OK se a
    /// proposta for encontrada; caso contrário, retorna uma resposta de erro.</returns>
    [HttpGet("{externalId:guid}/status")]
    [ProducesResponseType(typeof(ResponseDefault<ProposalStatusResponse>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<ProposalStatusResponse>>> GetStatusByExternalIdAsync(
        [FromRoute] Guid externalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetProposalByIdRequest(externalId);
            var response = await proposalService.GetStatusByExternalIdAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao obter status da proposta por ID");
            return CreateResponse(
                ResponseDefault<ProposalStatusResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Recupera todas as propostas associadas ao número de documento do cliente especificado.
    /// </summary>
    /// <remarks>Este endpoint retorna todas as propostas vinculadas ao número de documento do cliente fornecido. Se nenhuma proposta for encontrada, a resposta conterá uma lista vazia. A operação pode retornar erro interno do servidor se ocorrer uma exceção inesperada.</remarks>
    /// <param name="documentNumber">O número de documento do cliente para o qual as propostas devem ser recuperadas. Deve conter apenas caracteres alfabéticos.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>Um <see cref="ActionResult"/> contendo um ResponseDefault com a lista de propostas para o cliente especificado. Retorna 200 OK com as propostas se encontradas; caso contrário, retorna uma resposta de erro apropriada.</returns>
    [HttpGet("client/{documentNumber}")]
    [ProducesResponseType(typeof(ResponseDefault<IList<ProposalResponse>>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<IList<ProposalResponse>>>> GetByClientDocumentNumberAsync(
        [FromRoute] string documentNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetProposalsByClientDocumentNumberRequest(documentNumber);
            var response = await proposalService.GetByClientDocumentNumberAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao obter propostas por documento do cliente");
            return CreateResponse(
                ResponseDefault<IList<ProposalResponse>>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Recupera a análise de risco para uma proposta e cliente especificados.
    /// </summary>
    /// <remarks>Este método retorna uma resposta 200 OK com os dados da análise de risco se for bem-sucedido. Se ocorrer um erro durante o processamento, uma resposta de erro interno do servidor é retornada. A operação é assíncrona e pode ser cancelada usando o token fornecido.</remarks>
    /// <param name="proposalId">O identificador único da proposta para obter a análise de risco.</param>
    /// <param name="cliendId">O identificador único do cliente associado à proposta.</param>
    /// <param name="cancellationToken">Um token de cancelamento que pode ser usado para cancelar a operação.</param>
    /// <returns>Um resultado de ação contendo uma resposta com os dados da análise de risco para a proposta e cliente especificados. Retorna uma resposta de erro se a operação falhar.</returns>
    [HttpGet("{proposalId:guid}/risk-analysis/{cliendId:guid}")]
    [ProducesResponseType(typeof(ResponseDefault<RiskAnalysisResponse>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public async Task<ActionResult<ResponseDefault<RiskAnalysisResponse>>> GetRiskAnalysisAsync(
        [FromRoute] Guid proposalId,
        [FromRoute] Guid cliendId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new RiskAnalysisRequest(proposalId, cliendId);
            var response = await proposalService.GetRiskAnalysisAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao obter análise de risco");
            return CreateResponse(
                ResponseDefault<RiskAnalysisResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Cria uma nova proposta usando os dados especificados na requisição.
    /// </summary>
    /// <remarks>Retorna uma resposta 201 Created se a proposta for criada com sucesso. Se a validação falhar, retorna uma resposta com os erros de validação. Se ocorrer um erro de domínio ou inesperado, retorna uma resposta de erro apropriada.</remarks>
    /// <param name="request">As informações da proposta a ser criada. Não pode ser nulo e deve conter dados válidos conforme as regras de negócio.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação.</param>
    /// <returns>Um ActionResult contendo um ResponseDefault com os detalhes da proposta criada se bem-sucedido. Retorna informações de validação ou erro se a requisição for inválida ou ocorrer um erro.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDefault<ProposalResponse>), StatusCodes.Status201Created)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
    public async Task<ActionResult<ResponseDefault<ProposalResponse>>> CreateAsync(
        [FromBody] CreateProposalRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = ValidateRequest<CreateProposalRequestValidation, CreateProposalRequest>(request);
            if (!validationResult.IsValid)
            {
                Logger.LogWarning("Ocorreram erros de validação ao criar proposta: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                return ValidationResponseError<ProposalResponse>(validationResult);
            }

            var response = await proposalService.CreateAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (DomainException dex)
        {
            Logger.LogWarning(dex, "Erro de domínio ao criar proposta: {Message}", dex.Message);
            var erros = new List<Error>
            {
                new("DomainError", dex.Message)
            };
            return CreateResponse(ResponseDefault<ProposalResponse>.CreateBadRequestResponse(erros));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao criar proposta");
            return CreateResponse(
                ResponseDefault<ProposalResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Aprova uma proposta identificada pelo ID externo especificado.
    /// </summary>
    /// <remarks>Retorna uma resposta 200 OK com a proposta aprovada em caso de sucesso. Se a proposta não puder ser aprovada devido a erros de validação de domínio, uma resposta 400 Bad Request é retornada com detalhes dos erros. Se ocorrer um erro inesperado, uma resposta 500 Internal Server Error é retornada.</remarks>
    /// <param name="externalId">O identificador único da proposta a ser aprovada.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Um resultado de ação contendo uma resposta com os detalhes da proposta aprovada se bem-sucedido, ou informações de erro se a operação falhar.</returns>
    [HttpPut("{externalId:guid}/approve")]
    [ProducesResponseType(typeof(ResponseDefault<ProposalResponse>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<ResponseDefault<ProposalResponse>>> ApproveAsync(
        [FromRoute] Guid externalId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new ApproveProposalRequest(externalId);
            var response = await proposalService.ApproveAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (DomainException dex)
        {
            Logger.LogWarning(dex, "Erro de domínio ao aprovar proposta: {Message}", dex.Message);
            var erros = new List<Error>
            {
                new("DomainError", dex.Message)
            };
            return CreateResponse(ResponseDefault<ProposalResponse>.CreateBadRequestResponse(erros));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao aprovar proposta");
            return CreateResponse(
                ResponseDefault<ProposalResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }

    /// <summary>
    /// Rejeita uma proposta identificada pelo ID externo especificado.
    /// </summary>
    /// <remarks>Esta operação define o ID da proposta na requisição para corresponder ao ID externo fornecido na rota. Se a validação de domínio falhar, a resposta incluirá detalhes dos erros. Se ocorrer um erro inesperado, uma mensagem de erro genérica será retornada. O método é destinado ao uso como endpoint de API e segue as convenções padrão do HTTP PUT.</remarks>
    /// <param name="externalId">O identificador único da proposta a ser rejeitada.</param>
    /// <param name="request">Os dados da requisição contendo os detalhes necessários para rejeitar a proposta.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação.</param>
    /// <returns>Um resultado de ação contendo uma resposta com os detalhes da proposta rejeitada. Retorna uma resposta Bad Request se a rejeição falhar devido a erros de validação de domínio, ou uma resposta Internal Server Error se ocorrer um erro inesperado.</returns>
    [HttpPut("{externalId:guid}/reject")]
    [ProducesResponseType(typeof(ResponseDefault<ProposalResponse>), StatusCodes.Status200OK)]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public async Task<ActionResult<ResponseDefault<ProposalResponse>>> RejectAsync(
        [FromRoute] Guid externalId,
        [FromBody] RejectProposalRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            request.ProposalId = externalId;
            var response = await proposalService.RejectAsync(request, cancellationToken);
            return CreateResponse(response);
        }
        catch (DomainException dex)
        {
            Logger.LogWarning(dex, "Erro de domínio ao rejeitar proposta: {Message}", dex.Message);
            var erros = new List<Error>
            {
                new("DomainError", dex.Message)
            };
            return CreateResponse(ResponseDefault<ProposalResponse>.CreateBadRequestResponse(erros));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao rejeitar proposta");
            return CreateResponse(
                ResponseDefault<ProposalResponse>.CreateInternalServerErrorResponse("Ocorreu um erro ao processar sua requisição."));
        }
    }
}
