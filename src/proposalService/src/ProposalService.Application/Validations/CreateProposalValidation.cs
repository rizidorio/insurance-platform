using FluentValidation;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Application.Validations.Base;

namespace ProposalService.Application.Validations;

/// <summary>
/// Fornece regras de validação para o modelo CreateProposalRequest, garantindo que campos obrigatórios estejam presentes e valores
/// atendam aos limites definidos antes da criação de uma proposta.
/// </summary>
/// <remarks>Este validador aplica requisitos condicionais com base na presença ou ausência dos campos de identificação do cliente,
/// além de restrições de formato e faixas de valores. Utilize esta classe para validar instâncias de CreateProposalRequest antes de processar ou persistir propostas.</remarks>
public sealed class CreateProposalValidation : BaseValidator<CreateProposalRequest>
{
    public CreateProposalValidation()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.ClientDocumentNumber))
            .WithMessage("Id do cliente é obrigatório quando o CPF não é fornecido.");
        When(x => x.ClientId is null, () =>
        {
            RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Nome do cliente é obrigatório quando o Id do cliente não é informado.")
            .MaximumLength(100).WithMessage("Nome do cliente deve ter no máximo 100 caracteres.");
            RuleFor(x => x.ClientDocumentNumber)
                .NotEmpty().WithMessage("Número do documento é obrigatório quando o Id do cliente não é informado.")
                .MaximumLength(20).WithMessage("Número do documento deve ter no máximo 20 caracteres.");
            RuleFor(x => x.ClientEmail)
                .EmailAddress().WithMessage("E-mail inválido.")
                .When(x => !string.IsNullOrEmpty(x.ClientEmail))
                .MaximumLength(100).WithMessage("E-mail deve ter no máximo 100 caracteres.");
            RuleFor(x => x.ClientBirthDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de nascimento não pode ser no futuro.")
                .When(x => x.ClientBirthDate.HasValue);
        });
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Valor precisa ser maior que zero.");
        RuleFor(x => x.InsuranceType)
            .IsInEnum().WithMessage("Tipo do seguro precisa ser válido.");
    }
}
