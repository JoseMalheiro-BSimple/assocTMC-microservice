using Domain.ValueObjects;

namespace Domain.Interfaces;
public interface ITrainingModule
{
    Guid Id { get; }
    List<PeriodDateTime> Periods { get; }
}
