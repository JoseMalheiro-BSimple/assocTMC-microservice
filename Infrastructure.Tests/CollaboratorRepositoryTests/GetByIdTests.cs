using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

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

        var expected = new Mock<ICollaborator>();

        expected.Setup(c => c.Id).Returns(id);

        _mapper.Setup(m => m.Map<CollaboratorDataModel, ICollaborator>(It.Is<CollaboratorDataModel>(c => c.Id == id)))
               .Returns(expected.Object);

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

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

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.Null(result);
    }
}

