using Domain.Factory;
using Domain.ValueObjects;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.CollaboratorTests;
public class CollaboratorFactoryTests
{
    [Fact]
    public void WhenPassingValidData_TheInstatiateFactory()
    {
        // Arrange

        // Act
        var factory = new CollaboratorFactory();

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void WhenPassingValidId_ThenCreateCollaborator()
    {
        // Arrange
        var collaboratorFactory = new CollaboratorFactory();

        // Act
        var collab = collaboratorFactory.Create(It.IsAny<Guid>(), It.IsAny<PeriodDateTime>());

        // Assert
        Assert.NotNull(collab);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateCollaborator()
    {
        // Arrange
        Mock<ICollaboratorVisitor> visitor = new Mock<ICollaboratorVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.Period).Returns(It.IsAny<PeriodDateTime>());

        var collaboratorFactory = new CollaboratorFactory();

        // Act
        var collab = collaboratorFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(collab);
    }
}
