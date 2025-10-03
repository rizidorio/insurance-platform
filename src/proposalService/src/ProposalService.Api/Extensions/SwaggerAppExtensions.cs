namespace ProposalService.Api.Extensions;

/// <summary>
/// Configura o middleware do Swagger para a instância especificada de <see cref="WebApplication"/>.
/// </summary>
/// <remarks>Este método habilita os middlewares Swagger e Swagger UI para a aplicação quando executada em ambientes de desenvolvimento
/// ou homologação. Ele configura a interface Swagger UI na URL raiz e define o endpoint do Swagger como
/// "/swagger/v1/swagger.json".</remarks>
public static class SwaggerAppExtensions
{
    /// <summary>
    /// Configura o middleware do Swagger para o <see cref="WebApplication"/> especificado para gerar e servir
    /// documentação da API.
    /// </summary>
    /// <remarks>Este método habilita os middlewares Swagger e Swagger UI quando a aplicação está sendo executada em
    /// ambientes de desenvolvimento ou homologação. Ele configura a interface Swagger UI na URL raiz e define o endpoint
    /// da especificação da API como "/swagger/v1/swagger.json".</remarks>
    /// <param name="app">O <see cref="WebApplication"/> a ser configurado com a documentação do Swagger.</param>
    /// <returns>A instância configurada do <see cref="WebApplication"/>.</returns>
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance Proposal API");
                c.DefaultModelsExpandDepth(-1); // Set default model expand depth
                c.DefaultModelExpandDepth(-1);
                c.EnableDeepLinking();
            });
        }

        return app;
    }
}
