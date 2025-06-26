using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class GetByIdTests : RepositoryTestBase
{
    [Fact]
    public void WhenSearchingById_ThenReturnsCollaboratorIfExists()
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
        var result = repo.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result!.Id);
    }

    [Fact]
    public void WhenSearchingById_AndNotFound_ThenReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repo = new CollaboratorRepositoryEF(context, _mapper);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.Null(result);
    }
}

