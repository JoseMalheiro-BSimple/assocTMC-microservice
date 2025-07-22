using Domain.ValueObjects;

namespace Application.DTO;

public record CreateCollaboratorDTO
{
    public Guid Id { get; set; }
    public PeriodDateTime Period { get; set; } = new PeriodDateTime();

    public CreateCollaboratorDTO(Guid id, PeriodDateTime period)
    {
        Id = id;
        Period = period;
    }
}
