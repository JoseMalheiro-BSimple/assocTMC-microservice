using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;
public class GetBydIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingByIdAsync_ThenReturnsTrainingModuleIfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tmDM = new TrainingModuleDataModel { Id = id };

        context.TrainingModules.Add(tmDM);
        await context.SaveChangesAsync();

        var expected = new Mock<ITrainingModule>();

        expected.Setup(tm => tm.Id).Returns(id);

        _mapper.Setup(m => m.Map<TrainingModuleDataModel, ITrainingModule>(It.Is<TrainingModuleDataModel>(tm => tm.Id == id)))
               .Returns(expected.Object);

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Object.Id, result!.Id);
    }


    [Fact]
    public async Task WhenSearchingByIdAsync_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repo = new TrainingModuleRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

}
