using FluentValidation;
using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Insurence.Platform.Common.Middlewares;

/// <summary>
/// Middleware que trata exceções não tratadas durante o processamento de requisições HTTP e retorna respostas de erro padronizadas.
/// </summary>
/// <remarks>
/// Este middleware intercepta exceções lançadas por componentes subsequentes e as mapeia para códigos de status HTTP e mensagens de erro apropriadas.
/// Ele garante que os clientes recebam respostas de erro JSON consistentes para tipos comuns de exceção, como erros de validação, recurso não encontrado, argumentos inválidos e operações inválidas.
/// Para exceções inesperadas, uma mensagem de erro genérica e um status 500 são retornados.
/// Coloque este middleware no início do pipeline para garantir um tratamento abrangente de exceções.
/// </remarks>
/// <param name="next">O próximo delegate de middleware no pipeline de requisição. Invocado para continuar o processamento da requisição HTTP.</param>
/// <param name="logger">O logger utilizado para registrar detalhes da exceção e informações de erro.</param>
public sealed class ExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Trata uma exceção registrando o erro e escrevendo uma resposta de erro JSON padronizada no contexto HTTP.
    /// </summary>
    /// <remarks>
    /// A resposta inclui um código de status HTTP apropriado e uma mensagem de erro amigável ao usuário, baseada no tipo da exceção.
    /// Os tipos de exceção suportados incluem erros de validação, recurso não encontrado, argumentos inválidos e operações inválidas.
    /// Todas as outras exceções resultam em uma resposta genérica de erro interno do servidor.
    /// A resposta é formatada como JSON e enviada com o content type 'application/json'.
    /// </remarks>
    /// <param name="context">O contexto HTTP da requisição atual. A resposta de erro será escrita neste contexto.</param>
    /// <param name="exception">A exceção a ser tratada. Determina o código de status HTTP e a mensagem de erro retornados ao cliente.</param>
    /// <returns>Uma task que representa a operação assíncrona de tratar a exceção e escrever a resposta.</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Ocorreu um erro não tradado: {Message}.", exception.Message);
        
        var (statusCode, message) = exception switch
        {
            ValidationException => (StatusCodes.Status422UnprocessableEntity, "Ocorreu erro na validação."),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "O recurso solicitado não foi encontrado."),
            ArgumentException => (StatusCodes.Status400BadRequest, "Um ou mais argumentos fornecidos são inválidos."),
            InvalidOperationException => (StatusCodes.Status409Conflict, "A operação solicitada não é válida no estado atual do recurso."),
            _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ResponseDefault<string>(
            success: false,
            message: message,
            statusCode: statusCode
        );
        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
