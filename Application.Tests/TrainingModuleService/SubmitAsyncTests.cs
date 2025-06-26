using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.TrainingModuleServiceTests;
public class SubmitAsyncTests
{
    [Fact]
    public async Task SubmitAsync_WhenAddReturnsCollaborator_DoesNotThrow()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var mockTrainingModule = new Mock<ITrainingModule>();

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock
            .Setup(f => f.Create(testId))
            .Returns(mockTrainingModule.Object);

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockTrainingModule.Object))
            .ReturnsAsync(mockTrainingModule.Object);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object);

        // Act
        await service.SubmitAsync(testId);

        // Assert
        factoryMock.Verify(f => f.Create(testId), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WhenAddReturnsNull_ThrowsException()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var mockTrainingModule = new Mock<ITrainingModule>();

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock
            .Setup(f => f.Create(testId))
            .Returns(mockTrainingModule.Object);

        // It returns null
        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockTrainingModule.Object))
            .ReturnsAsync((ITrainingModule?)null!);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => service.SubmitAsync(testId));
        Assert.Equal("An error as occured!", exception.Message);

        // Assert
        factoryMock.Verify(f => f.Create(testId), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
    }
}
