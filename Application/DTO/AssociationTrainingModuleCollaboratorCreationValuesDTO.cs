using Domain.ValueObjects;

namespace Application.DTO;

public record AssociationTrainingModuleCollaboratorCreationValuesDTO(Guid CollaboratorId, Guid TrainingModuleId, PeriodDate PeriodDate);
