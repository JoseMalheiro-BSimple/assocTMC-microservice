using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class SubmitAsyncTests
{
    [Fact]
    public async Task SubmitAsync_WhenAddReturnsCollaborator_DoesNotThrow()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var mockCollaborator = new Mock<ICollaborator>();

        var factoryMock = new Mock<ICollaboratorFactory>();
        factoryMock
            .Setup(f => f.Create(testId))
            .Returns(mockCollaborator.Object);

        var repositoryMock = new Mock<ICollaboratorRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockCollaborator.Object))
            .ReturnsAsync(mockCollaborator.Object);

        var service = new CollaboratorService(repositoryMock.Object, factoryMock.Object);

        // Act
        await service.SubmitAsync(testId);

        //  Assert
        factoryMock.Verify(f => f.Create(testId), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockCollaborator.Object), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WhenAddReturnsNull_ThrowsException()
    {
        // Arrange
        var testId = Guid.NewGuid();
        var mockCollaborator = new Mock<ICollaborator>();

        var factoryMock = new Mock<ICollaboratorFactory>();
        factoryMock
            .Setup(f => f.Create(testId))
            .Returns(mockCollaborator.Object);

        // It returns null
        var repositoryMock = new Mock<ICollaboratorRepository>();
        repositoryMock
            .Setup(r => r.AddAsync(mockCollaborator.Object))
            .ReturnsAsync((ICollaborator?)null!);

        var service = new CollaboratorService(repositoryMock.Object, factoryMock.Object);

        // Act 
        var exception = await Assert.ThrowsAsync<Exception>(() => service.SubmitAsync(testId));
        Assert.Equal("An error as occured!", exception.Message);

        // Assert
        factoryMock.Verify(f => f.Create(testId), Times.Once);
        repositoryMock.Verify(r => r.AddAsync(mockCollaborator.Object), Times.Once);
    }
}
