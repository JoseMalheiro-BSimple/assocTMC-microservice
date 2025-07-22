using Domain.ValueObjects;

namespace Domain.Interfaces;
public interface ICollaborator
{
    Guid Id { get; }
    PeriodDateTime Period { get; }
    void UpdatePeriod(PeriodDateTime period);
}
