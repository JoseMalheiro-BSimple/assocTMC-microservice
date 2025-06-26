using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class GetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingByIdAsync_ThenReturnsCollaboratorIfExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cDM = new CollaboratorDataModel { Id = id };

        context.Collaborators.Add(cDM);

        var id2 = Guid.NewGuid();
        var cDM2 = new CollaboratorDataModel { Id = id2 };

        context.Collaborators.Add(cDM2);
        context.SaveChanges();

        var repo = new CollaboratorRepositoryEF(context, _mapper);

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

        var repo = new CollaboratorRepositoryEF(context, _mapper);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }
}
