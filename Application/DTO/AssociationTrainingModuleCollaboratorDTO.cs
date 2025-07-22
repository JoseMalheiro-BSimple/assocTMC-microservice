using Domain.ValueObjects;

namespace Application.DTO;

public record AssociationTrainingModuleCollaboratorDTO
{
    public Guid Id { get; set; }
    public Guid TrainingModuleId { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public AssociationTrainingModuleCollaboratorDTO()
    {

    }
}