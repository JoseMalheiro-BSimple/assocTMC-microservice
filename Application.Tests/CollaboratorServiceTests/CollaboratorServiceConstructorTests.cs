using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class CollaboratorServiceConstructorTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstantiateService()
    {
        // Arrange
        Mock<ICollaboratorRepository> _collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ICollaboratorFactory> _collabFactory = new Mock<ICollaboratorFactory>();

        // Act
        var service = new CollaboratorService(_collabRepo.Object, _collabFactory.Object);

        // Assert
        Assert.NotNull(service);
    }
}
