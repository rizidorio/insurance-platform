using HiringService.Domain.Entities;

namespace HiringService.Domain.Interfaces.Services;

/// <summary>
/// Define um serviço para recuperar o status de uma proposta pelo seu identificador único.
/// </summary>
public interface IProposalClientService
{
    /// <summary>
    /// Recupera assincronamente o status atual de uma proposta identificada pelo ID especificado.
    /// </summary>
    /// <param name="proposalId">O identificador único da proposta cujo status será recuperado.</param>
    /// <param name="cancellationToken">Um token que pode ser usado para cancelar a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém um objeto <see cref="Proposal"/> com o status atual da proposta especificada.</returns>
    Task<Proposal?> GetProposalStatusAsync(Guid proposalId, CancellationToken cancellationToken);
}
