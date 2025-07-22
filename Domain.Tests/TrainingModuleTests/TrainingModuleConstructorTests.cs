using Domain.Factory;
using Domain.ValueObjects;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.TrainingModuleTests;

public class TrainingModuleConstructorTests
{
    [Fact]
    public void WhenPassingValidData_ThenInstatiateFactory()
    {
        // Arran´ge

        // Act
        var factory = new TrainingModuleFactory();

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public void WhenPassingValidGUID_ThenCreateTrainingModule()
    {
        // Arrange
        var tmFactory = new TrainingModuleFactory();

        // Act
        var tm = tmFactory.Create(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>());

        // Assert
        Assert.NotNull(tm);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateTrainingModule()
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
