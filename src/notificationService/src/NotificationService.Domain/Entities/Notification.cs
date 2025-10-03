using Insurence.Platform.Common.Notification.Enums;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Entities;

/// <summary>
/// Representa uma mensagem de notificação com metadados, canal de entrega, status e informações de conteúdo. Fornece métodos
/// para criar, atualizar e gerenciar o ciclo de vida de uma instância de notificação.
/// </summary>
/// <remarks>Uma notificação encapsula detalhes como assunto, corpo, informações do destinatário e agendamento
/// de entrega. O status da notificação reflete seu estado atual no processo de entrega, incluindo pendente, enviada,
/// falha ou cancelada. Use o método estático Create para construir uma notificação válida, garantindo todos os campos obrigatórios
/// e restrições. A classe é imutável, exceto para atualizações de status e agendamento, que são gerenciadas através dos
/// métodos de instância fornecidos. Este tipo não é thread-safe; modificações concorrentes devem ser sincronizadas externamente
/// se necessário.</remarks>
public sealed class Notification
{
    /// <summary>
    /// Obtém o identificador único para esta instância.
    /// </summary>
    public uint Id { get; init; }

    /// <summary>
    /// Obtém o identificador externo único associado a esta instância.
    /// </summary>
    public Guid ExternalId { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Obtém o tipo de notificação representado por esta instância.
    /// </summary>
    public NotificationType Type { get; private set; }

    /// <summary>
    /// Obtém o canal de notificação associado a esta instância.
    /// </summary>
    public NotificationChannel Channel { get; private set; }

    /// <summary>
    /// Obtém o status atual da notificação.
    /// </summary>
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;

    /// <summary>
    /// Obtém o assunto associado a esta instância.
    /// </summary>
    public string Subject { get; private set; } = string.Empty;

    /// <summary>
    /// Obtém o conteúdo em texto do corpo da mensagem.
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// Obtém o endereço de e-mail associado ao usuário.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Obtém o número de telefone associado à instância atual.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Obtém a data e hora UTC em que o objeto foi criado.
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Obtém a data e hora UTC da última atualização do objeto.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Obtém a data e hora agendada para o envio da mensagem.
    /// </summary>
    public DateTime? SendIn { get; private set; }

    /// <summary>
    /// Obtém a mensagem de erro associada à última operação de envio, se ocorreu.
    /// </summary>
    public string? SendError { get; private set; }

    /// <summary>
    /// Construtor privado para uso interno e ORM.
    /// </summary>
    private Notification() { }

    /// <summary>
    /// Inicializa uma nova instância da classe Notification com o tipo, canal, assunto, corpo e
    /// detalhes opcionais do destinatário especificados.
    /// </summary>
    /// <param name="type">O tipo de notificação a ser enviada, como informativa, alerta ou erro.</param>
    /// <param name="channel">O canal de entrega da notificação, como e-mail, SMS ou push notification.</param>
    /// <param name="subject">O assunto ou título da mensagem de notificação.</param>
    /// <param name="body">O conteúdo principal ou corpo da mensagem de notificação.</param>
    /// <param name="email">O endereço de e-mail do destinatário se a notificação for enviada por e-mail; caso contrário, null.</param>
    /// <param name="phoneNumber">O número de telefone do destinatário se a notificação for enviada por SMS; caso contrário, null.</param>
    /// <param name="sendIn">A data e hora agendada para envio da notificação. Se null, a notificação é enviada imediatamente.</param>
    private Notification(
        NotificationType type,
        NotificationChannel channel,
        string subject,
        string body,
        string? email = null,
        string? phoneNumber = null,
        DateTime? sendIn = null)
    {
        Type = type;
        Channel = channel;
        Subject = subject;
        Body = body;
        Email = email;
        PhoneNumber = phoneNumber;
        SendIn = sendIn;
    }

    /// <summary>
    /// Cria uma nova instância de notificação com o tipo, canal, assunto, corpo e detalhes opcionais do destinatário
    /// especificados.
    /// </summary>
    /// <remarks>Os parâmetros devem ser consistentes com o canal de notificação selecionado. Por exemplo, um
    /// endereço de e-mail é obrigatório quando o canal é e-mail, e um número de telefone é obrigatório quando o canal é SMS.
    /// A validação é realizada em todos os parâmetros de entrada, e uma exceção é lançada se algum for inválido.</remarks>
    /// <param name="type">O tipo de notificação a ser criada. Determina a natureza e o propósito da notificação.</param>
    /// <param name="channel">O canal pelo qual a notificação será enviada, como e-mail ou SMS.</param>
    /// <param name="subject">O assunto da notificação. Não pode ser nulo ou vazio.</param>
    /// <param name="body">O conteúdo principal da mensagem de notificação. Não pode ser nulo ou vazio.</param>
    /// <param name="email">O endereço de e-mail do destinatário, se a notificação for enviada por e-mail. Pode ser nulo se não aplicável.</param>
    /// <param name="phoneNumber">O número de telefone do destinatário, se a notificação for enviada por SMS ou canais similares. Pode ser nulo se não
    /// aplicável.</param>
    /// <param name="sendIn">O horário agendado para envio da notificação. Se nulo, a notificação será enviada imediatamente.</param>
    /// <returns>Uma nova instância da classe Notification inicializada com os parâmetros fornecidos.</returns>
    public static Notification Create(
        NotificationType type,
        NotificationChannel channel,
        string subject,
        string body,
        string? email = null,
        string? phoneNumber = null,
        DateTime? sendIn = null)
    {
        NotificationTypeValidation(type);
        SubjectValidation(subject);
        BodyValidation(body);
        EmailValidation(email);
        PhoneNumberValidation(phoneNumber);
        SendNotificationChannelValidation(channel, email, phoneNumber);
        return new Notification(type, channel, subject, body, email, phoneNumber, sendIn);
    }

    /// <summary>
    /// Marca a notificação como enviada e registra o horário UTC atual como o timestamp de envio.
    /// </summary>
    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SendIn = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a notificação como falha e registra a mensagem de erro especificada.
    /// </summary>
    /// <remarks>Este método atualiza o status da notificação para falha e define o horário de envio para o horário
    /// UTC atual. Use este método para indicar que o envio da notificação não foi bem-sucedido e fornecer detalhes
    /// sobre a falha.</remarks>
    /// <param name="errorMessage">A mensagem de erro descrevendo o motivo da falha. Não pode ser nula.</param>
    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        SendError = errorMessage;
        SendIn = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca a notificação como cancelada atualizando seu status.
    /// </summary>
    /// <remarks>Após chamar este método, o status da notificação é definido para indicar cancelamento. Esta
    /// operação é idempotente; chamadas repetidas não têm efeito adicional se a notificação já estiver
    /// cancelada.</remarks>
    public void MarkAsCancelled()
    {
        Status = NotificationStatus.Cancelled;
        SendIn = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reagenda a notificação para ser enviada na data e hora especificadas.
    /// </summary>
    /// <remarks>Se a notificação for reagendada, seu status é definido como pendente e qualquer erro de envio anterior
    /// é limpo.</remarks>
    /// <param name="newSendIn">A nova data e hora em que a notificação deve ser enviada.</param>
    /// <exception cref="InvalidOperationException">Lançada se a notificação já foi enviada e não pode ser reagendada.</exception>
    public void Reschedule(DateTime newSendIn)
    {
        if (Status == NotificationStatus.Sent)
        {
            throw new InvalidOperationException("Notificação já foi enviada e não pode ser reagendada.");
        }
        SendIn = newSendIn;
        Status = NotificationStatus.Pending;
        SendError = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Valida se o tipo de notificação especificado está definido na enumeração NotificationType.
    /// </summary>
    /// <param name="type">O tipo de notificação a validar. Deve ser um valor válido da enumeração NotificationType.</param>
    /// <exception cref="ArgumentException">Lançada quando <paramref name="type"/> não é um valor definido da enumeração NotificationType.</exception>
    private static void NotificationTypeValidation(NotificationType type)
    {
        if (!Enum.IsDefined(typeof(NotificationType), type))
        {
            throw new ArgumentException("Tipo de notificação inválido.", nameof(type));
        }
    }

    /// <summary>
    /// Valida se o assunto da notificação não é nulo, vazio ou maior que 200 caracteres.
    /// </summary>
    /// <param name="subject">O assunto da notificação a validar. Não deve ser nulo, vazio ou composto apenas por espaços, e não deve
    /// exceder 200 caracteres.</param>
    /// <exception cref="ArgumentException">Lançada se <paramref name="subject"/> for nulo, vazio, composto apenas por espaços ou exceder 200 caracteres.</exception>
    private static void SubjectValidation(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("O assunto da notificação não pode ser vazio.", nameof(subject));
        }
        if (subject.Length > 200)
        {
            throw new ArgumentException("O assunto da notificação não pode exceder 200 caracteres.", nameof(subject));
        }
    }

    /// <summary>
    /// Valida se o corpo da notificação não é nulo, vazio ou maior que 255 caracteres.
    /// </summary>
    /// <param name="body">O corpo da notificação a validar. Não deve ser nulo, vazio ou composto apenas por espaços, e não deve exceder
    /// 255 caracteres.</param>
    /// <exception cref="ArgumentException">Lançada se <paramref name="body"/> for nulo, vazio, composto apenas por espaços ou exceder 255 caracteres.</exception>
    private static void BodyValidation(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("O corpo da notificação não pode ser vazio.", nameof(body));
        }
        if (body.Length > 255)
        {
            throw new ArgumentException("O corpo da notificação não pode exceder 255 caracteres.", nameof(body));
        }
    }
    
    /// <summary>
    /// Valida se o endereço de e-mail especificado está em um formato válido.
    /// </summary>
    /// <param name="email">O endereço de e-mail a validar. Se <paramref name="email"/> for <see langword="null"/>, nenhuma validação é
    /// realizada.</param>
    /// <exception cref="ArgumentException">Lançada se <paramref name="email"/> não for <see langword="null"/> e não corresponder a um formato válido de endereço de e-mail.</exception>
    private static void EmailValidation(string? email)
    {
        if (email != null)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("Endereço de e-mail inválido.", nameof(email));
            }
        }
    }

    /// <summary>
    /// Valida se o número de telefone especificado está conforme o formato internacional E.164.
    /// </summary>
    /// <remarks>O formato E.164 exige que o número de telefone comece com um '+' opcional seguido de até 15
    /// dígitos, com o primeiro dígito entre 1 e 9. Este método não verifica a validade específica do país ou
    /// existência do número.</remarks>
    /// <param name="phoneNumber">O número de telefone a validar. O valor pode ser nulo; se não for nulo, deve estar no formato E.164 (ex.:
    /// '+1234567890').</param>
    /// <exception cref="ArgumentException">Lançada se <paramref name="phoneNumber"/> não for nulo e não corresponder ao formato E.164.</exception>
    private static void PhoneNumberValidation(string? phoneNumber)
    {
        if (phoneNumber != null)
        {
            var phonePattern = @"^\+?[1-9]\d{1,14}$"; // Formato E.164
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, phonePattern))
            {
                throw new ArgumentException("Número de telefone inválido.", nameof(phoneNumber));
            }
        }
    }

    /// <summary>
    /// Valida as informações de contato obrigatórias para o canal de notificação especificado.
    /// </summary>
    /// <remarks>Para notificações push, nem e-mail nem número de telefone são obrigatórios. Este método não envia
    /// nenhuma notificação; apenas valida os parâmetros de entrada para o canal especificado.</remarks>
    /// <param name="channel">O canal de notificação a validar. Deve ser um valor definido da enumeração NotificationChannel.</param>
    /// <param name="email">O endereço de e-mail a validar para notificações por e-mail. Obrigatório se <paramref name="channel"/> for <see
    /// cref="NotificationChannel.Email"/>; caso contrário, pode ser nulo.</param>
    /// <param name="phoneNumber">O número de telefone a validar para notificações por SMS ou WhatsApp. Obrigatório se <paramref name="channel"/> for <see
    /// cref="NotificationChannel.SMS"/> ou <see cref="NotificationChannel.Whatsapp"/>; caso contrário, pode ser nulo.</param>
    /// <exception cref="ArgumentException">Lançada se <paramref name="channel"/> não for um valor válido de NotificationChannel, ou se as informações de contato obrigatórias
    /// estiverem ausentes para o canal especificado.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Lançada se <paramref name="channel"/> for um canal de notificação desconhecido.</exception>
    private static void SendNotificationChannelValidation(NotificationChannel channel, string? email, string? phoneNumber)
    {
        if (!Enum.IsDefined(typeof(NotificationChannel), channel))
        {
            throw new ArgumentException("Canal de notificação inválido.", nameof(channel));
        }

        switch (channel)
        {
            case NotificationChannel.Email:
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("O endereço de e-mail é obrigatório para notificações por e-mail.", nameof(email));
                }
                break;
            case NotificationChannel.SMS:
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    throw new ArgumentException("O número de telefone é obrigatório para notificações por SMS.", nameof(phoneNumber));
                }
                break;
            case NotificationChannel.PushNotification:
                // Notificações push podem não exigir e-mail ou número de telefone
                break;
            case NotificationChannel.Whatsapp:
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    throw new ArgumentException("O número de telefone é obrigatório para notificações por WhatsApp.", nameof(phoneNumber));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(channel), "Canal de notificação desconhecido.");
        }
    }
}
