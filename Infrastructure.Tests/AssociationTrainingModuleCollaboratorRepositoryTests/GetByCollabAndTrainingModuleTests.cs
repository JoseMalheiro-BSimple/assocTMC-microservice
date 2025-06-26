using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByCollabAndTrainingModuleTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenMatchingAssociationsExist_ThenReturnsAll()
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
        var result = await repo.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId);

        // Assert
        Assert.Single(result.ToList());
        Assert.Contains(result, r => r.Id == assocDM.Id);
    }

    [Fact]
    public async Task WhenNoMatchingAssociationsExist_ThenReturnsEmpty()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper);

        // Act
        var result = await repo.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId);

        // Assert
        Assert.Empty(result);
    }
}
