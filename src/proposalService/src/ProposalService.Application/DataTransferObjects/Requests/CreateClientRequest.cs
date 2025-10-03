namespace ProposalService.Application.DataTransferObjects.Requests;

/// <summary>
/// Representa uma solicitação para criar um novo cliente com informações pessoais e de contato especificadas.
/// </summary>
/// <param name="Name">O nome completo do cliente. Não pode ser nulo ou vazio.</param>
/// <param name="DocumentNumber">O número de identificação ou documento associado ao cliente. Não pode ser nulo ou vazio.</param>
/// <param name="Email">O endereço de e-mail do cliente. Pode ser nulo se nenhum e-mail for fornecido.</param>
/// <param name="BirthDate">A data de nascimento do cliente. Pode ser nula se não for especificada.</param>
public sealed record CreateClientRequest(
    string Name,
    string DocumentNumber,
    string Email,
    DateTime? BirthDate);
