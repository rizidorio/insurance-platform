using Insurence.Platform.Common.Messaging.RabbitMq.Configurations;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces;
using Insurence.Platform.Common.Messaging.RabbitMq.Messages;
using Microsoft.Extensions.Options;
using NotificationService.Application.DataTransferObjects.Requests;
using NotificationService.Application.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Worker;

/// <summary>
/// Fornece um serviço em segundo plano que escuta mensagens de uma fila RabbitMQ e processa solicitações de notificação.
/// </summary>
/// <remarks>
/// Este serviço estabelece uma conexão com o RabbitMQ e consome mensagens da fila configurada. Cada mensagem é desserializada e processada como uma solicitação de notificação. O worker gerencia seu próprio ciclo de vida de conexão e canal, e registra erros ou status de processamento. O serviço deve rodar continuamente até ser interrompido, sendo adequado para ambientes hospedados ASP.NET Core. Liberação de recursos e tratamento de erros são realizados durante o descarte e processamento de mensagens.
/// </remarks>
/// <param name="logger">O logger utilizado para registrar informações operacionais e de diagnóstico do worker.</param>
/// <param name="connectionFactory">A fábrica utilizada para criar conexões com o servidor RabbitMQ.</param>
/// <param name="settings">As opções de configuração do RabbitMQ, incluindo nome da fila e detalhes de conexão.</param>
/// <param name="serviceProvider">O provedor de serviços utilizado para resolver serviços e dependências da aplicação dentro do escopo do worker.</param>
public class Worker(
    ILogger<Worker> logger,
    IRabbitMqConnectionFactory connectionFactory,
    IOptions<RabbitMqSettings> settings,
    IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IRabbitMqConnectionFactory _connectionFactory = connectionFactory;
    private readonly RabbitMqSettings _settings = settings.Value;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;

    /// <summary>
    /// Executa a operação do worker em segundo plano de forma assíncrona e escuta notificações até que o cancelamento seja solicitado.
    /// </summary>
    /// <remarks>
    /// Este método é chamado pelo host para iniciar o serviço em segundo plano. A operação continua rodando até que o token de cancelamento seja acionado. Se ocorrer uma exceção não tratada durante a execução, ela é registrada e relançada.
    /// </remarks>
    /// <param name="stoppingToken">Um token de cancelamento que pode ser utilizado para sinalizar a solicitação de parada da operação em segundo plano.</param>
    /// <returns>Uma tarefa que representa a execução assíncrona do worker em segundo plano.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Iniciando o worker: {time}", DateTimeOffset.Now);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISendNotificationService>();
            await InitializeRabbitMqListenerAsync(service, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker aguardando mensagens...");
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar o worker: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Libera todos os recursos utilizados pela instância atual da classe.
    /// </summary>
    /// <remarks>
    /// Este método fecha e descarta quaisquer conexões e canais abertos associados à instância. É seguro chamar múltiplas vezes; chamadas subsequentes não têm efeito. Após chamar este método, a instância não deve ser utilizada. Este método suprime a finalização para otimizar a coleta de lixo.
    /// </remarks>
    public override void Dispose()
    {
        try
        {
            _channel?.CloseAsync().GetAwaiter().GetResult();
            _channel?.Dispose();
            _connection?.CloseAsync().GetAwaiter().GetResult();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao liberar recursos.");
        }
        finally
        {
            base.Dispose();
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Inicializa o listener do RabbitMQ e começa a consumir mensagens da fila configurada de forma assíncrona.
    /// </summary>
    /// <remarks>
    /// Este método configura a fila, define o prefetch de mensagens e inicia o consumo das mensagens. Mensagens recebidas da fila são processadas utilizando o serviço de notificação fornecido. Se o token de cancelamento for acionado, o listener irá parar de consumir mensagens.
    /// </remarks>
    /// <param name="sendNotification">O serviço de notificação utilizado para processar e enviar notificações para cada mensagem recebida.</param>
    /// <param name="stoppingToken">Um token de cancelamento que pode ser utilizado para cancelar a inicialização do listener e a operação de consumo de mensagens.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de inicialização do listener RabbitMQ.</returns>
    /// <exception cref="InvalidOperationException">Lançada se não for possível estabelecer uma conexão com o RabbitMQ.</exception>
    private async Task InitializeRabbitMqListenerAsync(ISendNotificationService sendNotification, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Iniciando listener RabbitMQ...");
        _connection = await _connectionFactory.CreateConnectionAsync();

        if (_connection == null || !_connection.IsOpen)
        {
            _logger.LogError("Falha ao conectar ao RabbitMQ.");
            throw new InvalidOperationException("Não foi possível estabelecer conexão com o RabbitMQ.");
        }

        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Mensagem recebida: {Message}", message);

                var notificationMessage = JsonSerializer.Deserialize<NotificationMessage>(message);

                if (notificationMessage == null)
                {
                    _logger.LogWarning("Mensagem inválida ou nula recebida.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                var request = new SendNotificationRequest(
                    CorrelationId: notificationMessage.CorrelationId,
                    NotificationType: notificationMessage.NotificationType,
                    NotificationChannel: notificationMessage.NotificationChannel,
                    Subject: notificationMessage.Subject,
                    Body: notificationMessage.Body,
                    Email: notificationMessage.Email,
                    PhoneNumber: notificationMessage.PhoneNumber);

                var response = await sendNotification.SendAsync(request, stoppingToken);

                if (response is null)
                {
                    _logger.LogWarning("Falha ao processar a notificação para CorrelationId: {CorrelationId}", request.CorrelationId);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                    return;
                }

                _logger.LogInformation("Notificação processada com sucesso. CorrelationId: {CorrelationId}, Status: {Status}",
                    response.CorrelationId, response.Status);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a mensagem.");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: _settings.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("Consumer iniciado e aguardando mensagens na fila: {QueueName}", _settings.QueueName);
    }
}
