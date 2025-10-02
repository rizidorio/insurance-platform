namespace Insurence.Platform.Common.Wrappers;

/// <summary>
/// Representa um erro com detalhes opcionais como um código, mensagem e contexto.
/// </summary>
/// <remarks>Esta classe é usada para encapsular informações de erro em um formato estruturado. Ela fornece propriedades
/// opcionais para especificar um código de erro, uma mensagem descritiva e contexto adicional sobre o erro.</remarks>
/// <param name="code">O código do erro</param>
/// <param name="message">A mensagem do erro</param>
/// <param name="context">O contexto do erro</param>
public sealed class Error(
    string? code = null,
    string? message = null,
    string? context = null)
{
    /// <summary>
    /// Obtém o código associado à instância atual.
    /// </summary>
    public string? Code { get; } = code;

    /// <summary>
    /// Obtém a mensagem associada à instância atual.
    /// </summary>
    public string? Message { get; } = message;

    /// <summary>
    /// Obtém o contexto atual associado à operação.
    /// </summary>
    public string? Context { get; } = context;
}