using ProposalService.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace ProposalService.Domain.ValueObjects;

/// <summary>
/// Representa um endereço de e-mail em um formato validado.
/// </summary>
/// <remarks>A classe <see cref="Email"/> garante que o endereço de e-mail fornecido seja válido de acordo com as regras
/// comuns de formatação de e-mail. Instâncias desta classe são imutáveis após serem criadas.</remarks>
public sealed class Email
{
    /// <summary>
    /// Obtém o valor representado por esta instância.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Email"/> com o endereço de e-mail especificado.
    /// </summary>
    /// <remarks>O endereço de e-mail deve estar em conformidade com as regras padrão de formatação de e-mail, incluindo ter um símbolo "@"
    /// e um domínio.</remarks>
    /// <param name="value">O endereço de e-mail para inicializar a instância. Deve ter um formato de e-mail válido e não pode exceder 254
    /// caracteres.</param>
    /// <exception cref="DomainException">Lançada se <paramref name="value"/> for nulo, vazio, consistir apenas de espaços em branco, exceder 254 caracteres ou não
    /// estiver em um formato de e-mail válido.</exception>
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("E-mail não pode ser nulo ou vazio", nameof(Email));
        }
        if (value.Length > 254)
        {
            throw new DomainException("E-mail não pode exceder 254 caracteres", nameof(Email));
        }
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new DomainException("Formato em e-mail inválido", nameof(Email));
        }
        Value = value;
    }

    /// <summary>
    /// Retorna uma representação de string do objeto atual.
    /// </summary>
    /// <returns>O valor da string da propriedade <see cref="Value"/>.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Determina se o objeto especificado é igual à instância atual.
    /// </summary>
    /// <param name="obj">O objeto a ser comparado com a instância atual. Deve ser um objeto <see cref="Email"/>.</param>
    /// <returns><see langword="true"/> se o objeto especificado for um <see cref="Email"/> e sua propriedade <c>Value</c> for igual
    /// à propriedade <c>Value</c> da instância atual; caso contrário, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;

    /// <summary>
    /// Retorna o código hash para a instância atual.
    /// </summary>
    /// <remarks>O código hash é derivado da propriedade <see cref="Value"/>. Este método é adequado para
    /// uso em algoritmos de hash e estruturas de dados como tabelas hash.</remarks>
    /// <returns>Um inteiro representando o código hash da instância atual.</returns>
    public override int GetHashCode() => Value.GetHashCode();
}
