using ProposalService.Domain.Enums;

namespace ProposalService.Domain.Entities;

/// <summary>
/// Representa uma proposta de produto de seguro, incluindo informações do cliente, valor solicitado, status e metadados relacionados.
/// </summary>
/// <remarks>
/// Uma proposta encapsula os detalhes e o ciclo de vida de uma solicitação de seguro, incluindo sua criação, aprovação, rejeição e status de análise.
/// A classe fornece métodos para atualizar o status da proposta e rastreia os timestamps de criação e atualização.
/// As instâncias normalmente são criadas usando o método <see cref="Create"/> para garantir que os campos obrigatórios sejam definidos.
/// </remarks>
public sealed class Proposal
{
    /// <summary>
    /// Obtém o identificador único da entidade.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Obtém o identificador externo único da entidade.
    /// </summary>
    public Guid ExternalId { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Obtém o identificador único do cliente associado a esta instância.
    /// </summary>
    public int ClientId { get; private set; }

    /// <summary>
    /// Obtém o cliente associado a esta instância.
    /// </summary>
    public Client? Client { get; init; } = default!;

    /// <summary>
    /// Obtém o valor monetário associado à transação.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Obtém o status atual da proposta.
    /// </summary>
    public ProposalStatus Status { get; private set; } = ProposalStatus.InAnalysis;

    /// <summary>
    /// Obtém o tipo de seguro associado a esta instância.
    /// </summary>
    public InsuranceType InsuranceType { get; private set; }

    /// <summary>
    /// Obtém a razão pela qual a proposta foi rejeitada, se aplicável.
    /// </summary>
    public string? RejectReason { get; private set; }

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
    private Proposal() { }

    /// <summary>
    /// Inicializa uma nova instância da classe Proposal com o cliente, tipo de seguro e valor especificados.
    /// </summary>
    /// <param name="clientId">O identificador único do cliente associado à proposta.</param>
    /// <param name="insuranceType">O tipo de seguro proposto para o cliente.</param>
    /// <param name="amount">O valor monetário da proposta de seguro. Deve ser um valor não negativo.</param>
    private Proposal(int clientId, InsuranceType insuranceType, decimal amount)
    {
        ClientId = clientId;
        InsuranceType = insuranceType;
        Amount = amount;
    }

    /// <summary>
    /// Cria uma nova instância de Proposal para o cliente, tipo de seguro e valor de cobertura especificados.
    /// </summary>
    /// <param name="clientId">O identificador único do cliente para quem a proposta está sendo criada.</param>
    /// <param name="insuranceType">O tipo de seguro a ser incluído na proposta.</param>
    /// <param name="amount">O valor de cobertura para a proposta de seguro. Deve ser um valor positivo.</param>
    /// <returns>Um objeto Proposal inicializado com o clientId, tipo de seguro e valor de cobertura especificados.</returns>
    public static Proposal Create(
        int clientId,
        InsuranceType insuranceType,
        decimal amount)
    {
        return new Proposal(clientId, insuranceType, amount);
    }

    /// <summary>
    /// Marca a proposta como aprovada e atualiza o timestamp de última modificação.
    /// </summary>
    /// <remarks>
    /// Este método define o status da proposta como <see cref="ProposalStatus.Approved"/> e atualiza <c>UpdatedAt</c> para o horário UTC atual.
    /// Chame este método para indicar que a proposta foi formalmente aprovada.
    /// </remarks>
    public void Approve()
    {
        Status = ProposalStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a proposta como rejeitada e registra o motivo especificado.
    /// </summary>
    /// <remarks>
    /// Ao chamar este método, o status da proposta é atualizado para rejeitado e o motivo da rejeição é definido.
    /// O timestamp de última atualização também é atualizado. Esta ação normalmente é irreversível; certifique-se de que a rejeição é
    /// realmente desejada antes de chamar este método.
    /// </remarks>
    /// <param name="reason">
    /// A explicação do motivo pelo qual a proposta está sendo rejeitada. Este valor é armazenado para referência e auditoria.
    /// </param>
    public void Reject(string reason)
    {
        Status = ProposalStatus.Rejected;
        RejectReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Define o status da proposta como EmAnálise e atualiza o timestamp de última modificação.
    /// </summary>
    /// <remarks>
    /// Chame este método para indicar que a proposta está atualmente em análise. Isso atualiza tanto o status quanto a propriedade UpdatedAt para refletir a alteração.
    /// </remarks>
    public void SetInAnalysis()
    {
        Status = ProposalStatus.InAnalysis;
        UpdatedAt = DateTime.UtcNow;
    }
}
