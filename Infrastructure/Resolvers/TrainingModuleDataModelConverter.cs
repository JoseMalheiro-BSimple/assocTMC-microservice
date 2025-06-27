using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;
public class TrainingModuleDataModelConverter : ITypeConverter<TrainingModuleDataModel, ITrainingModule>
{
    private readonly ITrainingModuleFactory _factory;

    public TrainingModuleDataModelConverter(ITrainingModuleFactory factory)
    {
        _factory = factory;
    }

    public ITrainingModule Convert(TrainingModuleDataModel source, ITrainingModule destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}
