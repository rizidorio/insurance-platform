namespace Insurence.Platform.Common.Wrappers;

/// <summary>
/// Representa uma resposta paginada contendo uma coleção de itens e metadados de paginação.
/// </summary>
/// <remarks>Esta classe fornece informações sobre a página atual, total de itens, tamanho da página e total de páginas.
/// Também inclui propriedades para determinar se há páginas anteriores ou posteriores disponíveis.</remarks>
/// <typeparam name="TObject">O tipo de itens contidos na resposta.</typeparam>
/// <param name="items">Os itens da página atual.</param>
/// <param name="totalItems">O número total de itens.</param>
/// <param name="currentPage">O número da página atual.</param>
/// <param name="pageSize">O tamanho da página.</param>
public sealed class PaginatedResponse<TObject>(
    IReadOnlyCollection<TObject>? items,
    int totalItems,
    int currentPage,
    int pageSize)
{
    /// <summary>
    /// Lista com o tipo de retorno
    /// </summary>
    public IReadOnlyCollection<TObject>? Items { get; private set; } = items;

    /// <summary>
    /// Obtem o total da lista do tipo <typeparamref name="TObject"/>
    /// </summary>
    public int TotalItems { get; private set; } = totalItems;

    /// <summary>
    /// Página atual da consulta ou lista de retorno
    /// </summary>
    public int CurrentPage { get; private set; } = currentPage;

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; private set; } = pageSize;

    /// <summary>
    /// Quantidade de página
    /// </summary>
    public int TotalPages { get; private set; } = (int)Math.Ceiling(totalItems / (double)pageSize);

    /// <summary>
    /// Indica se há paginação anterior
    /// </summary>
    public bool HasPreviusPage => CurrentPage > 1;

    /// <summary>
    /// Indica se há paginação posterior
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;
}
