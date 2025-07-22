using Domain.ValueObjects;

namespace Application.DTO;

public record CreateConsumedAssociationTrainingModuleCollaboratorDTO
{
    public Guid Id { get; set; }
    public Guid CollaboratorId { get; set; }
    public Guid TrainingModuleId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public CreateConsumedAssociationTrainingModuleCollaboratorDTO(Guid id, Guid collaboratorId, Guid trainingModuleId, PeriodDate periodDate)
    {
        Id = id;
        CollaboratorId = collaboratorId;
        TrainingModuleId = trainingModuleId;
        PeriodDate = periodDate;
    }
}
