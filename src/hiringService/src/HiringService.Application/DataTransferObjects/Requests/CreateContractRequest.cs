namespace HiringService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma requisição para criar um novo contrato com base em uma proposta, especificando o cliente, data de início do contrato e
/// duração do contrato.
/// </summary>
/// <param name="ProposalId">O identificador único da proposta a partir da qual o contrato será criado.</param>
/// <param name="ClientId">O identificador único do cliente para quem o contrato está sendo criado.</param>
/// <param name="EffectiveDateStart">A data em que o contrato entra em vigor.</param>
/// <param name="EffectiveMonths">O número de meses em que o contrato permanecerá vigente. Padrão de 12 se não especificado.</param>
public sealed record CreateContractRequest(
    Guid ProposalId,
    Guid ClientId,
    DateTime EffectiveDateStart,
    int EffectiveMonths = 12);
