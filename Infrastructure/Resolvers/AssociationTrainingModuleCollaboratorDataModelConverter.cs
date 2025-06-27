using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class AssociationTrainingModuleCollaboratorDataModelConverter : ITypeConverter<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>
{
    private readonly IAssociationTrainingModuleCollaboratorFactory _factory;

    public AssociationTrainingModuleCollaboratorDataModelConverter(IAssociationTrainingModuleCollaboratorFactory factory)
    {
        _factory = factory;
    }

    public IAssociationTrainingModuleCollaborator Convert(AssociationTrainingModuleCollaboratorDataModel source, IAssociationTrainingModuleCollaborator destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}
