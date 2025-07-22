using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Moq;

namespace Infrastructure.Tests.TrainingModuleRepositoryTests;

public class TrainingModuleResolverTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstatiateResolver()
    {
        // Arrange
        Mock<ITrainingModuleFactory> factory = new Mock<ITrainingModuleFactory>();

        // Act
        var result = new TrainingModuleDataModelConverter(factory.Object);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Convert_GivenValidSourceDataModel_CallsFactoryCreateOnceAndReturnsFactoryResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tmId = Guid.NewGuid();
        var cID = Guid.NewGuid();
        var periods = new List<PeriodDateTime>();

        var sourceDataModel = new TrainingModuleDataModel
        {
            Id = id,
            Periods = periods
        };

        var expectedDomain = new Mock<ITrainingModule>();
        expectedDomain.Setup(a => a.Id).Returns(id);
        expectedDomain.Setup(a => a.Periods).Returns(periods);

        var mockFactory = new Mock<ITrainingModuleFactory>();

        mockFactory.Setup(f => f.Create(It.Is<ITrainingModuleVisitor>(s => s == sourceDataModel)))
                   .Returns(expectedDomain.Object);

        ITrainingModule destination = null!;
        ResolutionContext context = null!;

        var converter = new TrainingModuleDataModelConverter(mockFactory.Object);

        // Act
        var actualResult = converter.Convert(sourceDataModel, destination, context);

        // Assert
        mockFactory.Verify(f => f.Create(It.Is<ITrainingModuleVisitor>(s => s == sourceDataModel)), Times.Once);
        Assert.Equal(expectedDomain.Object, actualResult);
    }
}
