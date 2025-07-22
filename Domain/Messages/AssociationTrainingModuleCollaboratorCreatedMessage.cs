using Domain.ValueObjects;

namespace Domain.Messages;
public record AssociationTrainingModuleCollaboratorCreatedMessage(Guid Id, Guid TrainingModuleId, Guid CollaboratorId, PeriodDate PeriodDate);
    