using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;
public class GetByIdTests : RepositoryTestBase
{
    [Fact]
    public void WhenSearchingById_ThenReturnsTrainingModuleIfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tmDM = new TrainingModuleDataModel { Id = id };

        context.TrainingModules.Add(tmDM);

        var id1 = Guid.NewGuid();
        var tmDM1 = new TrainingModuleDataModel { Id = id1 };

        context.TrainingModules.Add(tmDM1);
        context.SaveChanges();

        var expected = new Mock<ITrainingModule>();

        expected.Setup(tm => tm.Id).Returns(id);

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.Is<TrainingModuleDataModel>(tm => tm.Id == id)))
               .Returns(expected.Object);

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Object.Id, result!.Id);
    }

    [Fact]
    public void WhenSearchingById_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.Null(result);
    }
}
