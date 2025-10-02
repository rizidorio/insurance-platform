namespace HiringService.Domain.Entities;

/// <summary>
/// Representa um contrato associado a uma proposta, incluindo suas datas de vigência, informações de apólice e status de ativação.
/// </summary>
/// <remarks>
/// Um contrato é identificado de forma única por seus identificadores internos e externos, e está vinculado a uma proposta.
/// O contrato mantém seu estado de ativação e rastreia os timestamps de criação e atualização.
/// Utilize o método estático Create para instanciar um novo contrato com os detalhes obrigatórios da proposta e da apólice.
/// A ativação e desativação do contrato são gerenciadas pelos métodos Activate e Deactivate, que atualizam o status e o timestamp de modificação.
/// </remarks>
public sealed class Contract
{
    /// <summary>
    /// Obtém o identificador único da entidade.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Obtém o identificador externo único associado a esta instância.
    /// </summary>
    public Guid ExternalId { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Obtém o identificador único da proposta.
    /// </summary>
    public Guid ProposalId { get; private set; }

    /// <summary>
    /// Obtém o identificador único do cliente associado a esta instância.
    /// </summary>
    public Guid ClientId { get; private set; }

    /// <summary>
    /// Obtém a data e hora em que o funcionário foi contratado.
    /// </summary>
    public DateTime HiringDate { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtém a data de início da vigência do contrato.
    /// </summary>
    public DateTime EffectiveDateStart { get; private set; }

    /// <summary>
    /// Obtém a data de término da vigência do contrato.
    /// </summary>
    public DateTime EffectiveDateEnd { get; private set; }

    /// <summary>
    /// Obtém o número único da apólice de seguro atribuída.
    /// </summary>
    public string PolicyNumber { get; private set; } = default!;

    /// <summary>
    /// Indica se a instância atual está ativa.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Obtém o status atual como uma string indicando se a entidade está ativa ou inativa.
    /// </summary>
    /// <remarks>
    /// Retorna "Ativo" se a entidade estiver ativa; caso contrário, retorna "Inativo".
    /// O status é determinado pelo valor da propriedade IsActive.
    /// </remarks>
    public string Status => IsActive ? "Ativo" : "Inativo";

    /// <summary>
    /// Obtém a data e hora UTC em que o objeto foi criado.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtém a data e hora em que a entidade foi atualizada pela última vez.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Construtor privado para uso interno e por frameworks de ORM.
    /// </summary>
    private Contract() { }

    /// <summary>
    /// Inicializa uma nova instância da classe Contract com o ID da proposta, intervalo de datas de vigência e número da apólice especificados.
    /// </summary>
    /// <param name="proposalId">O identificador único da proposta associada a este contrato.</param>
    /// <param name="effectiveDateStart">A data e hora de início da vigência do contrato.</param>
    /// <param name="effectiveDateEnd">A data e hora de término da vigência do contrato.</param>
    /// <param name="policyNumber">O número da apólice atribuído a este contrato. Não pode ser nulo.</param>
    private Contract(
        Guid proposalId,
        Guid clientId,
        DateTime effectiveDateStart,
        DateTime effectiveDateEnd)
    {
        ProposalId = proposalId;
        ClientId = clientId;
        EffectiveDateStart = effectiveDateStart;
        EffectiveDateEnd = effectiveDateEnd;
        PolicyNumber = GeneratePolicyNumber();
    }

    /// <summary>
    /// Cria uma nova instância de Contract usando o ID da proposta, intervalo de datas de vigência e número da apólice especificados.
    /// </summary>
    /// <param name="proposalId">O identificador único da proposta associada ao contrato.</param>
    /// <param name="effectiveDateStart">A data e hora de início da vigência do contrato.</param>
    /// <param name="effectiveDateEnd">A data e hora de término da vigência do contrato.</param>
    /// <param name="policyNumber">O número da apólice a ser atribuído ao contrato. Não pode ser nulo.</param>
    /// <returns>Um objeto Contract inicializado com o ID da proposta, datas de vigência e número da apólice fornecidos.</returns>
    public static Contract Create(
        Guid proposalId,
        Guid clientId,
        DateTime effectiveDateStart,
        DateTime effectiveDateEnd)
    => new(
        clientId,
        proposalId,
        effectiveDateStart,
        effectiveDateEnd);

    /// <summary>
    /// Desativa a instância atual, marcando-a como inativa e atualizando o timestamp de última modificação.
    /// </summary>
    /// <remarks>
    /// Se a instância já estiver inativa, este método não tem efeito.
    /// Após chamar este método, a propriedade IsActive será definida como false e UpdatedAt refletirá o momento da desativação.
    /// </remarks>
    public void Deactivate()
    {
        if (!IsActive) 
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Ativa a instância atual se ela não estiver ativa.
    /// </summary>
    /// <remarks>
    /// Se a instância já estiver ativa, este método não tem efeito.
    /// Ao ativar, o estado da instância é atualizado e o timestamp de última atualização é definido para o horário UTC atual.
    /// </remarks>
    public void Activate()
    {
        if (IsActive) 
            return;
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gera um número de apólice único, em maiúsculas, composto por 10 caracteres alfanuméricos.
    /// </summary>
    /// <remarks>
    /// O número de apólice gerado é adequado para uso como identificador em seguros ou domínios similares.
    /// Cada chamada produz um valor diferente, garantindo unicidade entre as invocações.
    /// </remarks>
    /// <returns>
    /// Uma string contendo o número de apólice gerado. O valor sempre possui 10 caracteres e contém apenas letras maiúsculas e dígitos.
    /// </returns>
    private static string GeneratePolicyNumber() 
        => Guid.NewGuid().ToString().Replace("-", "")[..10].ToUpper();
}
