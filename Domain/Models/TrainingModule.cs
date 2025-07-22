using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class TrainingModule: ITrainingModule
{
    public Guid Id { get; set; }
    public List<PeriodDateTime> Periods { get; set; } = new List<PeriodDateTime>();
    public TrainingModule() { }

    public TrainingModule(Guid id, List<PeriodDateTime> periods)
    {
        Id = id;
        Periods = periods;
    }
}
