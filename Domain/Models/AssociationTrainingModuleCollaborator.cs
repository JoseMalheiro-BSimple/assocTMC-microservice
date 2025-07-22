using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class AssociationTrainingModuleCollaborator : IAssociationTrainingModuleCollaborator
{
    public Guid Id { get; }
    public Guid TrainingModuleId { get; }
    public Guid CollaboratorId { get; }
    public PeriodDate PeriodDate { get; } = new PeriodDate();

    public AssociationTrainingModuleCollaborator()
    {
    }

    public AssociationTrainingModuleCollaborator(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        Id = id;
        TrainingModuleId = trainingModuleId;
        CollaboratorId = collaboratorId;
        PeriodDate = periodDate;
    }
}

