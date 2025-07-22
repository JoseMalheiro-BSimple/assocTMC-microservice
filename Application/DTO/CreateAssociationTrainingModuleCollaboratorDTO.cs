using Domain.ValueObjects;

namespace Application.DTO;

public record CreateAssociationTrainingModuleCollaboratorDTO
{
    public Guid CollaboratorId { get; set; }
    public Guid TrainingModuleId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public CreateAssociationTrainingModuleCollaboratorDTO(Guid collaboratorId, Guid trainingModuleId, PeriodDate periodDate)
    {
        CollaboratorId = collaboratorId;
        TrainingModuleId = trainingModuleId;
        PeriodDate = periodDate;
    }
}