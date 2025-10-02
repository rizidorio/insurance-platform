namespace Insurence.Platform.Common.Wrappers;

/// <summary>
/// Representa uma estrutura de resposta padronizada para operações de API, encapsulando status de sucesso, código de status,
/// mensagem opcional, payload de dados e detalhes de erro.
/// </summary>
/// <remarks>Esta classe fornece métodos de fábrica para criar tipos comuns de resposta HTTP, como OK, Created, No
/// Content, Bad Request, Unauthorized e outros. É projetada para simplificar a criação de respostas
/// de API consistentes.</remarks>
/// <typeparam name="TObject">O tipo do payload de dados incluído na resposta.</typeparam>
/// <param name="success"></param>
/// <param name="message"></param>
/// <param name="statusCode"></param>
/// <param name="data"></param>
/// <param name="errors"></param>
public sealed class ResponseDefault<TObject>(
    bool success,
    string? message = null,
    int statusCode = 200,
    TObject? data = default,
    IList<Error>? errors = null)
{
    /// <summary>
    /// Obtém um valor indicando se a operação foi bem-sucedida.
    /// </summary>
    public bool Success { get; } = success;

    /// <summary>
    /// Obtém a mensagem associada à instância atual.
    /// </summary>
    public string? Message { get; } = message;

    /// <summary>
    /// Obtém o código de status HTTP associado à resposta.
    /// </summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Obtém o objeto de dados associado a esta instância.
    /// </summary>
    public TObject? Data { get; } = data;

    /// <summary>
    /// Obtém a coleção de erros associados à operação ou contexto atual.
    /// </summary>
    public IList<Error> Errors { get; } = errors ?? [];

    /// <summary>
    /// Cria uma resposta bem-sucedida com os dados especificados e mensagem opcional.
    /// </summary>
    /// <param name="data">Os dados a serem incluídos na resposta. Pode ser <see langword="null"/> se nenhum dado for fornecido.</param>
    /// <param name="message">Uma mensagem opcional descrevendo a resposta. Pode ser <see langword="null"/>.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta bem-sucedida com código de status 200.</returns>
    public static ResponseDefault<TObject> CreateSuccessResponse(TObject? data, string? message = null)
    {
        return new ResponseDefault<TObject>(true, message, 200, data);
    }

    /// <summary>
    /// Cria um objeto de resposta indicando uma criação bem-sucedida de recurso.
    /// </summary>
    /// <param name="data">Os dados associados ao recurso criado. Pode ser <see langword="null"/> se nenhum dado for fornecido.</param>
    /// <param name="message">Uma mensagem opcional descrevendo o resultado da operação. Pode ser <see langword="null"/>.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta de criação bem-sucedida, com um código
    /// de status 201 (Created).</returns>
    public static ResponseDefault<TObject> CreateCreatedResponse(TObject? data, string? message = null)
    {
        return new ResponseDefault<TObject>(true, message, 201, data);
    }

    /// <summary>
    /// Cria uma resposta indicando nenhum conteúdo com uma mensagem opcional.
    /// </summary>
    /// <param name="message">Uma mensagem opcional fornecendo contexto adicional sobre a resposta. Se <see langword="null"/>, nenhuma mensagem será
    /// incluída na resposta.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta bem-sucedida com código de status 204
    /// (No Content).</returns>
    public static ResponseDefault<TObject> CreateNoContentResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(true, message, 204, default);
    }

    /// <summary>
    /// Cria um objeto de resposta representando um erro "Bad Request" (HTTP 400).
    /// </summary>
    /// <param name="errors">Uma lista de erros que descrevem os motivos da requisição inválida. Não pode ser nulo.</param>
    /// <param name="message">Uma mensagem opcional fornecendo contexto adicional sobre a requisição inválida. Se não especificada, a mensagem será
    /// nula.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> indicando uma operação falha com código de status 400. A
    /// resposta inclui os erros fornecidos e mensagem opcional.</returns>
    public static ResponseDefault<TObject> CreateBadRequestResponse(IList<Error> errors, string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 400, default, errors);
    }

    /// <summary>
    /// Cria uma resposta indicando que a requisição não foi autorizada.
    /// </summary>
    /// <param name="message">Uma mensagem opcional fornecendo detalhes adicionais sobre a resposta não autorizada. Se <see langword="null"/>, nenhuma
    /// mensagem será incluída na resposta.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta não autorizada, com código de status
    /// 401 e um valor padrão para o objeto de resposta.</returns>
    public static ResponseDefault<TObject> CreateUnauthorizedResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 401, default);
    }

    /// <summary>
    /// Cria uma resposta indicando que a operação solicitada é proibida.
    /// </summary>
    /// <param name="message">Uma mensagem opcional fornecendo detalhes adicionais sobre por que a operação é proibida. Se <see
    /// langword="null"/>, nenhuma mensagem será incluída na resposta.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta proibida. A resposta tem código
    /// de status 403, valor de sucesso <see langword="false"/> e um valor de objeto padrão.</returns>
    public static ResponseDefault<TObject> CreateForbiddenResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 403, default);
    }

    /// <summary>
    /// Cria uma resposta indicando que o recurso solicitado não foi encontrado.
    /// </summary>
    /// <param name="message">Uma mensagem opcional fornecendo detalhes adicionais sobre o status de não encontrado. Se nulo, uma mensagem padrão pode ser
    /// usada.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando uma resposta de não encontrado, com código de status 404
    /// e um valor padrão para o objeto.</returns>
    public static ResponseDefault<TObject> CreateNotFoundResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 404, default);
    }

    /// <summary>
    /// Cria uma resposta indicando um erro de conflito (código de status HTTP 409).
    /// </summary>
    /// <param name="message">Uma mensagem opcional descrevendo o conflito. Se <see langword="null"/>, nenhuma mensagem será incluída na
    /// resposta.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> representando a resposta de conflito, com código de status 409.</returns>
    public static ResponseDefault<TObject> CreateConflictResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 409, default);
    }

    /// <summary>
    /// Cria uma resposta indicando que a requisição não pôde ser processada devido a erros semânticos.
    /// </summary>
    /// <param name="errors">Uma lista de erros que descrevem os motivos do status de entidade não processável.</param>
    /// <param name="message">Uma mensagem opcional fornecendo contexto adicional sobre a resposta. Pode ser <see langword="null"/>.</param>
    /// <returns>Um objeto <see cref="ResponseDefault{TObject}"/> representando a resposta de entidade não processável, com um código de 
    /// status 422.</returns>
    public static ResponseDefault<TObject> CreateUnprocessableEntityResponse(IList<Error> errors, string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 422, default, errors);
    }

    /// <summary>
    /// Cria um objeto de resposta representando um erro interno do servidor.
    /// </summary>
    /// <param name="message">Uma mensagem opcional descrevendo o erro. Se nulo, uma mensagem de erro padrão pode ser usada.</param>
    /// <returns>Uma instância de <see cref="ResponseDefault{TObject}"/> indicando a falha, com código de status 500. O
    /// objeto de resposta contém a mensagem de erro fornecida e um valor padrão para os dados.</returns>
    public static ResponseDefault<TObject> CreateInternalServerErrorResponse(string? message = null)
    {
        return new ResponseDefault<TObject>(false, message, 500, default);
    }

    /// <summary>
    /// Cria um objeto de resposta personalizado com o status de sucesso, mensagem, código de status, dados e erros especificados.
    /// </summary>
    /// <param name="success">Um valor indicando se a operação foi bem-sucedida. <see langword="true"/> se a operação foi bem-sucedida;
    /// caso contrário, <see langword="false"/>.</param>
    /// <param name="message">Uma mensagem opcional fornecendo informações adicionais sobre a resposta. Pode ser <see langword="null"/> se nenhuma
    /// mensagem for fornecida.</param>
    /// <param name="statusCode">O código de status HTTP associado à resposta. O padrão é 200.</param>
    /// <param name="data">O payload de dados opcional a ser incluído na resposta. Pode ser <see langword="null"/> ou o valor padrão de
    /// <typeparamref name="TObject"/>.</param>
    /// <param name="errors">Uma lista opcional de erros associados à resposta. Pode ser <see langword="null"/> se não houver erros presentes.</param>
    /// <returns>Um objeto <see cref="ResponseDefault{TObject}"/> contendo os detalhes de resposta especificados.</returns>
    public static ResponseDefault<TObject> CreateCustomResponse(bool success, string? message = null, int statusCode = 200, TObject? data = default, IList<Error>? errors = null)
    {
        return new ResponseDefault<TObject>(success, message, statusCode, data, errors);
    }
}
