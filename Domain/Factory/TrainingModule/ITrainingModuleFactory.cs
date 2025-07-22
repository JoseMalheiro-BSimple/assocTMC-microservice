using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;
public interface ITrainingModuleFactory
{
    ITrainingModule Create(Guid id, List<PeriodDateTime> periods);
    ITrainingModule Create(ITrainingModuleVisitor visitor);
}
