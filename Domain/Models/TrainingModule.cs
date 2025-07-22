using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class TrainingModule: ITrainingModule
{
    public Guid Id { get; private set; }
    public List<PeriodDateTime> Periods { get; private set; } = new List<PeriodDateTime>();
    public TrainingModule() { }

    public TrainingModule(Guid id, List<PeriodDateTime> periods)
    {
        Id = id;
        Periods = periods;
    }
}
