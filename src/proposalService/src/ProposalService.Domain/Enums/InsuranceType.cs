namespace ProposalService.Domain.Enums;

/// <summary>
/// Especifica os tipos de seguro disponíveis no sistema.
/// </summary>
/// <remarks>Use esta enumeração para indicar a categoria de seguro para uma apólice, sinistro ou operação relacionada.
/// Os valores correspondem a domínios comuns de seguro, como vida, veículo, propriedade e saúde. O valor <see
/// cref="InsuranceType.None"/> representa a ausência de um tipo de seguro e pode ser usado como padrão ou estado não inicializado.</remarks>
public enum InsuranceType
{
    /// <summary>
    /// Indica que nenhum valor ou opção foi selecionado.
    /// </summary>
    None = 0,

    /// <summary>
    /// Representa o tipo de apólice de seguro 'Vida'.
    /// </summary>
    Life = 1,

    /// <summary>
    /// Representa um veículo como tipo de entidade.
    /// </summary>
    Vehicle = 2,

    /// <summary>
    /// Especifica que o membro representa uma propriedade.
    /// </summary>
    Property = 3,

    /// <summary>
    /// Representa a categoria saúde para status ou enumeração de métricas.
    /// </summary>
    Health = 4,
}
