namespace Insurence.Platform.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converte a string especificada para o formato snake_case.
    /// </summary>
    /// <remarks>Este método processa a string de entrada convertendo todas as letras maiúsculas para minúsculas e
    /// inserindo underscores antes das letras maiúsculas que não estão no início da string. Caracteres de espaço em branco
    /// são preservados na saída.</remarks>
    /// <param name="str">A string de entrada para converter. Não deve ser nula, vazia ou consistir apenas de espaços em branco.</param>
    /// <returns>Uma nova string no formato snake_case, onde letras maiúsculas são substituídas por letras minúsculas e underscores
    /// são inseridos antes das letras maiúsculas (exceto no início da string).</returns>
    /// <exception cref="ArgumentException">Lançada se <paramref name="str"/> for nula, vazia ou consistir apenas de espaços em branco.</exception>
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            throw new ArgumentException("String cannot be null or empty.", nameof(str));
        var result = new System.Text.StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (char.IsUpper(c) && i > 0 && !char.IsWhiteSpace(str[i - 1]))
            {
                result.Append('_');
            }
            result.Append(char.ToLowerInvariant(c));
        }
        return result.ToString();
    }
}
