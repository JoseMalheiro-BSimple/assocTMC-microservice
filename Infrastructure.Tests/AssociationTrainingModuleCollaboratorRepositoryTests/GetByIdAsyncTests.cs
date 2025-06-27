using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingById_ThenReturnsAssociationIfExists()
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
        await context.SaveChangesAsync();

        // Expected domain model
        var expected = new Mock<IAssociationTrainingModuleCollaborator>();
        expected.SetupGet(x => x.Id).Returns(id);
        expected.SetupGet(x => x.TrainingModuleId).Returns(trainingModuleId);
        expected.SetupGet(x => x.CollaboratorId).Returns(collaboratorId);
        expected.SetupGet(x => x.PeriodDate).Returns(period);

        // Mapper returns mocked domain model for the matching EF entity
        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.Is<AssociationTrainingModuleCollaboratorDataModel>(dm => dm.Id == id)))
            .Returns(expected.Object);

        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Object.Id, result!.Id);
        Assert.Equal(expected.Object.TrainingModuleId, result.TrainingModuleId);
        Assert.Equal(expected.Object.CollaboratorId, result.CollaboratorId);
        Assert.Equal(expected.Object.PeriodDate, result.PeriodDate);
    }


    [Fact]
    public async Task WhenSearchingById_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }
}
