using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class AssociationTrainingModuleCollaborator : IAssociationTrainingModuleCollaborator
{
    public Guid Id { get; private set; }
    public Guid TrainingModuleId { get; private set; }
    public Guid CollaboratorId { get; private set; }
    public PeriodDate PeriodDate { get; private set; } = new PeriodDate();

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

