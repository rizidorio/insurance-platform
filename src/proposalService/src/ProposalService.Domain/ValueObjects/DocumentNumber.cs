using ProposalService.Domain.Exceptions;

namespace ProposalService.Domain.ValueObjects;

/// <summary>
/// Representa um número de documento brasileiro, como CPF (pessoa física) ou CNPJ (empresa).
/// </summary>
/// <remarks>CPF (Cadastro de Pessoas Físicas) e CNPJ (Cadastro Nacional da Pessoa Jurídica) são números
/// de documentos padronizados usados no Brasil para identificar pessoas físicas e jurídicas, respectivamente.
/// Instâncias desta classe garantem que o número do documento fornecido é válido de acordo com o formato de CPF ou CNPJ.</remarks>
public sealed class DocumentNumber
{
    /// <summary>
    /// Obtém o valor representado por esta instância.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="DocumentNumber"/> com o número de documento especificado.
    /// </summary>
    /// <remarks>CPF e CNPJ são números de documentos brasileiros usados para pessoas físicas e jurídicas, respectivamente.
    /// Este construtor valida o número de documento fornecido para garantir que ele esteja em conformidade com o formato esperado.</remarks>
    /// <param name="value">O número de documento a ser inicializado. Deve ser um CPF ou CNPJ válido, não pode ser nulo ou vazio, e não deve exceder 20
    /// caracteres.</param>
    /// <exception cref="DomainException">Lançada se <paramref name="value"/> for nulo, vazio, exceder 20 caracteres, ou não for um CPF ou CNPJ válido.</exception>
    public DocumentNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Número do documento não pode ser vazio", nameof(DocumentNumber));
        }
        if (value.Length > 20)
        {
            throw new DomainException("Número do documento não pode exceder 20 caracteres", nameof(DocumentNumber));
        }
        if (!IsValidCpf(value) && !IsValidCnpj(value))
        {
            throw new DomainException("Número do documento deve ser um CPF ou CNPJ válido", nameof(DocumentNumber));
        }
        Value = value;
    }

    /// <summary>
    /// Retorna uma representação em string do objeto atual.
    /// </summary>
    /// <returns>O valor da propriedade <see cref="Value"/>.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Determina se o objeto especificado é igual à instância atual.
    /// </summary>
    /// <param name="obj">O objeto a ser comparado com a instância atual. Deve ser do tipo <see cref="DocumentNumber"/>.</param>
    /// <returns><see langword="true"/> se o objeto especificado for um <see cref="DocumentNumber"/> e sua propriedade <c>Value</c>
    /// for igual à propriedade <c>Value</c> da instância atual; caso contrário, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is DocumentNumber other && Value == other.Value;

    /// <summary>
    /// Retorna o código hash para a instância atual.
    /// </summary>
    /// <remarks>O código hash é derivado da propriedade <see cref="Value"/>. Este método é adequado para
    /// uso em algoritmos de hash e estruturas de dados como tabelas hash.</remarks>
    /// <returns>Um inteiro representando o código hash da instância atual.</returns>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Valida se o número de CPF (Cadastro de Pessoas Físicas) especificado é válido de acordo com as regras
    /// brasileiras de CPF.
    /// </summary>
    /// <remarks>Um número de CPF válido deve ter 11 dígitos e não pode consistir em todos os dígitos idênticos.
    /// O método realiza a validação usando o algoritmo padrão para verificação de CPF, que inclui o cálculo e a verificação
    /// dos dois dígitos de verificação.</remarks>
    /// <param name="cpf">O número de CPF a ser validado. Deve ser uma string contendo exatamente 11 dígitos numéricos.
    /// Caracteres não numéricos são ignorados.</param>
    /// <returns><see langword="true"/> se o número de CPF for válido; caso contrário, <see langword="false"/>.</returns>
    private static bool IsValidCpf(string cpf)
    {
        cpf = new string([.. cpf.Where(char.IsDigit)]);
        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += (tempCpf[i] - '0') * mult1[i];

        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;
        tempCpf += digit1;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (tempCpf[i] - '0') * mult2[i];

        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;
        tempCpf += digit2;

        return cpf.EndsWith(tempCpf.Substring(9, 2));
    }

    /// <summary>
    /// Valida se o CNPJ (Cadastro Nacional da Pessoa Jurídica) especificado está em um formato válido.
    /// </summary>
    /// <remarks>Um CNPJ válido deve consistir em exatamente 14 dígitos numéricos e passar na validação de soma de verificação
    /// baseada no algoritmo oficial. CNPJs com todos os dígitos idênticos (por exemplo, "11111111111111") são considerados
    /// inválidos.</remarks>
    /// <param name="cnpj">A string CNPJ a ser validada. Deve conter apenas caracteres numéricos ou pode incluir caracteres de formatação
    /// (por exemplo, pontos, barras ou hífens), que serão ignorados durante a validação.</param>
    /// <returns><see langword="true"/> se o CNPJ for válido de acordo com as regras padrão de validação de CNPJ brasileiro;
    /// caso contrário, <see langword="false"/>.</returns>
    private static bool IsValidCnpj(string cnpj)
    {
        cnpj = new string([.. cnpj.Where(char.IsDigit)]);
        if (cnpj.Length != 14 || cnpj.All(c => c == cnpj[0]))
            return false;

        int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj[..12];
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += (tempCnpj[i] - '0') * mult1[i];

        int remainder = sum % 11;
        int digit = remainder < 2 ? 0 : 11 - remainder;
        tempCnpj += digit;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += (tempCnpj[i] - '0') * mult2[i];

        remainder = sum % 11;
        digit = remainder < 2 ? 0 : 11 - remainder;
        tempCnpj += digit;

        return cnpj.EndsWith(tempCnpj.Substring(12, 2));
    }
}
