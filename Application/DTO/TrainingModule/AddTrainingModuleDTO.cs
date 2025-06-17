using Domain.Models;

namespace Application.DTO.TrainingModule;

public record AddTrainingModuleDTO
{
    public Guid TrainingSubjectId { get; set; }
    public List<PeriodDateTime> Periods { get; set; }

    public AddTrainingModuleDTO()
    {

    }
}