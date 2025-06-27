using Domain.Interfaces;
using Domain.Visitor;
using Domain.Models;

namespace Domain.Factory;
public class CollaboratorFactory : ICollaboratorFactory
{
    public CollaboratorFactory() { }

    public ICollaborator Create(Guid id)
    {
        return new Collaborator(id);
    }

    public ICollaborator Create(ICollaboratorVisitor visitor)
    {
        return new Collaborator(visitor.Id);
    }
}
