using HiringService.Application.DataTransferObjects.Requests;
using HiringService.Application.DataTransferObjects.Responses;
using HiringService.Application.Services.Interfaces;
using HiringService.Domain.Entities;
using HiringService.Domain.Enums;
using HiringService.Domain.Interfaces.Repositories;
using HiringService.Domain.Interfaces.Services;
using Insurence.Platform.Common.Extensions;
using Insurence.Platform.Common.Helpers;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces.Notification;
using Insurence.Platform.Common.Messaging.RabbitMq.Messages;
using Insurence.Platform.Common.Notification.Enums;
using Insurence.Platform.Common.Wrappers;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace HiringService.Application.Services;

/// <inheritdoc/>
public sealed class ContractService(
    IContractValidation contractValidation,
    ICalculateEffectiveDateEnd calculateEffectiveDateEnd,
    IContractRepository contractRepository,
    IProposalClientService proposalClientService,
    INotificationMessagePublish notificationMessagePublish,
    ILogger<ContractService> logger) : IContractService
{
    /// <inheritdoc/>
    public async Task<ResponseDefault<ContractResponse>> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken)
    {
        var proposal = await proposalClientService.GetProposalStatusAsync(request.ProposalId, cancellationToken);

        if (proposal is null)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não encontrada ao tentar criar contrato.", request.ProposalId);
            return ResponseDefault<ContractResponse>.CreateNotFoundResponse("Proposta não encontrada.");
        }

        if (proposal.Status != ProposalStatus.Approved)
        {
            logger.LogWarning("Proposta com ID {ProposalId} não está aprovada. Status atual: {Status}.", request.ProposalId, proposal.Status);
            var errors = new List<Error>
            {
                new Error("ProposalNotApproved", "A proposta deve estar aprovada para criar um contrato.")
            };
            return ResponseDefault<ContractResponse>.CreateBadRequestResponse(errors);
        }

        var effectiveDateEnd = calculateEffectiveDateEnd.Calculate(request.EffectiveDateStart, request.EffectiveMonths);

        var contract = Contract.Create(
            request.ProposalId,
            request.ClientId,
            request.EffectiveDateStart,
            effectiveDateEnd);

        contractValidation.Validate(contract);

        await contractRepository.CreateAsync(contract, cancellationToken);
        logger.LogInformation("Contrato com ID {ContractId} criado com sucesso para a proposta {ProposalId}.", contract.Id, request.ProposalId);

        var notificationMessage = new NotificationMessage(
            CorrelationId: Guid.NewGuid(),
            NotificationType: NotificationType.CreateContract,
            NotificationChannel: NotificationChannel.Email,
            Subject: "Contrato Criado com Sucesso",
            Body: $"Seu contrato com ID {contract.Id} foi criado com sucesso e é válido de {contract.EffectiveDateStart:dd/MM/yyyy} até {contract.EffectiveDateEnd:dd/MM/yyyy}.",
            Email: proposal.ClientEmail);

        await notificationMessagePublish.PublishAsync(notificationMessage, cancellationToken);

        var response = (ContractResponse)contract;
        return ResponseDefault<ContractResponse>.CreateCreatedResponse(response, "Contrato criado com sucesso.");
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<PaginatedResponse<ContractResponse>>> GetByFilterAsync(GetContractsByFilterRequest request, CancellationToken cancellationToken)
    {
        var filter = BuildFilterExpression(request);
        var query = await contractRepository.GetAllAsync(filter, cancellationToken);

        var paginatedResult = await query.ToPaginatedListAsync(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);

        var response = new PaginatedResponse<ContractResponse>(
            items: paginatedResult.Items?.ToList().ConvertAll(c => (ContractResponse)c),
            totalItems: paginatedResult.TotalItems,
            currentPage: paginatedResult.CurrentPage,
            pageSize: paginatedResult.PageSize);

        logger.LogInformation("Recuperados {TotalItems} contratos com os filtros aplicados.", response.TotalItems);
        return ResponseDefault<PaginatedResponse<ContractResponse>>.CreateSuccessResponse(response, "Contratos recuperados com sucesso.");
    }

    /// <summary>
    /// Constrói uma expressão LINQ que filtra contratos com base nos critérios especificados na requisição.
    /// </summary>
    /// <remarks>
    /// A expressão retornada pode ser utilizada em consultas LINQ para recuperar contratos que correspondam
    /// às opções de filtro fornecidas. Apenas critérios com valores definidos na requisição são incluídos no filtro.
    /// </remarks>
    /// <param name="request">
    /// Objeto contendo critérios de filtro para contratos, como ID do cliente, ID da proposta, status de ativo e intervalo de datas de criação.
    /// Qualquer critério não definido será ignorado.
    /// </param>
    /// <returns>
    /// Uma expressão representando o filtro a ser aplicado aos contratos. A expressão retorna <see langword="true"/> para
    /// contratos que correspondem a todos os critérios especificados.
    /// </returns>
    private static Expression<Func<Contract, bool>> BuildFilterExpression(GetContractsByFilterRequest request)
    {
        Expression<Func<Contract, bool>> filter = c => true;
        if (request.ClientId.HasValue)
        {
            filter = filter.And(c => c.ClientId == request.ClientId.Value);
        }
        if (request.ProposalId.HasValue)
        {
            filter = filter.And(c => c.ProposalId == request.ProposalId.Value);
        }
        if (request.IsActive.HasValue)
        {
            filter = filter.And(c => c.IsActive == request.IsActive.Value);
        }
        if (request.CreatedAtStart.HasValue)
        {
            filter = filter.And(c => c.CreatedAt >= request.CreatedAtStart.Value);
        }
        if (request.CreatedAtEnd.HasValue)
        {
            filter = filter.And(c => c.CreatedAt <= request.CreatedAtEnd.Value);
        }
        return filter;
    }
}
