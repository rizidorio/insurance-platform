namespace HiringService.Domain.Interfaces.Services;

/// <summary>
/// Define um método para calcular a data final de um período efetivo com base em uma data de início e um número especificado de meses.
/// </summary>
/// <remarks>As implementações podem variar em como lidam com casos especiais, como anos bissextos ou meses com diferentes quantidades de dias. Os consumidores devem consultar a implementação específica para detalhes sobre a lógica de cálculo de datas.</remarks>
public interface ICalculateEffectiveDateEnd
{
    /// <summary>
    /// Calcula a data resultante da adição de um número especificado de meses à data de início efetiva.
    /// </summary>
    /// <param name="effectiveDateStart">A data inicial a partir da qual o cálculo começa. Representa a data de início efetiva.</param>
    /// <param name="months">O número de meses a adicionar à data de início efetiva. Deve ser zero ou maior.</param>
    /// <returns>Um valor DateTime representando a data que é o número especificado de meses após a data de início efetiva.</returns>
    DateTime Calculate(DateTime effectiveDateStart, int months);
}
