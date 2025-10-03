using HiringService.Domain.Exceptions;
using HiringService.Domain.Interfaces.Services;

namespace HiringService.Domain.Services;

public sealed class CalculateEffectiveDateEnd : ICalculateEffectiveDateEnd
{
    public DateTime Calculate(DateTime effectiveDateStart, int months)
    {
        if (months <= 0)
            throw new DomainException("O número de meses deve ser maior que zero.", nameof(months));
        return effectiveDateStart.AddMonths(months);
    }
}
