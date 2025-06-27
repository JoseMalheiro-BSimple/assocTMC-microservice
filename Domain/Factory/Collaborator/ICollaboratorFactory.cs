using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;
public interface ICollaboratorFactory
{
    ICollaborator Create(Guid id);
    ICollaborator Create(ICollaboratorVisitor visitor);
}
