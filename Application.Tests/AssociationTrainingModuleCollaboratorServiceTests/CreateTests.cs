using Application.DTO;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class CreateTests
{
    [Fact]
    public async Task Create_WhenValidInput_ReturnsSuccessResultAndPublishesMessage()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IAssociationTrainingModuleCollaboratorPublisher> _publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();
        
        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        );

        var generatedId = Guid.NewGuid(); 
        var mockEntity = new Mock<IAssociationTrainingModuleCollaborator>();
        mockEntity.Setup(e => e.Id).Returns(generatedId);
        mockEntity.Setup(e => e.TrainingModuleId).Returns(dto.TrainingModuleId);
        mockEntity.Setup(e => e.CollaboratorId).Returns(dto.CollaboratorId);
        mockEntity.Setup(e => e.PeriodDate).Returns(dto.PeriodDate);

        _factoryMock
            .Setup(f => f.Create(dto.TrainingModuleId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate))
            .ReturnsAsync(mockEntity.Object);


        _repoMock.Setup(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
            .ReturnsAsync(mockEntity.Object);

        var _service = new AssociationTrainingModuleCollaboratorService(
            _repoMock.Object,
            _factoryMock.Object,
            _publisherMock.Object
        );

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(generatedId, result.Value.Id);
        Assert.Equal(dto.TrainingModuleId, result.Value.TrainingModuleId);
        Assert.Equal(dto.CollaboratorId, result.Value.CollaboratorId);
        Assert.Equal(dto.PeriodDate, result.Value.PeriodDate);

        _factoryMock.Verify(f => f.Create(dto.TrainingModuleId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.Is<IAssociationTrainingModuleCollaborator>(a => a == mockEntity.Object)), Times.Once);
        _publisherMock.Verify(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(
            generatedId,
            dto.TrainingModuleId,
            dto.CollaboratorId,
            It.Is<PeriodDate>(pd => pd.InitDate == dto.PeriodDate.InitDate && pd.FinalDate == dto.PeriodDate.FinalDate)),
            Times.Once);
    }

    [Fact]
    public async Task Create_WhenFactoryThrowsArgumentException_ReturnsBadRequestAndDoesNotPublish()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IAssociationTrainingModuleCollaboratorPublisher> _publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        );

        _factoryMock
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ThrowsAsync(new ArgumentException("Invalid input"));

        var _service = new AssociationTrainingModuleCollaboratorService(
            _repoMock.Object,
            _factoryMock.Object,
            _publisherMock.Object
        );

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.BadRequest("Invalid input").StatusCode, result.Error?.StatusCode);
        Assert.Equal("Invalid input", result.Error?.Message);

        _factoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        _repoMock.Verify(r => r.AddWithoutSavingAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Never);
        _publisherMock.Verify(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
        _repoMock.Verify(d => d.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Create_WhenPublisherThrowsException_ReturnsInternalServerErrorAndDoesNotSave()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IAssociationTrainingModuleCollaboratorPublisher> _publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        );

        var generatedId = Guid.NewGuid();
        var mockEntity = new Mock<IAssociationTrainingModuleCollaborator>();
        mockEntity.Setup(e => e.Id).Returns(generatedId);
        mockEntity.Setup(e => e.TrainingModuleId).Returns(dto.TrainingModuleId);
        mockEntity.Setup(e => e.CollaboratorId).Returns(dto.CollaboratorId);
        mockEntity.Setup(e => e.PeriodDate).Returns(dto.PeriodDate);

        _factoryMock
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(mockEntity.Object);

        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
            .ReturnsAsync(mockEntity.Object);

        var expectedPublisherErrorMessage = "Broker connection failed.";
        _publisherMock.Setup(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()))
            .ThrowsAsync(new Exception(expectedPublisherErrorMessage));

        var _service = new AssociationTrainingModuleCollaboratorService(
            _repoMock.Object,
            _factoryMock.Object,
            _publisherMock.Object
        );

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred during creation: {expectedPublisherErrorMessage}").StatusCode, result.Error?.StatusCode);

        _factoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Once);
        _publisherMock.Verify(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Once);
    }

    [Fact]
    public async Task Create_WhenRepositoryAddAsyncThrowsException_ReturnsInternalServerErrorAndDoesNotPublish()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IAssociationTrainingModuleCollaboratorPublisher> _publisherMock = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();
        
        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        );

        var mockEntity = new Mock<IAssociationTrainingModuleCollaborator>();
        _factoryMock
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(mockEntity.Object);

        // Setup AddWithoutSavingAsync to throw an exception
        var expectedRepoErrorMessage = "Database connection error during add.";
        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
            .Throws(new Exception(expectedRepoErrorMessage));

        var _service = new AssociationTrainingModuleCollaboratorService(
            _repoMock.Object,
            _factoryMock.Object,
            _publisherMock.Object
        );

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Error.InternalServerError($"An unexpected error occurred during creation: {expectedRepoErrorMessage}").StatusCode, result.Error?.StatusCode);
        Assert.Contains(expectedRepoErrorMessage, result.Error?.Message);

        _factoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()), Times.Once); 
        _publisherMock.Verify(p => p.PublishAssociationTrainingModuleCollaboratorCreatedMessage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
    }
}
