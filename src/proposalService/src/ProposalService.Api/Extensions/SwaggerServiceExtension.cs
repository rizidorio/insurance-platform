using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ProposalService.Api.Extensions;

/// <summary>
/// Fornece métodos de extensão para configurar a documentação Swagger em uma aplicação ASP.NET Core.
/// </summary>
/// <remarks>Esta classe estática contém métodos para adicionar e configurar serviços Swagger, permitindo a documentação
/// da API e geração de UI. Inclui configurações para versionamento de API, comentários XML e definições de segurança para
/// autenticação JWT.</remarks>
public static class SwaggerServiceExtensions
{
    /// <summary>
    /// Adiciona serviços de documentação Swagger à <see cref="IServiceCollection"/> especificada.
    /// </summary>
    /// <remarks>Este método configura o Swagger para gerar documentação da API para a aplicação. Ele define
    /// a versão da API, título e descrição, e inclui comentários XML do assembly em execução. Adicionalmente,
    /// configura autenticação por token JWT Bearer para a documentação da API.</remarks>
    /// <param name="services">A <see cref="IServiceCollection"/> à qual os serviços Swagger são adicionados.</param>
    /// <returns>A <see cref="IServiceCollection"/> modificada com os serviços Swagger configurados.</returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Insurance Proposal API",
                Description = "API de gerenciamento de Proposta",
                Contact = new OpenApiContact
                {
                    Name = "Ricardo Izidorio Ferreira",
                    Email = "rizidorio@outlook.com"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    Description = "Informe o token JWT no formato: Bearer {seu token}",
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer"
            //});

            //options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //{
            //    new OpenApiSecurityScheme {
            //        Reference = new OpenApiReference {
            //            Type = ReferenceType.SecurityScheme,
            //            Id = "Bearer"
            //        }
            //    },
            //    Array.Empty<string>()
            //}
            //});
        });

        return services;
    }
}