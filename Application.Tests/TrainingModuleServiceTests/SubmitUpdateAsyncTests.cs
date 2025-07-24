using Application.DTO;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.TrainingModuleServiceTests;

public class SubmitUpdateAsyncTests
{
    [Fact]
    public async Task SubmitUpdateAsync_WhenTrainingModuleExistsAndPeriodsAreValid_CallsUpdateAsync()
    {
        // Arrange
        var _tmRepositoryMock = new Mock<ITrainingModuleRepository>();
        var _tmFactoryMock = new Mock<ITrainingModuleFactory>();

        var trainingModuleId = Guid.NewGuid();
        var initialPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-5))
        };
        var updatePeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10)),
            new PeriodDateTime(DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(20))
        };

        var existingTrainingModule = new TrainingModule(trainingModuleId, initialPeriods);
        var updateDto = new UpdateConsumedTrainingModuleDTO(trainingModuleId, updatePeriods);

        _tmRepositoryMock.Setup(r => r.GetByIdAsync(trainingModuleId))
                         .ReturnsAsync(existingTrainingModule);

        ITrainingModule capturedUpdatedTrainingModule = null!;
        _tmRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ITrainingModule>()))
                         .Callback<ITrainingModule>(tm => capturedUpdatedTrainingModule = tm)
                         .ReturnsAsync(new TrainingModule(trainingModuleId, updatePeriods)); // Return a new instance representing the updated state

        var _service = new TrainingModuleService(_tmRepositoryMock.Object, _tmFactoryMock.Object);

        // Act
        await _service.SubmitUpdateAsync(updateDto);

        // Assert
        _tmRepositoryMock.Verify(r => r.GetByIdAsync(trainingModuleId), Times.Once);
        _tmRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ITrainingModule>()), Times.Once);

        Assert.NotNull(capturedUpdatedTrainingModule);
        Assert.Equal(trainingModuleId, capturedUpdatedTrainingModule.Id);
        Assert.Equal(updatePeriods.Count, capturedUpdatedTrainingModule.Periods.Count);
        Assert.Equal(updatePeriods[0]._initDate, capturedUpdatedTrainingModule.Periods[0]._initDate);
        Assert.Equal(updatePeriods[1]._finalDate, capturedUpdatedTrainingModule.Periods[1]._finalDate);

        _tmFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
    }

    [Fact]
    public async Task SubmitUpdateAsync_WhenTrainingModuleNotFound_ThrowsArgumentException()
    {
        // Arrange
        var _tmRepositoryMock = new Mock<ITrainingModuleRepository>();
        var _tmFactoryMock = new Mock<ITrainingModuleFactory>();

        var trainingModuleId = Guid.NewGuid();
        var updateDto = new UpdateConsumedTrainingModuleDTO(trainingModuleId, new List<PeriodDateTime>());

        // Mock GetByIdAsync to return null (not found)
        _tmRepositoryMock.Setup(r => r.GetByIdAsync(trainingModuleId))
                         .ReturnsAsync((ITrainingModule)null!);

        var _service = new TrainingModuleService(_tmRepositoryMock.Object, _tmFactoryMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.SubmitUpdateAsync(updateDto));

        Assert.Equal("TrainingModule not found.", exception.Message);
        _tmRepositoryMock.Verify(r => r.GetByIdAsync(trainingModuleId), Times.Once);
        _tmRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ITrainingModule>()), Times.Never); 
        _tmFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
    }

    [Fact]
    public async Task SubmitUpdateAsync_WhenGetByIdAsyncThrowsException_ExceptionIsPropagated()
    {
        // Arrange
        var _tmRepositoryMock = new Mock<ITrainingModuleRepository>();
        var _tmFactoryMock = new Mock<ITrainingModuleFactory>();

        var trainingModuleId = Guid.NewGuid();
        var updateDto = new UpdateConsumedTrainingModuleDTO(trainingModuleId, new List<PeriodDateTime>());
        var expectedException = new InvalidOperationException("Database connection lost during fetch.");

        // Mock GetByIdAsync to throw an exception
        _tmRepositoryMock.Setup(r => r.GetByIdAsync(trainingModuleId))
                         .ThrowsAsync(expectedException);

        var _service = new TrainingModuleService(_tmRepositoryMock.Object, _tmFactoryMock.Object);

        // Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            // Act
            _service.SubmitUpdateAsync(updateDto)
        );

        Assert.Equal(expectedException.Message, actualException.Message);
        _tmRepositoryMock.Verify(r => r.GetByIdAsync(trainingModuleId), Times.Once);
        _tmRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ITrainingModule>()), Times.Never);
        _tmFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
    }

    [Fact]
    public async Task SubmitUpdateAsync_WhenUpdateAsyncThrowsException_ExceptionIsPropagated()
    {
        // Arrange
        var _tmRepositoryMock = new Mock<ITrainingModuleRepository>();
        var _tmFactoryMock = new Mock<ITrainingModuleFactory>();

        var trainingModuleId = Guid.NewGuid();
        var initialPeriods = new List<PeriodDateTime> { new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)) };
        var updatePeriods = new List<PeriodDateTime> { new PeriodDateTime(DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(10)) };

        var existingTrainingModule = new TrainingModule(trainingModuleId, initialPeriods);
        var updateDto = new UpdateConsumedTrainingModuleDTO(trainingModuleId, updatePeriods);
        var expectedException = new DbUpdateException("Concurrency conflict.", new Exception()); // Example EF exception

        _tmRepositoryMock.Setup(r => r.GetByIdAsync(trainingModuleId))
                         .ReturnsAsync(existingTrainingModule);

        _tmRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ITrainingModule>()))
                         .ThrowsAsync(expectedException);

        var _service = new TrainingModuleService(_tmRepositoryMock.Object, _tmFactoryMock.Object);

        // Assert
        var actualException = await Assert.ThrowsAsync<DbUpdateException>(() =>
            // Act
            _service.SubmitUpdateAsync(updateDto)
        );

        Assert.Equal(expectedException.Message, actualException.Message);
        _tmRepositoryMock.Verify(r => r.GetByIdAsync(trainingModuleId), Times.Once);
        _tmRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ITrainingModule>()), Times.Once);
        _tmFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>()), Times.Never);
    }
}
