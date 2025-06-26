using Application.DTO;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.AssociationTrainingModuleCollaboratorServiceTests;
public class CreateTests
{
    [Fact]
    public async Task Create_WhenValidInput_ReturnsSuccessResult()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IMessagePublisher> _publisherMock = new Mock<IMessagePublisher>();

        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        {
            TrainingModuleId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        };

        var mockEntity = new Mock<IAssociationTrainingModuleCollaborator>();
        mockEntity.SetupGet(e => e.Id).Returns(Guid.NewGuid());
        mockEntity.SetupGet(e => e.TrainingModuleId).Returns(dto.TrainingModuleId);
        mockEntity.SetupGet(e => e.CollaboratorId).Returns(dto.CollaboratorId);
        mockEntity.SetupGet(e => e.PeriodDate).Returns(dto.PeriodDate);

        _factoryMock
            .Setup(f => f.Create(dto.TrainingModuleId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate))
            .ReturnsAsync(mockEntity.Object);

        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<IAssociationTrainingModuleCollaborator>()))
            .ReturnsAsync(mockEntity.Object);

        var _service = new AssociationTrainingModuleCollaboratorService(_repoMock.Object, _factoryMock.Object, _publisherMock.Object);

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dto.TrainingModuleId, result.Value.TrainingModuleId);
        Assert.Equal(dto.CollaboratorId, result.Value.CollaboratorId);
        Assert.Equal(dto.PeriodDate, result.Value.PeriodDate);

        _publisherMock.Verify(p => p.PublishOrderSubmittedAsync(
            mockEntity.Object.Id,
            dto.TrainingModuleId,
            dto.CollaboratorId,
            dto.PeriodDate), Times.Once);
    }

    [Fact]
    public async Task Create_WhenFactoryThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IMessagePublisher> _publisherMock = new Mock<IMessagePublisher>();

        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        {
            TrainingModuleId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        };

        _factoryMock
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ThrowsAsync(new ArgumentException("Invalid input"));
        
        var _service = new AssociationTrainingModuleCollaboratorService(_repoMock.Object, _factoryMock.Object, _publisherMock.Object);

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid input", result.Error?.Message);
        _publisherMock.Verify(p => p.PublishOrderSubmittedAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenFactoryThrowsUnexpectedException_ReturnsFailure()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorsRepository> _repoMock = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();
        Mock<IAssociationTrainingModuleCollaboratorFactory> _factoryMock = new Mock<IAssociationTrainingModuleCollaboratorFactory>();
        Mock<IMessagePublisher> _publisherMock = new Mock<IMessagePublisher>();

        var dto = new CreateAssociationTrainingModuleCollaboratorDTO
        {
            TrainingModuleId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
        };

        _factoryMock
            .Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
            .ThrowsAsync(new Exception("Database error"));

        var _service = new AssociationTrainingModuleCollaboratorService(_repoMock.Object, _factoryMock.Object, _publisherMock.Object);

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Database error", result.Error?.Message);
    }
}
