using Domain.Models;

namespace Domain.Messages;
public record AssociationTrainingModuleCollaboratorCreated(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    