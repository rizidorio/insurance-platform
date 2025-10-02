using FluentValidation;
using FluentValidation.Results;
using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ProposalService.Api.Controllers.Base;

/// <summary>
/// Fornece uma classe controladora base para lidar com requisições de API com manipulação de respostas e lógica de validação comuns.
/// </summary>
/// <remarks>Este controlador é projetado para simplificar o processo de manipulação de requisições de API, fornecendo métodos
/// integrados de validação e criação de respostas. Ele suporta vários códigos de status HTTP e registra as respostas
/// de acordo.</remarks>
/// <typeparam name="TController">O tipo do controlador que herda de <see cref="MainController{TController}"/>.</typeparam>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class MainController<TController> : ControllerBase
    where TController : MainController<TController>
{
    private ILogger<TController>? _logger;

    public ILogger<TController> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<TController>>()
        ?? throw new InvalidOperationException($"Logger for {typeof(TController).Name} is not registered in the service collection.");

    /// <summary>
    /// Valida o objeto de requisição especificado usando um validador do tipo especificado.
    /// </summary>
    /// <typeparam name="TValidator">O tipo do validador a ser usado, que deve herdar de <see cref="AbstractValidator{TObject}"/> e ter um
    /// construtor sem parâmetros.</typeparam>
    /// <typeparam name="TObject">O tipo do objeto a ser validado.</typeparam>
    /// <param name="request">O objeto de requisição a ser validado. Não pode ser nulo.</param>
    /// <returns>Um <see cref="ValidationResult"/> contendo os resultados da validação.</returns>
    public ValidationResult ValidateRequest<TValidator, TObject>(TObject request)
        where TValidator : AbstractValidator<TObject>, new()
    {
        var validator = new TValidator();
        return validator.Validate(request);
    }

    /// <summary>
    /// Cria uma resposta de entidade não processável contendo erros de validação.
    /// </summary>
    /// <typeparam name="TObject">O tipo do objeto associado à resposta.</typeparam>
    /// <param name="validationResult">O resultado da validação contendo quaisquer erros.</param>
    /// <returns>Um <see cref="ActionResult{T}"/> contendo uma resposta com erros de validação.</returns>
    protected ActionResult<ResponseDefault<TObject>> ValidationResponseError<TObject>(ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => new Error("10000", e.ErrorMessage, e.PropertyName)).ToList();

        Logger.LogWarning("Validation failed with errors: {Errors}", string.Join(", ", errors.Select(e => e.Message)));

        return ResponseDefault<TObject>.CreateUnprocessableEntityResponse(errors);
    }

    /// <summary>
    /// Cria um <see cref="ActionResult{T}"/> com base no código de status dos dados de resposta fornecidos.
    /// </summary>
    /// <remarks>Registra a mensagem de resposta em diferentes níveis com base no código de status. Este método é
    /// destinado a padronizar a criação de respostas HTTP com base em um conjunto predefinido de códigos de status.</remarks>
    /// <typeparam name="TObject">O tipo do objeto contido nos dados de resposta. Deve ser não-nulo.</typeparam>
    /// <param name="data">Os dados de resposta contendo o código de status e a mensagem para determinar a resposta HTTP apropriada.</param>
    /// <returns>Um <see cref="ActionResult{T}"/> correspondente ao código de status nos dados de resposta. As respostas possíveis
    /// incluem: <list type="bullet"> <item><description><see cref="StatusCodes.Status200OK"/>: Retorna uma resposta 
    /// OK.</description></item> <item><description><see cref="StatusCodes.Status201Created"/>: Retorna uma resposta 
    /// Created.</description></item> <item><description><see cref="StatusCodes.Status204NoContent"/>: Retorna uma resposta 
    /// No Content.</description></item> <item><description><see cref="StatusCodes.Status400BadRequest"/>: Retorna uma resposta 
    /// Bad Request.</description></item> <item><description><see cref="StatusCodes.Status401Unauthorized"/>: Retorna uma resposta 
    /// Unauthorized.</description></item> <item><description><see cref="StatusCodes.Status403Forbidden"/>: Retorna uma resposta 
    /// Forbidden.</description></item> <item><description><see cref="StatusCodes.Status404NotFound"/>: Retorna uma resposta 
    /// Not Found.</description></item> <item><description><see cref="StatusCodes.Status409Conflict"/>: Retorna uma resposta 
    /// Conflict.</description></item> <item><description><see cref="StatusCodes.Status422UnprocessableEntity"/>: Retorna uma resposta 
    /// Unprocessable Entity.</description></item> <item><description>Qualquer outro código de status: Retorna uma resposta 
    /// Internal Server Error.</description></item> </list></returns>
    protected ActionResult<ResponseDefault<TObject>> CreateResponse<TObject>(ResponseDefault<TObject> data)
        where TObject : notnull
    {
        switch (data.StatusCode)
        {
            case StatusCodes.Status200OK:
                Logger.LogInformation("Returning OK response: {Message}", data.Message);
                return Ok(data);
            case StatusCodes.Status201Created:
                Logger.LogInformation("Returning Created response: {Message}", data.Message);
                return Created(string.Empty, data);
            case StatusCodes.Status204NoContent:
                Logger.LogInformation("Returning No Content response: {Message}", data.Message);
                return NoContent();
            case StatusCodes.Status400BadRequest:
                Logger.LogWarning("Returning Bad Request response with errors: {Message}", data.Message);
                return BadRequest(data);
            case StatusCodes.Status401Unauthorized:
                Logger.LogWarning("Returning Unauthorized response: {Message}", data.Message);
                return Unauthorized();
            case StatusCodes.Status403Forbidden:
                Logger.LogWarning("Returning Forbidden response: {Message}", data.Message);
                return StatusCode(StatusCodes.Status403Forbidden, data.Message);
            case StatusCodes.Status404NotFound:
                Logger.LogWarning("Returning Not Found response: {Message}", data.Message);
                return NotFound(data);
            case StatusCodes.Status409Conflict:
                Logger.LogWarning("Returning Conflict response: {Message}", data.Message);
                return Conflict(data);
            case StatusCodes.Status422UnprocessableEntity:
                Logger.LogWarning("Returning Unprocessable Entity response: {Message}", data.Message);
                return UnprocessableEntity(data);
            default:
                Logger.LogError("Returning Internal Server Error response: {Message}", data.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, data);
        }
    }
}
