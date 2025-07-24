using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class UpdateAsyncTests: RepositoryTestBase
{

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ThrowsArgumentNullException()
    {
        // Arrange

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            // Act
            repo.UpdateAsync(null!)
        );
    }

    [Fact]
    public async Task UpdateAsync_WhenTrainingModuleNotFound_ThrowsArgumentException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateEntity = new TrainingModule(nonExistentId, new List<PeriodDateTime>());

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            repo.UpdateAsync(updateEntity)
        );
        Assert.Contains($"TrainingModule with ID {nonExistentId} not found for update.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenTrainingModuleExistsAndPeriodsAreUpdated_SavesChangesCorrectly()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var initialPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31)),
            new PeriodDateTime(new DateTime(2023, 2, 2), new DateTime(2023, 2, 28))
        };

        var newPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2024, 3, 3), new DateTime(2024, 3, 15)),
            new PeriodDateTime(new DateTime(2024, 4, 4), new DateTime(2024, 4, 30)),
            new PeriodDateTime(new DateTime(2024, 5, 5), new DateTime(2024, 5, 10))
        };

        var existingDm = new TrainingModuleDataModel
        {
            Id = trainingModuleId,
            Periods = initialPeriods
        };

        await context.TrainingModules.AddAsync(existingDm);
        await context.SaveChangesAsync();

        // New training module entity
        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tm.Setup(t => t.Id).Returns(trainingModuleId);
        tm.Setup(t => t.Periods).Returns(newPeriods);

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.IsAny<TrainingModuleDataModel>()))
               .Returns(tm.Object);

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateAsync(tm.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingModuleId, result.Id);
        Assert.Equal(newPeriods.Count, result.Periods.Count);
        Assert.Equal(newPeriods[0]._initDate, result.Periods[0]._initDate);
        Assert.Equal(newPeriods[0]._finalDate, result.Periods[0]._finalDate);
        Assert.Equal(newPeriods[1]._initDate, result.Periods[1]._initDate);
        Assert.Equal(newPeriods[1]._finalDate, result.Periods[1]._finalDate);
        Assert.Equal(newPeriods[2]._initDate, result.Periods[2]._initDate);
        Assert.Equal(newPeriods[2]._finalDate, result.Periods[2]._finalDate);


        // 2. Verify the state in the in-memory database
        var updatedDmInDb = await context.TrainingModules
                                          .Include(dm => dm.Periods)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(dm => dm.Id == trainingModuleId);

        Assert.NotNull(updatedDmInDb);
        Assert.Equal(newPeriods.Count, updatedDmInDb.Periods.Count);
        
        // Verify periods in DB match the new periods
        foreach (var newPeriod in newPeriods)
        {
            Assert.Contains(updatedDmInDb.Periods, p =>
                p._initDate == newPeriod._initDate && p._finalDate== newPeriod._finalDate);
        }
    }

    [Fact]
    public async Task UpdateAsync_WhenPeriodsAreSetToEmptyList_RemovesAllPeriods()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var initialPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31))
        };

        // Seed the in-memory database
        var existingDm = new TrainingModuleDataModel
        {
            Id = trainingModuleId,
            Periods = initialPeriods
        };
        await context.TrainingModules.AddAsync(existingDm);
        await context.SaveChangesAsync();

        // New training module entity
        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tm.Setup(t => t.Id).Returns(trainingModuleId);
        tm.Setup(t => t.Periods).Returns(new List<PeriodDateTime>());

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.Is<TrainingModuleDataModel>(tm => tm.Id == trainingModuleId)))
               .Returns(tm.Object);

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateAsync(tm.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingModuleId, result.Id);
        Assert.Empty(result.Periods);

        var updatedDmInDb = await context.TrainingModules
                                          .Include(dm => dm.Periods)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(dm => dm.Id == trainingModuleId);

        Assert.NotNull(updatedDmInDb);
        Assert.Empty(updatedDmInDb.Periods);
    }

    [Fact]
    public async Task UpdateAsync_WhenPeriodsAreSetToNull_RemovesAllPeriods()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var initialPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2023, 1, 1), new DateTime(2023, 1, 31))
        };

        // Seed the in-memory database
        var existingDm = new TrainingModuleDataModel
        {
            Id = trainingModuleId,
            Periods = initialPeriods
        };
        await context.TrainingModules.AddAsync(existingDm);
        await context.SaveChangesAsync();
        
        // New training module entity
        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tm.Setup(t => t.Id).Returns(trainingModuleId);
        tm.Setup(t => t.Periods).Returns((List<PeriodDateTime>)null!);

        // Expected 
        Mock<ITrainingModule> tmExpected = new Mock<ITrainingModule>();
        tmExpected.Setup(t => t.Id).Returns(trainingModuleId);
        tmExpected.Setup(t => t.Periods).Returns(new List<PeriodDateTime>());

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.Is<TrainingModuleDataModel>(tm => tm.Id == trainingModuleId)))
               .Returns(tmExpected.Object);

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateAsync(tm.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingModuleId, result.Id);
        Assert.Empty(result.Periods); 

        var updatedDmInDb = await context.TrainingModules
                                          .Include(dm => dm.Periods)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(dm => dm.Id == trainingModuleId);

        Assert.NotNull(updatedDmInDb);
        Assert.Empty(updatedDmInDb.Periods);
    }
}
