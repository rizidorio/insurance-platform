namespace Insurence.Platform.Common.Messaging.RabbitMq.Configurations;

/// <summary>
/// Representa as configurações necessárias para conectar a um servidor RabbitMQ.
/// </summary>
/// <remarks>Esta classe encapsula os parâmetros necessários para estabelecer uma conexão com um servidor RabbitMQ,
/// incluindo host, porta, credenciais de autenticação e configurações específicas de fila. É tipicamente usada para configurar
/// clientes ou bibliotecas RabbitMQ.</remarks>
public sealed class RabbitMqSettings
{
    /// <summary>
    /// Obtém ou define o host do servidor RabbitMQ.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a porta do servidor RabbitMQ.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Obtém ou define o nome de usuário para autenticação no servidor RabbitMQ.
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Obtém ou define a senha para autenticação no servidor RabbitMQ.
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Obtém ou define o Virtual Host para a conexão RabbitMQ.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Obtém ou define o nome da fila onde os eventos de auditoria serão publicados.
    /// </summary>
    public string QueueName { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define se as confirmações do publicador estão habilitadas.
    /// </summary>
    public bool EnablePublisherConfirms { get; set; } = true;
}
