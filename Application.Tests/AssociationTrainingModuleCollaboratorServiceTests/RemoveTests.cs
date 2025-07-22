using Application.DTO;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;

public class RemoveTests
{
    [Fact]
    public async Task Remove_WhenAssociationExists_ReturnsSuccessResultAndPublishesMessage()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var mockAssociation = new Mock<IAssociationTrainingModuleCollaborator>();
        mockAssociation.SetupGet(a => a.Id).Returns(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>(); 
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        _mockAssocTMCRepo.Setup(d => d.SaveChangesAsync()).ReturnsAsync(1); 

        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ReturnsAsync(mockAssociation.Object);

        _mockAssocTMCRepo.Setup(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
                         .Returns(Task.CompletedTask);

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(mockAssociation.Object), Times.Once);
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Remove_WhenAssociationDoesNotExist_ReturnsNotFoundFailureResultAndDoesNotRemoveOrPublish()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ReturnsAsync((IAssociationTrainingModuleCollaborator)null!);

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.NotFound("AssociationTrainingModuleCollaborator not found.").Message, result.Error!.Message);
        Assert.Equal("AssociationTrainingModuleCollaborator not found.", result.Error.Message);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Never);
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(It.IsAny<Guid>()), Times.Never);
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Remove_WhenRepositoryGetByIdThrowsException_ReturnsInternalServerErrorFailureResult()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        var expectedExceptionMessage = "Database connection lost during GetById.";
        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ThrowsAsync(new Exception(expectedExceptionMessage));

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred: {expectedExceptionMessage}").Message, result.Error!.Message);
        Assert.Contains(expectedExceptionMessage, result.Error.Message);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once); 
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Never);
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(It.IsAny<Guid>()), Times.Never);
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Remove_WhenRepositoryRemoveWithoutSavingThrowsException_ReturnsInternalServerErrorFailureResultAndDoesNotPublishOrSave()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var mockAssociation = new Mock<IAssociationTrainingModuleCollaborator>();
        mockAssociation.SetupGet(a => a.Id).Returns(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ReturnsAsync(mockAssociation.Object);

        var expectedExceptionMessage = "Database error during removal staging.";
        _mockAssocTMCRepo.Setup(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
                         .ThrowsAsync(new Exception(expectedExceptionMessage));

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred: {expectedExceptionMessage}").Message, result.Error!.Message);
        Assert.Contains(expectedExceptionMessage, result.Error.Message);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(mockAssociation.Object), Times.Once); // Called and threw
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(It.IsAny<Guid>()), Times.Never);
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Remove_WhenPublisherThrowsException_ReturnsInternalServerErrorFailureResultAndDoesNotSave()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var mockAssociation = new Mock<IAssociationTrainingModuleCollaborator>();
        mockAssociation.SetupGet(a => a.Id).Returns(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ReturnsAsync(mockAssociation.Object);

        _mockAssocTMCRepo.Setup(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
                         .Returns(Task.CompletedTask);

        var expectedPublisherErrorMessage = "Message broker unavailable.";
        // FIX: Setup publisher to throw an exception
        _mockPublisher.Setup(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(It.IsAny<Guid>()))
                      .ThrowsAsync(new Exception(expectedPublisherErrorMessage));

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred: {expectedPublisherErrorMessage}").Message, result.Error!.Message);
        Assert.Contains(expectedPublisherErrorMessage, result.Error.Message);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(mockAssociation.Object), Times.Once);
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(associationIdToRemove), Times.Once); // Called and threw
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Never); // SaveChangesAsync should NOT be called
    }

    [Fact]
    public async Task Remove_WhenRepositorySaveChangesAsyncThrowsException_ReturnsInternalServerErrorFailureResult()
    {
        // Arrange
        var associationIdToRemove = Guid.NewGuid();
        var removeDto = new RemoveAssociationTrainingModuleCollaboratorDTO(associationIdToRemove);

        var mockAssociation = new Mock<IAssociationTrainingModuleCollaborator>();
        mockAssociation.SetupGet(a => a.Id).Returns(associationIdToRemove);

        var _mockAssocTMCRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        var _mockAssocTMCFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        var _mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        _mockAssocTMCRepo.Setup(repo => repo.GetByIdAsync(associationIdToRemove))
                         .ReturnsAsync(mockAssociation.Object);

        _mockAssocTMCRepo.Setup(repo => repo.RemoveWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
                         .Returns(Task.CompletedTask);

        _mockPublisher.Setup(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(It.IsAny<Guid>()))
                      .Returns(Task.CompletedTask);

        var expectedSaveErrorMessage = "Database concurrency error during save.";
        _mockAssocTMCRepo.Setup(d => d.SaveChangesAsync()).ThrowsAsync(new Exception(expectedSaveErrorMessage));

        var service = new AssociationTrainingModuleCollaboratorService(
            _mockAssocTMCRepo.Object,
            _mockAssocTMCFactory.Object,
            _mockPublisher.Object
        );

        // Act
        var result = await service.Remove(removeDto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred: {expectedSaveErrorMessage}").Message, result.Error!.Message);
        Assert.Contains(expectedSaveErrorMessage, result.Error.Message);

        _mockAssocTMCRepo.Verify(repo => repo.GetByIdAsync(associationIdToRemove), Times.Once);
        _mockAssocTMCRepo.Verify(repo => repo.RemoveWithoutSavingAsync(mockAssociation.Object), Times.Once);
        _mockPublisher.Verify(publisher => publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(associationIdToRemove), Times.Once); 
        _mockAssocTMCRepo.Verify(d => d.SaveChangesAsync(), Times.Once); 
    }
}
