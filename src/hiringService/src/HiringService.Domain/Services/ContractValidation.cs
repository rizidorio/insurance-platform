using HiringService.Domain.Entities;
using HiringService.Domain.Exceptions;
using HiringService.Domain.Interfaces.Services;

namespace HiringService.Domain.Services;

public sealed class ContractValidation : IContractValidation
{
    public void Validate(Contract contract)
    {
        if (contract is null)
            throw new DomainException("Contrato não pode ser nulo.", nameof(contract));

        if (contract.EffectiveDateStart == default)
            throw new DomainException("Data de início da vigência é obrigatória.", nameof(contract.EffectiveDateStart));

        if (contract.EffectiveDateEnd == default)
            throw new DomainException("Data de término da vigência é obrigatória.", nameof(contract.EffectiveDateEnd));

        if (contract.EffectiveDateEnd <= contract.EffectiveDateStart)
            throw new DomainException("Data de término da vigência deve ser posterior à data de início.", nameof(contract.EffectiveDateEnd));

        if (string.IsNullOrWhiteSpace(contract.PolicyNumber))
            throw new DomainException("Número da apólice é obrigatório.", nameof(contract.PolicyNumber));

        if (contract.PolicyNumber.Length > 50)
            throw new DomainException("Número da apólice não pode exceder 50 caracteres.", nameof(contract.PolicyNumber));

        if (contract.ProposalId == default)
            throw new DomainException("Identificador da proposta é obrigatório.", nameof(contract.ProposalId));
    }
}
