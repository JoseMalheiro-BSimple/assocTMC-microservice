using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class Collaborator : ICollaborator
{
    public Guid Id { get; set; }
    public PeriodDateTime Period { get; set; } = new PeriodDateTime();
    public Collaborator()
    {
    }

    public Collaborator(Guid id, PeriodDateTime periodDateTime)
    {
        Id = id;
        Period = periodDateTime;
    }

}
