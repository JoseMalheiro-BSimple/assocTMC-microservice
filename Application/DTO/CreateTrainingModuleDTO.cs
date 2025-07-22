using Domain.ValueObjects;

namespace Application.DTO;

public record CreateTrainingModuleDTO
{
    public Guid Id { get; set; }
    public List<PeriodDateTime> Periods { get; set; } = new List<PeriodDateTime>();

    public CreateTrainingModuleDTO(Guid id, List<PeriodDateTime> periods)
    {
        Id = id;
        Periods = periods;
    }
}
