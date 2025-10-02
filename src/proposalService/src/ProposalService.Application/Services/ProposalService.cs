using Insurence.Platform.Common.Extensions;
using Insurence.Platform.Common.Helpers;
using Insurence.Platform.Common.Wrappers;
using Microsoft.Extensions.Logging;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;
using ProposalService.Application.Services.Interfaces;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Extensions;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace ProposalService.Application.Services;

/// <inheritdoc/>
public sealed class ProposalService(
    IProposalValidation proposalValidation,
    IClientValidation clientValidation,
    IRiskAnalysisService riskAnalysisService,
    IClientRepository clientRepository,
    IProposalRepository proposalRepository,
    ILogger<ProposalService> logger) : IProposalService
{
    /// <inheritdoc/>
    public async Task<ResponseDefault<ProposalResponse>> ApproveAsync(ApproveProposalRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalRepository.GetByExternalIdAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada para aprovação.", request.ProposalId);
            return ResponseDefault<ProposalResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        if (proposal.Status != ProposalStatus.InAnalysis)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não está em estado 'em analise' e não pode ser aprovada. Estado atual: {Status}.", request.ProposalId, proposal.Status);
            return ResponseDefault<ProposalResponse>.CreateConflictResponse("A proposta deve estar em estado 'em analise' para ser aprovada.");
        }

        proposal.Approve();
        await proposalRepository.UpdateAsync(proposal, cancellationToken);

        logger.LogInformation("Proposta com ID {ProposalId} aprovada com sucesso.", request.ProposalId);
        return ResponseDefault<ProposalResponse>.CreateSuccessResponse((ProposalResponse)proposal);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<ProposalResponse>> CreateAsync(CreateProposalRequest request, CancellationToken cancellationToken)
    {
        Client? client;
        if (request.ClientId.HasValue)
        {
            client = await clientRepository.GetByExternalIdAsync(request.ClientId.Value, cancellationToken);

            if (client is null)
            {
                logger.LogWarning("Cliente com ID {ClientId} não encontrado ao criar proposta.", request.ClientId);
                return ResponseDefault<ProposalResponse>.CreateNotFoundResponse("Cliente não encontrado.");
            }
        }
        else
        {
            client = Client.Create(
                request.ClientName!,
                request.ClientDocumentNumber!,
                request.ClientEmail!,
                request.ClientBirthDate!);
            clientValidation.Validate(client);
            await clientRepository.AddAsync(client, cancellationToken);
        }

        var proposal = Proposal.Create(
            client.Id,
            request.InsuranceType,
            request.Amount);
        proposalValidation.Validate(proposal);
                
        var riskLevel = riskAnalysisService.AnalyzeRiskLevel(proposal, client);
        var recomendation = riskAnalysisService.GeraneteRecommendation(proposal, client);
        logger.LogInformation("Nível de risco analisado: {RiskLevel}. Recomendação: {Recomendation}", riskLevel, recomendation);

        switch (riskLevel)
        {
            case RiskLevel.Low:
                proposal.Approve();
                break;
            case RiskLevel.Medium:
                proposal.SetInAnalysis();
                break;
            case RiskLevel.High:
                proposal.Reject(recomendation);
                break;
            default:
                logger.LogError("Nível de risco desconhecido retornado pela análise de risco. Cliente ID: {ClientId}, Proposta ID: {ProposalId}", client.Id, proposal.ExternalId);
                var erros = new List<Error> { new Error("RiskAnalysis", "Nível de risco desconhecido retornado pela análise de risco.") };
                return ResponseDefault<ProposalResponse>.CreateBadRequestResponse(erros, "Erro ao analisar o nível de risco da proposta.");
        }

        await proposalRepository.AddAsync(proposal, cancellationToken);

        logger.LogInformation("Proposta com ID {ProposalId} criada com sucesso para o cliente ID {ClientId}.", proposal.ExternalId, client.Id);

        return ResponseDefault<ProposalResponse>.CreateSuccessResponse((ProposalResponse)proposal);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<IList<ProposalResponse>>> GetByClientDocumentNumberAsync(GetProposalsByClientDocumentNumberRequest request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByDocumentNumberAsync(request.ClientDocumentNumber, cancellationToken);

        if (client is null)
        {
            logger.LogWarning("Cliente com número de documento {DocumentNumber} não encontrado ao buscar propostas.", request.ClientDocumentNumber);
            return ResponseDefault<IList<ProposalResponse>>.CreateNotFoundResponse("Cliente não encontrado.");
        }

        var proposals = await proposalRepository.GetAllAsync(p => p.ClientId == client.Id, cancellationToken);

        var proposalResponses = proposals.ToList().ConvertAll(p => (ProposalResponse)p);

        logger.LogInformation("Recuperadas {Count} propostas para o cliente com número de documento {DocumentNumber}.", proposalResponses.Count, request.ClientDocumentNumber);

        return ResponseDefault<IList<ProposalResponse>>.CreateSuccessResponse(proposalResponses);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<ProposalResponse>> GetByExternalIdAsync(GetProposalByIdRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalRepository.GetByExternalIdAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada.", request.ProposalId);
            return ResponseDefault<ProposalResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        logger.LogInformation("Proposta com ID {ProposalId} recuperada com sucesso.", request.ProposalId);
        return ResponseDefault<ProposalResponse>.CreateSuccessResponse((ProposalResponse)proposal);
    }

    public async Task<ResponseDefault<ProposalStatusResponse>> GetStatusByExternalIdAsync(GetProposalByIdRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalRepository.GetByExternalIdAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada ao buscar status.", request.ProposalId);
            return ResponseDefault<ProposalStatusResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        var response = new ProposalStatusResponse(
            ExternalId: proposal.ExternalId,
            ClientId: proposal.Client?.ExternalId ?? Guid.Empty,
            Status: proposal.Status);

        logger.LogInformation("Status da proposta com ID {ProposalId} recuperado com sucesso. Status: {Status}.", request.ProposalId, proposal.Status);
        return ResponseDefault<ProposalStatusResponse>.CreateSuccessResponse(response);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<PaginatedResponse<ProposalResponse>>> GetByFilterAsync(GetProposalFilterRequest request, CancellationToken cancellationToken)
    {
        Client? client = null;
        if (request.ClientId.HasValue)
        {
            client = await clientRepository.GetByExternalIdAsync(request.ClientId.Value, cancellationToken);
            if (client is null)
            {
                logger.LogWarning("Cliente com ID {ClientId} não encontrado ao filtrar propostas.", request.ClientId);
                return ResponseDefault<PaginatedResponse<ProposalResponse>>.CreateNotFoundResponse("Cliente não encontrado.");
            }
        }

        var filter = BuildFilterExpression(request, client?.Id);

        var proposalsQuery = await proposalRepository.GetAllAsync(filter, cancellationToken);

        var paginatedResults = await proposalsQuery.ToPaginatedListAsync(
            request.Page ?? 1, 
            request.PageSize ?? 10, 
            cancellationToken);
        logger.LogInformation("Recuperadas {Count} propostas com filtro aplicado. Página {Page} de {TotalPages}.", paginatedResults.Items?.Count, paginatedResults.CurrentPage, paginatedResults.TotalPages);
        
        var paginatedResponse = new PaginatedResponse<ProposalResponse>(
            items: paginatedResults.Items?.ToList().ConvertAll(p => (ProposalResponse)p),
            totalItems: paginatedResults.TotalItems,
            currentPage: paginatedResults.CurrentPage,
            pageSize: paginatedResults.PageSize);

        return ResponseDefault<PaginatedResponse<ProposalResponse>>.CreateSuccessResponse(paginatedResponse);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<RiskAnalysisResponse>> GetRiskAnalysisAsync(RiskAnalysisRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalRepository.GetByExternalIdAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada para análise de risco.", request.ProposalId);
            return ResponseDefault<RiskAnalysisResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        var client = await clientRepository.GetByExternalIdAsync(request.ClientId, cancellationToken);

        if (client is null)
        {
            logger.LogWarning("Cliente com ID {ClientId} não encontrado para análise de risco.", request.ClientId);
            return ResponseDefault<RiskAnalysisResponse>.CreateNotFoundResponse("Cliente não encontrado.");
        }

        var riskLevel = riskAnalysisService.AnalyzeRiskLevel(proposal, client);
        var recomendation = riskAnalysisService.GeraneteRecommendation(proposal, client);

        var response = new RiskAnalysisResponse(
            RiskLevel: riskLevel.ToDisplayName(),
            Recommendation: recomendation);

        logger.LogInformation("Análise de risco realizada para a proposta ID {ProposalId}. Nível de risco: {RiskLevel}, Recomendação: {Recomendation}", request.ProposalId, riskLevel, recomendation);

        return ResponseDefault<RiskAnalysisResponse>.CreateSuccessResponse(response);
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<ProposalResponse>> RejectAsync(RejectProposalRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalRepository.GetByExternalIdAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada para rejeição.", request.ProposalId);
            return ResponseDefault<ProposalResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        if (proposal.Status != ProposalStatus.InAnalysis)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não está em estado 'em analise' e não pode ser rejeitada. Estado atual: {Status}.", request.ProposalId, proposal.Status);
            return ResponseDefault<ProposalResponse>.CreateConflictResponse("A proposta deve estar em estado 'em analise' para ser rejeitada.");
        }

        proposal.Reject(request.Reason);
        await proposalRepository.UpdateAsync(proposal, cancellationToken);

        logger.LogInformation("Proposta com ID {ProposalId} rejeitada com sucesso.", request.ProposalId);

        return ResponseDefault<ProposalResponse>.CreateSuccessResponse((ProposalResponse)proposal);
    }

    /// <summary>
    /// Constrói uma expressão LINQ que filtra propostas com base nos critérios de filtro especificados.
    /// </summary>
    /// <remarks>A expressão retornada pode ser usada diretamente em consultas LINQ para filtrar propostas
    /// de forma eficiente em uma fonte de dados. Apenas critérios com valores não nulos ou não vazios são incluídos no filtro.</remarks>
    /// <param name="request">Os critérios de filtro a serem aplicados ao selecionar propostas. As propriedades deste request determinam quais condições
    /// serão incluídas na expressão resultante.</param>
    /// <param name="clientId">Um identificador de cliente opcional para filtrar propostas por cliente. Se especificado, apenas propostas associadas a este
    /// cliente serão incluídas.</param>
    /// <returns>Uma expressão que pode ser usada para filtrar propostas de acordo com os critérios fornecidos. A expressão retorna
    /// <see langword="true"/> para propostas que correspondem a todos os filtros especificados; caso contrário, <see langword="false"/>.</returns>
    private static Expression<Func<Proposal, bool>> BuildFilterExpression(GetProposalFilterRequest request, int? clientId = null)
    {
        Expression<Func<Proposal, bool>> filter = p => true;
        if (clientId.HasValue)
        {
            filter = filter.And(p => p.ClientId == clientId);
        }
        if (!string.IsNullOrWhiteSpace(request.ClientDocumentNumber))
        {
            filter = filter.And(p => p.Client != null && p.Client.DocumentNumber.ToString() == request.ClientDocumentNumber);
        }
        if (!string.IsNullOrWhiteSpace(request.ClientEmail))
        {
            filter = filter.And(p => p.Client != null && p.Client.Email.ToString().Contains(request.ClientEmail));
        }
        if (request.Status.HasValue)
        {
            filter = filter.And(p => p.Status == request.Status.Value);
        }
        if (request.InsuranceType.HasValue)
        {
            filter = filter.And(p => p.InsuranceType == request.InsuranceType.Value);
        }
        if (request.MinAmount.HasValue)
        {
            filter = filter.And(p => p.Amount >= request.MinAmount.Value);
        }
        if (request.MaxAmount.HasValue)
        {
            filter = filter.And(p => p.Amount <= request.MaxAmount.Value);
        }
        if (request.CreatedAfter.HasValue)
        {
            filter = filter.And(p => p.CreatedAt >= request.CreatedAfter.Value);
        }
        if (request.CreatedBefore.HasValue)
        {
            filter = filter.And(p => p.CreatedAt <= request.CreatedBefore.Value);
        }
        return filter;
    }
}
