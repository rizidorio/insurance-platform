using ProposalService.Domain.Entities;
using ProposalService.Domain.Exceptions;
using ProposalService.Domain.Services.Interface;

namespace ProposalService.Domain.Services;

/// <inheritdoc/>
public sealed class ClientValidation : IClientValidation
{
    /// <inheritdoc/>
    public void Validate(Client client)
    {
        if (client is null)
            throw new DomainException("Cliente não pode ser nulo.", nameof(client));

        if (client.BirthDate > DateTime.UtcNow)
            throw new DomainException("Data de nascimento não pode ser no futuro.", nameof(client.BirthDate));
    }
}
