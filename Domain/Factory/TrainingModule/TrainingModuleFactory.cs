using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;
public class TrainingModuleFactory : ITrainingModuleFactory
{
    public TrainingModuleFactory() { }

    public ITrainingModule Create(Guid id, List<PeriodDateTime> periods)
    {
        return new TrainingModule(id, periods);
    }

    public ITrainingModule Create(ITrainingModuleVisitor visitor)
    {
        return new TrainingModule(visitor.Id, visitor.Periods);
    }
}
