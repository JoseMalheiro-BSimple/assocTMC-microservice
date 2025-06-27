using AutoMapper;
using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>();
        CreateMap<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>()
            .ConvertUsing<AssociationTrainingModuleCollaboratorDataModelConverter>();
        CreateMap<ICollaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, ICollaborator>()
            .ConvertUsing<CollaboratorDataModelConverter>();
        CreateMap<ITrainingModule, TrainingModuleDataModel>();
        CreateMap<TrainingModuleDataModel, ITrainingModule>()
            .ConvertUsing<TrainingModuleDataModelConverter>();
    }
}