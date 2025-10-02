using ProposalService.Domain.ValueObjects;

namespace ProposalService.Domain.Entities;

/// <summary>
/// Representa uma entidade de cliente com informações de identificação, detalhes de contato e metadados de auditoria.
/// </summary>
/// <remarks>
/// A classe Client encapsula os dados principais de um cliente, incluindo nome, número de documento e endereço de e-mail.
/// Ela fornece métodos para criar e atualizar informações do cliente, além de registrar os timestamps de criação e modificação.
/// As instâncias são imutáveis, exceto pelas atualizações realizadas via método Update. Esta classe é normalmente utilizada
/// para modelar clientes em aplicações de negócio ou orientadas ao domínio.
/// </remarks>
public sealed class Client
{
    /// <summary>
    /// Obtém o identificador único da entidade.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Obtém o identificador externo único associado a esta instância.
    /// </summary>
    public Guid ExternalId { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Obtém o nome associado a esta instância.
    /// </summary>
    public Name Name { get; private set; } = default!;

    /// <summary>
    /// Obtém o número de documento associado a esta instância.
    /// </summary>
    public DocumentNumber DocumentNumber { get; private set; } = default!;

    /// <summary>
    /// Obtém o endereço de e-mail associado ao usuário.
    /// </summary>
    public Email Email { get; private set; } = default!;

    /// <summary>
    /// Obtém a data de nascimento do cliente.
    /// </summary>
    public DateTime? BirthDate { get; private set; }

    /// <summary>
    /// Obtém a data e hora UTC em que o objeto foi criado.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtém a data e hora em que a entidade foi atualizada pela última vez.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Obtém a coleção de propostas associadas a este cliente.
    /// </summary>
    public ICollection<Proposal> Proposals { get; init; } = [];

    /// <summary>
    /// Construtor privado para uso interno e por frameworks de ORM.
    /// </summary>
    private Client() { }

    /// <summary>
    /// Inicializa uma nova instância da classe Client com o nome, número de documento e endereço de e-mail especificados.
    /// </summary>
    /// <param name="name">O nome completo do cliente. Não pode ser nulo.</param>
    /// <param name="documentNumber">O número de documento único que identifica o cliente. Não pode ser nulo.</param>
    /// <param name="email">O endereço de e-mail associado ao cliente. Não pode ser nulo.</param>
    /// <param name="birthDate">A data de nascimento do cliente.</param>
    private Client(
        Name name,
        DocumentNumber documentNumber,
        Email email,
        DateTime? birthDate)
    {
        Name = name;
        DocumentNumber = documentNumber;
        Email = email;
        BirthDate = birthDate;
    }

    /// <summary>
    /// Cria uma nova instância da classe Client usando o nome, número de documento e endereço de e-mail especificados.
    /// </summary>
    /// <param name="name">O nome completo do cliente. Não pode ser nulo ou vazio.</param>
    /// <param name="documentNumber">O número de documento que identifica exclusivamente o cliente. Não pode ser nulo ou vazio.</param>
    /// <param name="email">O endereço de e-mail do cliente. Não pode ser nulo ou vazio. Deve estar em formato válido.</param>
    /// <param name="birthDate">A data de nascimento do cliente.</param>
    /// <returns>Um objeto Client inicializado com o nome, número de documento e endereço de e-mail fornecidos.</returns>
    public static Client Create(
        string name,
        string documentNumber,
        string email,
        DateTime? birthDate)
    {
        var nameVo = new Name(name);
        var documentNumberVo = new DocumentNumber(documentNumber);
        var emailVo = new Email(email);
        return new Client(nameVo, documentNumberVo, emailVo, birthDate);
    }

    /// <summary>
    /// Atualiza o nome, número de documento e endereço de e-mail do objeto atual com os valores especificados.
    /// </summary>
    /// <param name="name">O novo nome a ser atribuído. Não pode ser nulo ou vazio.</param>
    /// <param name="documentNumber">O novo número de documento a ser atribuído. Não pode ser nulo ou vazio.</param>
    /// <param name="email">O novo endereço de e-mail a ser atribuído. Não pode ser nulo ou vazio.</param>
    /// <param name="birthDate">A data de nascimento do cliente.</param>
    public void Update(string name, string documentNumber, string email, DateTime birthDate)
    {
        Name = new Name(name);
        DocumentNumber = new DocumentNumber(documentNumber);
        Email = new Email(email);
        BirthDate = birthDate;
        UpdatedAt = DateTime.UtcNow;
    }
}
