using Domain.ValueObjects;

namespace Domain.Interfaces;
public interface IAssociationTrainingModuleCollaborator
{
    Guid Id { get; }
    Guid TrainingModuleId { get; }
    Guid CollaboratorId { get; }
    PeriodDate PeriodDate { get; }
}
