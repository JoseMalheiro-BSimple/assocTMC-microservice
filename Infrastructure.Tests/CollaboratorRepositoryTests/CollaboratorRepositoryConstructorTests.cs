using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryConstructorTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstatiateRepository()
    {
        // Arrange
        Mock<IMapper> _mapper = new Mock<IMapper>();
        var options = new DbContextOptions<AssocTMCContext>();
        var context = new AssocTMCContext(options);

        // Act
        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Assert 
        Assert.NotNull(repo);
    }
}
