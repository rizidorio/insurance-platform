namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa o conjunto de critérios de filtro usados para consultar registros de clientes, incluindo campos de pesquisa opcionais e parâmetros de paginação.
/// </summary>
/// <remarks>Todos os parâmetros de filtro são opcionais; apenas os critérios especificados serão aplicados. Os parâmetros de paginação têm como padrão a página 1 e 10 registros por página, caso não sejam informados.</remarks>
/// <param name="Name">O nome do cliente para filtrar. Especifique <see langword="null"/> para ignorar este critério.</param>
/// <param name="DocumentNumber">O número do documento (como RG ou CPF/CNPJ) para filtrar. Especifique <see langword="null"/> para ignorar este critério.</param>
/// <param name="Email">O endereço de e-mail do cliente para filtrar. Especifique <see langword="null"/> para ignorar este critério.</param>
/// <param name="BirthDateStart">A data de nascimento inicial para incluir nos resultados. Especifique <see langword="null"/> para incluir todas as datas de nascimento.</param>
/// <param name="BirthDateEnd">A data de nascimento final para incluir nos resultados. Especifique <see langword="null"/> para incluir todas as datas de nascimento.</param>
/// <param name="Page">O número da página dos resultados a serem recuperados. Deve ser maior que zero.</param>
/// <param name="PageSize">O número máximo de registros a retornar por página. Deve ser maior que zero.</param>
public sealed record GetClientsFilterRequest(
    string? Name,
    string? DocumentNumber,
    string? Email,
    DateTime? BirthDateStart,
    DateTime? BirthDateEnd,
    int? Page = 1,
    int? PageSize = 10);
