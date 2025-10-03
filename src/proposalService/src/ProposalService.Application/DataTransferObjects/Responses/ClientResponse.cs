using ProposalService.Domain.Entities;

namespace ProposalService.Application.DataTransferObjects.Responses;

/// <summary>
/// Representa um objeto de transferência de dados contendo informações do cliente para respostas de API.
/// </summary>
/// <param name="Id">O identificador único associado ao cliente.</param>
/// <param name="Name">O nome completo do cliente.</param>
/// <param name="DocumentNumber">O número do documento que identifica exclusivamente o cliente, como CPF ou passaporte.</param>
/// <param name="Email">O endereço de e-mail do cliente, ou <see langword="null"/> se não informado.</param>
/// <param name="BirthDate">A data de nascimento do cliente, ou <see langword="null"/> se não especificada.</param>
public sealed record ClientResponse(
    Guid Id,
    string Name,
    string DocumentNumber,
    string? Email,
    DateTime? BirthDate)
{
    /// <summary>
    /// Converte uma instância de <see cref="Client"/> em um <see cref="ClientResponse"/> para uso em respostas de API.
    /// </summary>
    /// <remarks>Este operador permite conversão direta entre entidades de cliente do domínio e modelos de resposta da API. Todas as propriedades relevantes são mapeadas diretamente; certifique-se de que a entidade de origem contenha dados válidos para serialização.</remarks>
    /// <param name="client">A entidade de cliente a ser convertida. Não pode ser nula.</param>
    public static implicit operator ClientResponse(Client? client)
    {
        return new ClientResponse(
            client?.ExternalId ?? Guid.Empty,
            client?.Name.ToString() ?? string.Empty,
            client?.DocumentNumber.ToString() ?? string.Empty,
            client?.Email?.ToString(),
            client?.BirthDate);
    }
    }
