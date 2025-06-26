using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByIdTests : RepositoryTestBase
{
    [Fact]
    public void WhenSearchingById_ThenReturnsAssociationIfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(6)));

        var assocDM = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = id,
            TrainingModuleId = trainingModuleId,
            CollaboratorId = collaboratorId,
            PeriodDate = period
        };

        context.AssociationTrainingModuleCollaborators.Add(assocDM);

        var id2 = Guid.NewGuid();
        var trainingModuleId2 = Guid.NewGuid();
        var collaboratorId2 = Guid.NewGuid();
        var period2 = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(6)));

        var assocDM2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = id2,
            TrainingModuleId = trainingModuleId2,
            CollaboratorId = collaboratorId2,
            PeriodDate = period2
        };

        context.AssociationTrainingModuleCollaborators.Add(assocDM2);
        context.SaveChanges();
        
        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
        Assert.Equal(trainingModuleId, result.TrainingModuleId);
        Assert.Equal(collaboratorId, result.CollaboratorId);
        Assert.Equal(period, result.PeriodDate);
    }


    [Fact]
    public void WhenSearchingById_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.Null(result);
    }
}
