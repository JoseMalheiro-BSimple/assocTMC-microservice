using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;

public class CreateWithNoValidationTests
{
    [Fact]
    public async Task WhenAssociationDoesNotExist_ThenCreatesIt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 10));

        var assocMock = new Mock<IAssociationTrainingModuleCollaborator>();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var publisherMock = new Mock<IMessagePublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);
        factoryMock.Setup(f => f.Create(id, trainingModuleId, collaboratorId, period)).Returns(assocMock.Object);
        repoMock.Setup(r => r.AddAsync(assocMock.Object)).ReturnsAsync(assocMock.Object);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        // Act
        await service.CreateWithNoValidations(id, trainingModuleId, collaboratorId, period);

        // Assert
        factoryMock.Verify(f => f.Create(id, trainingModuleId, collaboratorId, period), Times.Once);
        repoMock.Verify(r => r.AddAsync(assocMock.Object), Times.Once);
    }

    [Fact]
    public async Task WhenAssociationAlreadyExists_ThenDoesNotCreateNewOne()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 10));

        var existingAssoc = new Mock<IAssociationTrainingModuleCollaborator>();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var publisherMock = new Mock<IMessagePublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAssoc.Object);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        // Act
        await service.CreateWithNoValidations(id, trainingModuleId, collaboratorId, period);

        // Assert
        factoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
        repoMock.Verify(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Never);
    }

    [Fact]
    public async Task WhenAddAsyncReturnsNull_ThenThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 10));

        var assocMock = new Mock<IAssociationTrainingModuleCollaborator>();

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var publisherMock = new Mock<IMessagePublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);
        factoryMock.Setup(f => f.Create(id, trainingModuleId, collaboratorId, period)).Returns(assocMock.Object);
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            service.CreateWithNoValidations(id, trainingModuleId, collaboratorId, period)
        );

        Assert.Equal("An error occured!", ex.Message);
    }
}
