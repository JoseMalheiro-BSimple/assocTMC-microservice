using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;
public class CollaboratorDataModelConverter : ITypeConverter<CollaboratorDataModel, ICollaborator>
{
    private readonly ICollaboratorFactory _factory;

    public CollaboratorDataModelConverter(ICollaboratorFactory factory)
    {
        _factory = factory;
    }

    public ICollaborator Convert(CollaboratorDataModel source, ICollaborator destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}

