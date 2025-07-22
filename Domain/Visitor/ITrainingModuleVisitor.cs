using Domain.ValueObjects;

namespace Domain.Visitor;
public interface ITrainingModuleVisitor
{
    Guid Id { get; }
    List<PeriodDateTime> Periods { get; }
}
