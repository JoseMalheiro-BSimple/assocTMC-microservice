using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;

public class AssociationTrainingModuleCollaboratorConstructorTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstantiateService()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _assocTMCRepository = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _assocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IAssociationTrainingModuleCollaboratorPublisher> _publisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        // Act
        var service = new AssociationTrainingModuleCollaboratorService(_assocTMCRepository.Object, _assocTMCFactory.Object, _publisher.Object);

        // Assert
        Assert.NotNull(service);
    }
}
