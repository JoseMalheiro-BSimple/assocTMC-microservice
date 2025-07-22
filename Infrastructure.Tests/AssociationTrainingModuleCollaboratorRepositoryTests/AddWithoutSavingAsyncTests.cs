
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;

public class AddWithoutSavingAsyncTests : RepositoryTestBase
{
    [Fact]
    public void AddWithoutSavingAsync_MapsAndAddsEntityToContextChangeTracker()
    {
        // Arrange

        var domainEntityToAdd = new AssociationTrainingModuleCollaborator(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(10)))
        );

        var expectedDataModel = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = domainEntityToAdd.Id,
            TrainingModuleId = domainEntityToAdd.TrainingModuleId,
            CollaboratorId = domainEntityToAdd.CollaboratorId,
            PeriodDate = domainEntityToAdd.PeriodDate
        };

        var expectedReturnedDomainEntity = new Mock<IAssociationTrainingModuleCollaborator>();
        expectedReturnedDomainEntity.Setup(x => x.Id).Returns(domainEntityToAdd.Id);
        expectedReturnedDomainEntity.Setup(x => x.TrainingModuleId).Returns(domainEntityToAdd.TrainingModuleId);
        expectedReturnedDomainEntity.Setup(x => x.CollaboratorId).Returns(domainEntityToAdd.CollaboratorId);
        expectedReturnedDomainEntity.Setup(x => x.PeriodDate).Returns(domainEntityToAdd.PeriodDate);

        _mapper.Setup(m => m.Map<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>(domainEntityToAdd))
            .Returns(expectedDataModel);

        _mapper.Setup(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(expectedDataModel))
               .Returns(expectedReturnedDomainEntity.Object);

        var repo = new AssociationTrainingModuleCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.AddWithoutSavingAsync(domainEntityToAdd);

        // Assert
        _mapper.Verify(m => m.Map<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>(domainEntityToAdd), Times.Once);
        _mapper.Verify(m => m.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(expectedDataModel), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(expectedReturnedDomainEntity.Object.Id, result.Id);
        Assert.Equal(expectedReturnedDomainEntity.Object.TrainingModuleId, result.TrainingModuleId);
        Assert.Equal(expectedReturnedDomainEntity.Object.CollaboratorId, result.CollaboratorId);
        Assert.Equal(expectedReturnedDomainEntity.Object.PeriodDate, result.PeriodDate);
    }

}
