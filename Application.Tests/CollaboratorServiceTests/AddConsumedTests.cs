using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;
using Application.Services;
using Application.DTO;

namespace Application.Tests.CollaboratorServiceTests;

public class AddConsumedTests
{
    [Fact]
    public async Task AddConsumed_WhenAddReturnsCollaborator_DoesNotThrow()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var period = new PeriodDateTime();
        var mockCollaborator = new Mock<ICollaborator>();

        var factoryMock = new Mock<ICollaboratorFactory>();
        factoryMock
            .Setup(f => f.Create(testId, period))
            .Returns(mockCollaborator.Object);

        var repositoryMock = new Mock<ICollaboratorRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockCollaborator.Object))
            .ReturnsAsync(mockCollaborator.Object);

        var service = new CollaboratorService(repositoryMock.Object, factoryMock.Object);

        var dto = new CreateCollaboratorDTO(testId, period);

        // Act
        await service.AddConsumed(dto);

        //  Assert
        factoryMock.Verify(f => f.Create(testId, period), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockCollaborator.Object), Times.Once);
    }

    [Fact]
    public async Task AddConsumed_WhenAddReturnsNull_ThrowsException()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var period = new PeriodDateTime();
        var mockCollaborator = new Mock<ICollaborator>();

        var factoryMock = new Mock<ICollaboratorFactory>();
        factoryMock
            .Setup(f => f.Create(testId, period))
            .Returns(mockCollaborator.Object);

        // It returns null
        var repositoryMock = new Mock<ICollaboratorRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockCollaborator.Object))
            .ReturnsAsync((ICollaborator?)null!);

        var service = new CollaboratorService(repositoryMock.Object, factoryMock.Object);

        var dto = new CreateCollaboratorDTO(testId, period);

        // Act 
        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddConsumed(dto));
        Assert.Equal("An error as occured!", exception.Message);

        // Assert
        factoryMock.Verify(f => f.Create(testId, period), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockCollaborator.Object), Times.Once);
    }
}
