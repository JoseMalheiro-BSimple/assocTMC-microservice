using Domain.Factory;
using Domain.Models;
using Domain.ValueObjects;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.TrainingModuleTests;

public class TrainingModuleFactoryTests
{
    [Fact]
    public void WhenPassingValidGUID_ThenReturnTrainingModule()
    {
        // Act
        var result = new TrainingModuleFactory();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void WhenPassingValidId_ThenCreateCollaborator()
    {
        // Arrange
        var tmFactory = new TrainingModuleFactory();

        // Act
        var tm = tmFactory.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>());

        // Assert
        Assert.NotNull(tm);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateCollaborator()
    {
        // Arrange
        Mock<ITrainingModuleVisitor> visitor = new Mock<ITrainingModuleVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.Periods).Returns(It.IsAny<List<PeriodDateTime>>());

        var tmFactory = new TrainingModuleFactory();

        // Act
        var tm = tmFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(tm);
    }
}
