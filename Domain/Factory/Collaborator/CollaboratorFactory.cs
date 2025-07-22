using Domain.Interfaces;
using Domain.Visitor;
using Domain.Models;
using Domain.ValueObjects;

namespace Domain.Factory;
public class CollaboratorFactory : ICollaboratorFactory
{
    public CollaboratorFactory() { }

    public ICollaborator Create(Guid id, PeriodDateTime period)
    {
        return new Collaborator(id, period);
    }

    public ICollaborator Create(ICollaboratorVisitor visitor)
    {
        return new Collaborator(visitor.Id, visitor.Period);
    }
}
