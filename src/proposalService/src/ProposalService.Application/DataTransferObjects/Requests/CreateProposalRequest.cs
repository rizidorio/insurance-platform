using ProposalService.Domain.Enums;

namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma requisição para criar uma nova proposta de seguro, incluindo informações do cliente, valor solicitado e tipo de seguro.
/// </summary>
/// <param name="ClientId">O identificador único do cliente. Especifique <see langword="null"/> se o cliente ainda não estiver cadastrado.</param>
/// <param name="ClientName">O nome completo do cliente para quem a proposta está sendo criada. Pode ser <see langword="null"/> se não informado.</param>
/// <param name="ClientDocumentNumber">O número do documento oficial (como RG, CPF ou passaporte) que identifica o cliente. Pode ser <see langword="null"/> se não disponível.</param>
/// <param name="ClientEmail">O endereço de e-mail do cliente. Pode ser <see langword="null"/> se não informado.</param>
/// <param name="ClientBirthDate">A data de nascimento do cliente. Especifique <see langword="null"/> se a data de nascimento for desconhecida.</param>
/// <param name="Amount">O valor monetário solicitado para a proposta de seguro. Deve ser um valor não negativo.</param>
/// <param name="InsuranceType">O tipo de seguro solicitado para a proposta.</param>
public sealed record CreateProposalRequest(
    Guid? ClientId,
    string? ClientName,
    string? ClientDocumentNumber,
    string? ClientEmail,
    DateTime? ClientBirthDate,
    decimal Amount,
    InsuranceType InsuranceType);
