using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Infrastructure.DataModel;
public class TrainingModuleDataModel : ITrainingModuleVisitor
{
    public Guid Id { get; set; }
    public List<PeriodDateTime> Periods { get; set; } = new List<PeriodDateTime>();

    public TrainingModuleDataModel() { }

    public TrainingModuleDataModel(ITrainingModule trainingModule)
    {
        Id = trainingModule.Id;
        Periods = trainingModule.Periods;
    }
}
