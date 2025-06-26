using Domain.Models;

namespace Domain.Messaging;
public record AssociationTrainingModuleCollaboratorCreated(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    