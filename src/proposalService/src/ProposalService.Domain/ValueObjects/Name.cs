using ProposalService.Domain.Exceptions;

namespace ProposalService.Domain.ValueObjects;

/// <summary>
/// Representa um nome com restrições de validação específicas.
/// </summary>
/// <remarks>Uma instância <see cref="Name"/> encapsula um valor string que não pode ser nulo, vazio e não
/// pode ter mais de 100 caracteres. Esta classe é imutável.</remarks>
public sealed class Name
{
    /// <summary>
    /// Obtém o valor representado por esta instância.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Name"/> com o valor especificado.
    /// </summary>
    /// <param name="value">O valor do nome a ser atribuído. Deve ser uma string não vazia e não pode exceder 100 caracteres.</param>
    /// <exception cref="DomainException">Lançada se <paramref name="value"/> for nulo, vazio, consistir apenas de espaços em branco ou exceder 100 caracteres.</exception>
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Nome não pode ser nulo ou vazio", nameof(Name));
        }
        if (value.Length > 100)
        {
            throw new DomainException("Nome não pode exceder 100 caracteres", nameof(Name));
        }
        Value = value;
    }

    /// <summary>
    /// Retorna uma representação string do objeto atual.
    /// </summary>
    /// <returns>O valor string da propriedade <see cref="Value"/>.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Determina se o objeto especificado é igual à instância atual.
    /// </summary>
    /// <param name="obj">O objeto a ser comparado com a instância atual. Deve ser do tipo <see cref="Name"/>.</param>
    /// <returns><see langword="true"/> se o objeto especificado for uma instância de <see cref="Name"/> e sua propriedade <c>Value</c>
    /// for igual à propriedade <c>Value</c> da instância atual; caso contrário, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is Name other && Value == other.Value;

    /// <summary>
    /// Retorna o código hash para a instância atual.
    /// </summary>
    /// <remarks>O código hash é derivado da propriedade <see cref="Value"/>. Este método é adequado para
    /// uso em algoritmos de hash e estruturas de dados como tabelas hash.</remarks>
    /// <returns>Um inteiro representando o código hash da instância atual.</returns>
    public override int GetHashCode() => Value.GetHashCode();
}
