using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Application.Tests.TrainingModuleServiceTests;

public class TrainingModuleSereviceConstructorTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstantiateService()
    {
        // Arrange
        Mock<ITrainingModuleRepository> _tmRepository = new Mock<ITrainingModuleRepository>();
        Mock<ITrainingModuleFactory> _tmFactory = new Mock<ITrainingModuleFactory>();

        // Act
        var service = new TrainingModuleService(_tmRepository.Object, _tmFactory.Object);

        // Assert
        Assert.NotNull(service);
    }
}
