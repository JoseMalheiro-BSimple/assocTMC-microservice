using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.TrainingModuleServiceTests;

public class AddConsumedTests
{
    [Fact]
    public async Task AddConsumed_WhenAddReturnsCollaborator_DoesNotThrow()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();
        var mockTrainingModule = new Mock<ITrainingModule>();

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock
            .Setup(f => f.Create(testId, periods))
            .Returns(mockTrainingModule.Object);

        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockTrainingModule.Object))
            .ReturnsAsync(mockTrainingModule.Object);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object);

        var dto = new CreateTrainingModuleDTO(testId, periods);

        // Act
        await service.AddConsumed(dto);

        // Assert
        factoryMock.Verify(f => f.Create(testId, periods), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
    }

    [Fact]
    public async Task AddConsumed_WhenAddReturnsNull_ThrowsException()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();
        var mockTrainingModule = new Mock<ITrainingModule>();

        var factoryMock = new Mock<ITrainingModuleFactory>();
        factoryMock
            .Setup(f => f.Create(testId, periods))
            .Returns(mockTrainingModule.Object);

        // It returns null
        var repositoryMock = new Mock<ITrainingModuleRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockTrainingModule.Object))
            .ReturnsAsync((ITrainingModule?)null!);

        var service = new TrainingModuleService(repositoryMock.Object, factoryMock.Object);

        var dto = new CreateTrainingModuleDTO(testId, periods);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddConsumed(dto));
        Assert.Equal("An error as occured!", exception.Message);

        // Assert
        factoryMock.Verify(f => f.Create(testId, periods), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockTrainingModule.Object), Times.Once);
    }
}
