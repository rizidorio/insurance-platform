using HiringService.Domain.Entities;

namespace HiringService.Domain.Interfaces.Services;

/// <summary>
/// Define um serviço de validação de contrato que verifica a correção de uma instância de contrato especificada.
/// </summary>
/// <remarks>
/// Implementações desta interface devem realizar a lógica de validação apropriada ao tipo de contrato.
/// A validação pode incluir verificação de campos obrigatórios, regras de negócio ou integridade dos dados.
/// Esta interface é normalmente utilizada para impor restrições de contrato antes de processar ou persistir os dados do contrato.
/// </remarks>
public interface IContractValidation
{
    /// <summary>
    /// Valida o contrato especificado para garantir que ele atende a todos os critérios exigidos.
    /// </summary>
    /// <param name="contract">O contrato a ser validado. Não pode ser nulo.</param>
    void Validate(Contract contract);
}
