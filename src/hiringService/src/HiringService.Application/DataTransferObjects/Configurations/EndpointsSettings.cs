namespace HiringService.Application.DataTransferObjects.Configurations;

/// <summary>
/// Fornece configurações de URLs de endpoints de serviços utilizadas pela aplicação.
/// </summary>
public sealed class EndpointsSettings
{
    /// <summary>
    /// Obtém a URL base utilizada para acessar a API do serviço de propostas.
    /// </summary>
    public string ProposalServiceBaseUrl { get; init; } = null!;

    /// <summary>
    /// Obtém o tempo limite em segundos para requisições HTTP feitas aos serviços externos.
    /// </summary>
    public int TimeoutInSeconds { get; init; } = 30;
}
