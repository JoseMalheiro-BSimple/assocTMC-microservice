using Infrastructure.DataModel;
using Infrastructure.Repositories;

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

        var repo = new TrainingModuleRepositoryEF(context, _mapper);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
    }


    [Fact]
    public async Task WhenSearchingByIdAsync_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repo = new TrainingModuleRepositoryEF(context, _mapper);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

}
