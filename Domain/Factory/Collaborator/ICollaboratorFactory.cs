using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;
public interface ICollaboratorFactory
{
    ICollaborator Create(Guid id, PeriodDateTime Period);
    ICollaborator Create(ICollaboratorVisitor visitor);
}
