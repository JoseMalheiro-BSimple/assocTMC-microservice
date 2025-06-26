using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly IMapper _mapper;
    protected readonly AssocTMCContext context;
    protected readonly Mock<IAssociationTrainingModuleCollaboratorFactory> _assocFactoryMock;
    protected readonly Mock<ICollaboratorFactory> _collabFactoryMock;
    protected readonly Mock<ITrainingModuleFactory> _trainingModuleFactoryMock;

    protected RepositoryTestBase()
    {
        // Create mock factory
        _assocFactoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        _collabFactoryMock = new Mock<ICollaboratorFactory>();
        _trainingModuleFactoryMock = new Mock<ITrainingModuleFactory>();

        // Setup default factory behavior for tests
        _assocFactoryMock
            .Setup(f => f.Create(It.IsAny<AssociationTrainingModuleCollaboratorDataModel>()))
            .Returns<AssociationTrainingModuleCollaboratorDataModel>(dm => new AssociationTrainingModuleCollaborator(dm.Id, dm.TrainingModuleId, dm.CollaboratorId, dm.PeriodDate));

        _collabFactoryMock
             .Setup(f => f.Create(It.IsAny<CollaboratorDataModel>()))
             .Returns<CollaboratorDataModel>(dm => new Collaborator(dm.Id));

        _trainingModuleFactoryMock
            .Setup(f => f.Create(It.IsAny<TrainingModuleDataModel>()))
            .Returns<TrainingModuleDataModel>(dm => new TrainingModule(dm.Id));

        // Configure AutoMapper with service constructor support
        var config = new MapperConfiguration(cfg =>
        {
            // Let AutoMapper resolve constructors using this factory method
            cfg.ConstructServicesUsing(type =>
            {
                if (type == typeof(AssociationTrainingModuleCollaboratorDataModelConverter))
                    return new AssociationTrainingModuleCollaboratorDataModelConverter(_assocFactoryMock.Object);

                if(type == typeof(CollaboratorDataModelConverter))
                    return new CollaboratorDataModelConverter(_collabFactoryMock.Object);

                if (type == typeof(TrainingModuleDataModelConverter))
                    return new TrainingModuleDataModelConverter(_trainingModuleFactoryMock.Object);

                return Activator.CreateInstance(type)!;
            });

            cfg.AddProfile<DataModelMappingProfile>();
        });

        _mapper = config.CreateMapper();

        // Configure in-memory EF Core context
        var options = new DbContextOptionsBuilder<AssocTMCContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // isolate DB per test
            .Options;

        context = new AssocTMCContext(options);
    }
}
