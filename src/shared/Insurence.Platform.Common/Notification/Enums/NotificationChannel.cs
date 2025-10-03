namespace Insurence.Platform.Common.Notification.Enums;

/// <summary>
/// Especifica os canais disponíveis para entrega de notificações.
/// </summary>
/// <remarks>Use esta enumeração para selecionar o método preferido de entrega de notificação, como e-mail, SMS,
/// notificação push ou WhatsApp. Os valores podem ser combinados com outras APIs que exigem uma especificação de canal de notificação.</remarks>
public enum NotificationChannel
{
    /// <summary>
    /// Indica que o tipo de contato é um endereço de e-mail.
    /// </summary>
    Email = 1,

    /// <summary>
    /// Especifica que o canal de notificação é o Serviço de Mensagens Curtas (SMS).
    /// </summary>
    SMS = 2,

    /// <summary>
    /// Representa um tipo de mensagem de notificação push, normalmente usada para entregar alertas ou atualizações diretamente ao dispositivo do usuário.
    /// </summary>
    PushNotification = 3,

    /// <summary>
    /// Representa a plataforma de mensagens WhatsApp.
    /// </summary>
    Whatsapp = 4,
}
