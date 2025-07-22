using Domain.ValueObjects;

namespace Domain.Visitor;
public interface ICollaboratorVisitor
{
    Guid Id { get; }
    PeriodDateTime Period { get; }
}
