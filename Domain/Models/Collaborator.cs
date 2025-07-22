using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class Collaborator : ICollaborator
{
    public Guid Id { get; private set; }
    public PeriodDateTime Period { get; private set; } = new PeriodDateTime();
    public Collaborator()
    {
    }

    public Collaborator(Guid id, PeriodDateTime periodDateTime)
    {
        Id = id;
        Period = periodDateTime;
    }

    public void UpdatePeriod(PeriodDateTime period)
    {
        Period = period;
    }
}
