using ProposalService.Domain.Enums;

namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa um conjunto de critérios de filtro para recuperar propostas, incluindo informações do cliente, intervalo de datas de criação,
/// intervalo de valores, tipo de seguro, status e opções de paginação.
/// </summary>
/// <remarks>Todos os parâmetros de filtro são opcionais; apenas propostas que correspondem aos critérios especificados são retornadas.
/// Os parâmetros de paginação controlam o subconjunto de resultados retornados para grandes conjuntos de dados.</remarks>
/// <param name="ClientId">O identificador único do cliente para filtrar propostas. Se nulo, propostas de todos os clientes são incluídas.</param>
/// <param name="ClientDocumentNumber">O número do documento do cliente para filtrar propostas. Se nulo, propostas de todos os números de documento são incluídas.</param>
/// <param name="ClientEmail">O endereço de e-mail do cliente para filtrar propostas. Se nulo, propostas de todos os endereços de e-mail são incluídas.</param>
/// <param name="CreatedAfter">A data mínima de criação das propostas a serem incluídas. Apenas propostas criadas após essa data são retornadas. Se nulo, nenhum
/// limite inferior de data é aplicado.</param>
/// <param name="CreatedBefore">A data máxima de criação das propostas a serem incluídas. Apenas propostas criadas antes dessa data são retornadas. Se nulo, nenhum
/// limite superior de data é aplicado.</param>
/// <param name="MinAmount">O valor mínimo da proposta a ser incluída. Apenas propostas com valor maior ou igual a esse valor são
/// retornadas. Se nulo, nenhum mínimo é aplicado.</param>
/// <param name="MaxAmount">O valor máximo da proposta a ser incluída. Apenas propostas com valor menor ou igual a esse valor são retornadas.
/// Se nulo, nenhum máximo é aplicado.</param>
/// <param name="InsuranceType">O tipo de seguro para filtrar propostas. Se nulo, propostas de todos os tipos de seguro são incluídas.</param>
/// <param name="Status">O status para filtrar propostas. Se nulo, propostas de todos os status são incluídas.</param>
/// <param name="Page">O número da página de resultados a ser recuperada. Deve ser maior que zero. O padrão é 1 se não especificado.</param>
/// <param name="PageSize">A quantidade de propostas por página. Deve ser maior que zero. O padrão é 10 se não especificado.</param>
public sealed record GetProposalFilterRequest(
    Guid? ClientId,
    string? ClientDocumentNumber,
    string? ClientEmail,
    DateTime? CreatedAfter,
    DateTime? CreatedBefore,
    decimal? MinAmount,
    decimal? MaxAmount,
    InsuranceType? InsuranceType,
    ProposalStatus? Status,
    int? Page = 1,
    int? PageSize = 10);
