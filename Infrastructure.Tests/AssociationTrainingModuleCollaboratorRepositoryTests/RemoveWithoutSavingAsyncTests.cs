using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;

public class RemoveWithoutSavingAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task RemoveWithoutSavingAsync_MapsAndRemovesEntityFromContextChangeTracker()
    {
        // Arrange
        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        var existingId = Guid.NewGuid();
        var existingTrainingModuleId = Guid.NewGuid();
        var existingCollaboratorId = Guid.NewGuid();
        var existingPeriod = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(20)));

        var dataModelInDb = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = existingId,
            TrainingModuleId = existingTrainingModuleId,
            CollaboratorId = existingCollaboratorId,
            PeriodDate = existingPeriod,
        };

        context.AssociationTrainingModuleCollaborators.Add(dataModelInDb);
        await context.SaveChangesAsync(); 

        var domainEntityToRemove = new AssociationTrainingModuleCollaborator(
            existingId, existingTrainingModuleId, existingCollaboratorId, existingPeriod
        );

        _mapper.Setup(m => m.Map<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>(
            domainEntityToRemove))
            .Returns(dataModelInDb); 

        // Act
        await repo.RemoveWithoutSavingAsync(domainEntityToRemove);

        // Assert
        
        var entry = context.Entry(dataModelInDb);
        Assert.Equal(EntityState.Deleted, entry.State);
    }
}
