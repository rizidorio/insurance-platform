using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Exceptions;
using ProposalService.Domain.Interfaces.Services;

namespace ProposalService.Domain.Services;

/// <inheritdoc/>
public sealed class ProposalValidation : IProposalValidation
{
    /// <inheritdoc/>
    public void Validate(Proposal proposal)
    {
        if (proposal is null)
            throw new DomainException("Proposta não pode ser nula.", nameof(proposal));

        if (proposal.Amount <= 0)
            throw new DomainException("Valor da proposta deve ser maior que zero.", nameof(proposal.Amount));

        if (proposal.ClientId <= 0)
            throw new DomainException("Identificador do cliente inválido.", nameof(proposal.ClientId));

        if (!Enum.IsDefined(typeof(InsuranceType), proposal.InsuranceType))
            throw new DomainException("Tipo de seguro inválido.", nameof(proposal.InsuranceType));

        if (proposal.InsuranceType == InsuranceType.None)
            throw new DomainException("Tipo de seguro não pode ser 'Desconhecido'.", nameof(proposal.InsuranceType));

        if (proposal.InsuranceType == InsuranceType.Life && proposal.Amount > 100000)
            throw new DomainException("Valor máximo para seguro de vida é 100.000.", nameof(proposal.Amount));

        if (proposal.InsuranceType == InsuranceType.Vehicle && proposal.Amount > 50000)
            throw new DomainException("Valor máximo para seguro de veículo é 50.000.", nameof(proposal.Amount));

        if (proposal.InsuranceType == InsuranceType.Property && proposal.Amount > 200000)
            throw new DomainException("Valor máximo para seguro de propriedade é 200.000.", nameof(proposal.Amount));

        if (proposal.InsuranceType == InsuranceType.Health && proposal.Amount > 150000)
            throw new DomainException("Valor máximo para seguro de saúde é 150.000.", nameof(proposal.Amount));
    }
}
