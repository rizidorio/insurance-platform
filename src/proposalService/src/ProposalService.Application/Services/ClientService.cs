using Insurence.Platform.Common.Extensions;
using Insurence.Platform.Common.Helpers;
using Insurence.Platform.Common.Wrappers;
using Microsoft.Extensions.Logging;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.DataTransferObjects.Responses;
using ProposalService.Application.Services.Interfaces;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Interfaces;
using ProposalService.Domain.Services.Interface;
using System.Linq.Expressions;

namespace ProposalService.Application.Services;

/// <inheritdoc/>
public sealed class ClientService(
    IClientValidation clientValidation,
    IClientRepository clientRepository,
    ILogger<ClientService> logger) : IClientService
{
    /// <inheritdoc/>
    public async Task<ResponseDefault<ClientResponse>> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var client = Client.Create(
            request.Name,
            request.DocumentNumber,
            request.Email,
            request.BirthDate.GetValueOrDefault());

        clientValidation.Validate(client);

        await clientRepository.AddAsync(client, cancellationToken);
        logger.LogInformation("Cliente criado com sucesso, {ClientId}.", client.ExternalId);
        return ResponseDefault<ClientResponse>.CreateCreatedResponse(client, "Cliente criado com sucesso.");
    }

    /// <inheritdoc/>
    public async Task<ResponseDefault<PaginatedResponse<ClientResponse>>> GetByFilterAsync(GetClientsFilterRequest request, CancellationToken cancellationToken)
    {
        var filter = BuildFilter(request);

        var query = await clientRepository.GetAllAsync(filter, cancellationToken);

        var paginatedResult = await query.ToPaginatedListAsync(request.Page ?? 1, request.PageSize ?? 10, cancellationToken);

        var response  = new PaginatedResponse<ClientResponse>(
            items: paginatedResult.Items?.ToList().ConvertAll(client => (ClientResponse)client),
            totalItems: paginatedResult.TotalItems,
            currentPage: paginatedResult.CurrentPage,
            pageSize: paginatedResult.PageSize);

        logger.LogInformation("Consulta de clientes realizada com sucesso. Filtros: {Filters}.", request);
        return ResponseDefault<PaginatedResponse<ClientResponse>>.CreateSuccessResponse(response);
    }

    /// <summary>
    /// Constrói uma expressão de filtro para consultar clientes com base nos critérios de filtro especificados.
    /// </summary>
    /// <remarks>A expressão retornada pode ser utilizada em consultas LINQ para filtrar registros de clientes de acordo
    /// com os critérios fornecidos. Se nenhum critério for especificado, a expressão corresponde a todos os clientes.</remarks>
    /// <param name="request">Objeto contendo os critérios de filtro a serem aplicados, como nome, número do documento, e-mail e intervalo de data de nascimento.
    /// Não pode ser nulo.</param>
    /// <returns>Uma expressão que avalia como <see langword="true"/> para clientes que atendem a todos os critérios especificados; caso contrário,
    /// <see langword="false"/>.</returns>
    private static Expression<Func<Client, bool>> BuildFilter(GetClientsFilterRequest request)
    {
        Expression<Func<Client, bool>> filter = client => true;
        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(client => client.Name.ToString().Contains(request.Name));
        if (!string.IsNullOrWhiteSpace(request.DocumentNumber))
            filter = filter.And(client => client.DocumentNumber.ToString() == request.DocumentNumber);
        if (!string.IsNullOrWhiteSpace(request.Email))
            filter = filter.And(client => client.Email != null && client.Email.ToString() == request.Email);
        if (request.BirthDateStart.HasValue)
            filter = filter.And(client => client.BirthDate.HasValue && client.BirthDate.Value >= request.BirthDateStart.Value);
        if (request.BirthDateEnd.HasValue)
            filter = filter.And(client => client.BirthDate.HasValue && client.BirthDate.Value <= request.BirthDateEnd.Value);
        return filter;
    }
}
