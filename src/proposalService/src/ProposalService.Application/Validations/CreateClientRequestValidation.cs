using FluentValidation;
using Insurence.Platform.Common.Validation;
using ProposalService.Application.DataTransferObjects.Requests;

namespace ProposalService.Application.Validations;

/// <summary>
/// Fornece regras de validação para o modelo <see cref="CreateClientRequest"/> ao criar um novo cliente.
/// </summary>
/// <remarks>Este validador aplica restrições ao nome do cliente, número do documento, endereço de e-mail e data de nascimento
/// para garantir que os dados enviados atendam aos padrões exigidos. Use esta classe para validar solicitações de criação de cliente
/// antes de processá-las ou persistí-las. Todas as regras de validação são aplicadas automaticamente quando o validador é invocado.</remarks>
public sealed class CreateClientRequestValidation : BaseValidator<CreateClientRequest>
{
    public CreateClientRequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do cliente é obrigatório.")
            .MaximumLength(100).WithMessage("Nome do cliente deve ter no máximo 100 caracteres.");
        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Número do documento é obrigatório.")
            .MaximumLength(20).WithMessage("Número do documento deve ter no máximo 20 caracteres.");
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("E-mail inválido.")
            .When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(100).WithMessage("E-mail deve ter no máximo 100 caracteres.");
        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de nascimento não pode ser no futuro.")
            .When(x => x.BirthDate.HasValue);
    }
}
