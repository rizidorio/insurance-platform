    using FluentValidation;
    using FluentValidation.Results;

    namespace ProposalService.Application.Validations.Base;

    /// <summary>
    /// Fornece uma classe base para validadores de objetos que garantem que o payload está presente antes de realizar a validação.
    /// Herda de <see cref="AbstractValidator{TObject}"/> para suportar lógica de validação personalizada para objetos do tipo
    /// <typeparamref name="TObject"/>.
    /// </summary>
    /// <remarks>Esta classe adiciona uma verificação prévia para garantir que o objeto a ser validado não seja nulo. Se o
    /// payload estiver ausente, a validação falha com uma mensagem de erro específica. Validadores derivados devem herdar desta
    /// classe para aplicar automaticamente esta pré-condição antes de regras de validação adicionais.</remarks>
    /// <typeparam name="TObject">O tipo do objeto a ser validado.</typeparam>
    public abstract class BaseValidator<TObject> : AbstractValidator<TObject>
    {
        /// <summary>
        /// Realiza uma validação preliminar da instância do objeto dentro do contexto de validação especificado antes de
        /// executar regras de validação adicionais.
        /// </summary>
        /// <remarks>Se a instância do objeto a ser validada for nula, uma falha de validação é adicionada ao resultado e
        /// a pré-validação falha. Este método é normalmente chamado antes de aplicar regras detalhadas de validação para garantir que
        /// o payload está presente.</remarks>
        /// <param name="context">O contexto de validação contendo a instância do objeto a ser validada. A instância não pode ser nula.</param>
        /// <param name="result">O objeto de resultado de validação usado para registrar quaisquer falhas encontradas durante a pré-validação.</param>
        /// <returns>true se a instância do objeto está presente e a pré-validação foi aprovada; caso contrário, false.</returns>
        protected override bool PreValidate(ValidationContext<TObject> context, ValidationResult result)
        {
            if (context.InstanceToValidate is null)
            {
                result.Errors.Add(new ValidationFailure("10000", "Certifique-se que o payload foi preenchido."));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valida o contexto especificado e retorna o resultado da operação de validação.
        /// </summary>
        /// <param name="context">O contexto de validação contendo o objeto a ser validado. Não pode ser nulo.</param>
        /// <returns>Um ValidationResult que indica se o objeto no contexto é válido. Se o contexto for nulo, o resultado
        /// contém uma falha indicando que o payload deve ser fornecido.</returns>
        public override ValidationResult Validate(ValidationContext<TObject> context)
        {
            if (context is null)
                return new ValidationResult([new ValidationFailure("10000", "Certifique-se que o payload foi preenchido.")]);

            return base.Validate(context);
        }
    }