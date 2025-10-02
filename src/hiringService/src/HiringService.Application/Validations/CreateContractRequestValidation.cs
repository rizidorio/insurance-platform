using FluentValidation;
using HiringService.Application.DataTransferObjects.Requests;
using Insurence.Platform.Common.Validation;

namespace HiringService.Application.Validations;

/// <summary>
/// Fornece regras de validação para o modelo CreateContractRequest, garantindo que campos obrigatórios estejam presentes e valores
/// atendam aos requisitos de negócio antes da criação do contrato.
/// </summary>
/// <remarks>Este validador garante que os identificadores da proposta e do cliente estejam especificados, que a data de início
/// do contrato não seja no passado e que a duração do contrato em meses seja maior que zero. Utilize esta classe para validar
/// solicitações de criação de contrato antes do processamento ou persistência.</remarks>
public sealed class CreateContractRequestValidation : BaseValidator<CreateContractRequest>
{
    public CreateContractRequestValidation()
    {
        RuleFor(x => x.ProposalId)
            .NotEmpty().WithMessage("O identificador da proposta é obrigatório.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("O identificador do cliente é obrigatório.");

        RuleFor(x => x.EffectiveDateStart)
            .NotEmpty().WithMessage("A data de início do contrato é obrigatória.")
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("A data de início do contrato não pode ser no passado.");

        RuleFor(x => x.EffectiveMonths)
            .GreaterThan(0).WithMessage("A duração do contrato em meses deve ser maior que zero.");
    }
}
