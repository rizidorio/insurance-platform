using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Insurence.Platform.Common.Middlewares;

/// <summary>
/// Middleware que registra detalhes da requisição e resposta HTTP, incluindo método, caminho, corpo, código de status e duração do processamento.
/// </summary>
/// <remarks>Este middleware captura e registra os corpos completos das requisições e respostas para cada transação HTTP.
/// O registro ocorre antes e depois do processamento da requisição, incluindo o tempo decorrido para o tratamento. Utilize
/// este middleware para fins de diagnóstico ou auditoria. Os corpos das requisições e respostas são armazenados em memória, o que pode
/// impactar a performance para cargas grandes.</remarks>
/// <param name="next">O próximo delegate de middleware no pipeline de requisição HTTP. Usado para invocar os componentes subsequentes do middleware.</param>
/// <param name="logger">A instância de logger utilizada para registrar informações da requisição e resposta.</param>
public class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    /// <summary>
    /// Processa uma requisição HTTP registrando detalhes da requisição e resposta, incluindo método, caminho, corpo, código de status e duração.
    /// </summary>
    /// <remarks>Este método registra tanto os corpos da requisição recebida quanto da resposta enviada, além da
    /// duração do processamento da requisição. Ele substitui temporariamente o stream do corpo da resposta para capturar o conteúdo da resposta
    /// para fins de registro antes de restaurar o stream original. Este método deve ser utilizado como parte do pipeline de middleware do ASP.NET Core.</remarks>
    /// <param name="context">O contexto HTTP para a requisição atual. Fornece acesso às informações de requisição e resposta para processamento e
    /// registro.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de tratamento da requisição HTTP.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var requestBody = await GetRequestBodyAsync(context.Request);
        logger.LogInformation(
            "Request: {Method} {Path} - Body: {Body}",
            context.Request.Method,
            context.Request.Path,
            requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            var response = await GetResponseBodyAsync(context.Response);
            logger.LogInformation(
                "Response: {StatusCode} - Duration: {Duration}ms - Body: {Body}",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                response);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    /// <summary>
    /// Lê assincronamente todo o corpo da requisição HTTP especificada como uma string codificada em UTF-8.
    /// </summary>
    /// <remarks>A posição do stream do corpo da requisição é redefinida para o início após a leitura, permitindo leituras subsequentes.
    /// Este método habilita o buffering na requisição para suportar múltiplas leituras do corpo.</remarks>
    /// <param name="request">A requisição HTTP cujo corpo será lido. A requisição deve possuir um stream de corpo legível.</param>
    /// <returns>Uma string contendo todo o corpo da requisição. Retorna "empty" se o corpo estiver vazio.</returns>
    private static async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return string.IsNullOrEmpty(body) ? "empty" : body;
    }

    /// <summary>
    /// Lê assincronamente todo o corpo da resposta HTTP especificada e retorna seu conteúdo como uma string.
    /// </summary>
    /// <remarks>O método redefine a posição do stream do corpo da resposta antes e depois da leitura para garantir que
    /// operações subsequentes não sejam afetadas. O chamador é responsável por garantir que o stream do corpo da resposta seja
    /// seekable e esteja posicionado adequadamente para uso posterior.</remarks>
    /// <param name="response">A resposta HTTP cujo corpo será lido. O stream do corpo da resposta deve ser seekable.</param>
    /// <returns>Uma string contendo todo o conteúdo do corpo da resposta. Retorna "empty" se o corpo for nulo ou vazio.</returns>
    private static async Task<string> GetResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return string.IsNullOrEmpty(body) ? "empty" : body;
    }
}
