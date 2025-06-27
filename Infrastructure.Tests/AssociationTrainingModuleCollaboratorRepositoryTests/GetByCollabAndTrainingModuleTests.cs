using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;
public class GetByCollabAndTrainingModuleTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingByCollabAndModuleId_ReturnsMatchingAssociations()
    {
        // Arrange
        var matchingTrainingModuleId = Guid.NewGuid();
        var matchingCollaboratorId = Guid.NewGuid();

        var nonMatchingTrainingModuleId = Guid.NewGuid();
        var nonMatchingCollaboratorId = Guid.NewGuid();

        var assoc1 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = matchingCollaboratorId,
            TrainingModuleId = matchingTrainingModuleId,
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
        };

        var assoc2 = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = matchingCollaboratorId,
            TrainingModuleId = matchingTrainingModuleId,
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.Today.AddDays(1)), DateOnly.FromDateTime(DateTime.Today.AddMonths(6)))
        };

        var unrelatedAssoc = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = nonMatchingCollaboratorId,
            TrainingModuleId = nonMatchingTrainingModuleId,
            PeriodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(2)))
        };

        context.AssociationTrainingModuleCollaborators.AddRange(assoc1, assoc2, unrelatedAssoc);
        await context.SaveChangesAsync();

        // Setup mapper behavior
        var expectedDomain1 = new Mock<IAssociationTrainingModuleCollaborator>();
        expectedDomain1.SetupGet(x => x.Id).Returns(assoc1.Id);

        var expectedDomain2 = new Mock<IAssociationTrainingModuleCollaborator>();
        expectedDomain2.SetupGet(x => x.Id).Returns(assoc2.Id);

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.Is<AssociationTrainingModuleCollaboratorDataModel>(dm => dm.Id == assoc1.Id)))
            .Returns(expectedDomain1.Object);

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(
            It.Is<AssociationTrainingModuleCollaboratorDataModel>(dm => dm.Id == assoc2.Id)))
            .Returns(expectedDomain2.Object);

        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = (await repo.GetByCollabAndTrainingModule(matchingCollaboratorId, matchingTrainingModuleId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == assoc1.Id);
        Assert.Contains(result, r => r.Id == assoc2.Id);
        Assert.DoesNotContain(result, r => r.Id == unrelatedAssoc.Id);
    }

    [Fact]
    public async Task WhenNoMatchingAssociationsExist_ThenReturnsEmpty()
    {
        // Arrange
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId);

        // Assert
        Assert.Empty(result);
    }
}
