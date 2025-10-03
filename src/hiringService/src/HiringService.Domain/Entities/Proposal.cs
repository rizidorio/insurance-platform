using HiringService.Domain.Enums;

namespace HiringService.Domain.Entities;

/// <summary>
/// Representa uma proposta associada a um cliente, incluindo seu identificador externo e status atual.
/// </summary>
/// <remarks>Este tipo é destinado ao uso interno e não pode ser instanciado diretamente fora do seu assembly de origem. Todas as propriedades são imutáveis e definidas durante a inicialização do objeto.</remarks>
public sealed class Proposal
{
    /// <summary>
    /// Obtém o identificador externo único associado a esta entidade.
    /// </summary>
    public Guid ExternalId { get; init; }

    /// <summary>
    /// Obtém o identificador único do cliente associado a esta instância.
    /// </summary>
    public Guid ClientId { get; init; }

    /// <summary>
    /// Obtém o e-mail do cliente associado a esta instância, se disponível.
    /// </summary>
    public string? ClientEmail { get; init; }

    /// <summary>
    /// Obtém o status atual da proposta.
    /// </summary>
    public ProposalStatus Status { get; init; }

    /// <summary>
    /// O construtor é privado para evitar a instanciação direta.
    /// </summary>
    public Proposal() { }
}
