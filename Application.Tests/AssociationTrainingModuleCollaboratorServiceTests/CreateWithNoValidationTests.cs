using Application.DTO;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
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
        assocMock.Setup(a => a.Id).Returns(id); 
        assocMock.Setup(a => a.TrainingModuleId).Returns(trainingModuleId);
        assocMock.Setup(a => a.CollaboratorId).Returns(collaboratorId);
        assocMock.Setup(a => a.PeriodDate).Returns(period);

        var repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);
        factoryMock.Setup(f => f.Create(id, trainingModuleId, collaboratorId, period)).Returns(assocMock.Object);
        repoMock.Setup(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>())).ReturnsAsync(assocMock.Object);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        var createDTO = new CreateConsumedAssociationTrainingModuleCollaboratorDTO(id, collaboratorId, trainingModuleId, period);

        // Act
        await service.CreateWithNoValidations(createDTO); 

        // Assert
        repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        factoryMock.Verify(f => f.Create(id, trainingModuleId, collaboratorId, period), Times.Once);
        repoMock.Verify(r => r.AddAsync(It.Is<IAssociationTrainingModuleCollaborator>(a => a == assocMock.Object)), Times.Once);
        publisherMock.Verify(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
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
        var publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAssoc.Object);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        var createDTO = new CreateConsumedAssociationTrainingModuleCollaboratorDTO(id, trainingModuleId, collaboratorId, period);

        // Act
        await service.CreateWithNoValidations(createDTO);

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
        var publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);
        factoryMock.Setup(f => f.Create(id, trainingModuleId, collaboratorId, period)).Returns(assocMock.Object);
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((IAssociationTrainingModuleCollaborator?)null);

        var service = new AssociationTrainingModuleCollaboratorService(repoMock.Object, factoryMock.Object, publisherMock.Object);

        var createDTO = new CreateConsumedAssociationTrainingModuleCollaboratorDTO(id, trainingModuleId, collaboratorId, period);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            service.CreateWithNoValidations(createDTO)
        );

        Assert.Equal("An error occured!", ex.Message);
    }
}
