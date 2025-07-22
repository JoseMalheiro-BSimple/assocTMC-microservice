using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Infrastructure.DataModel;
public class CollaboratorDataModel : ICollaboratorVisitor
{
    public Guid Id { get; set; }
    public PeriodDateTime Period { get; set; } = new PeriodDateTime();

    public CollaboratorDataModel() { }

    public CollaboratorDataModel(ICollaborator collaborator)
    {
        Id = collaborator.Id;
        Period = collaborator.Period;
    }
}
