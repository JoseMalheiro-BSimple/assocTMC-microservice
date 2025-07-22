using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorResolverTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstatiateResolver()
    {
        // Arrange
        Mock<ICollaboratorFactory> factory = new Mock<ICollaboratorFactory>();

        // Act
        var result = new CollaboratorDataModelConverter(factory.Object);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Convert_GivenValidSourceDataModel_CallsFactoryCreateOnceAndReturnsFactoryResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDateTime();

        var sourceDataModel = new CollaboratorDataModel
        {
            Id = id,
            Period = period
        };

        var expectedDomain = new Mock<ICollaborator>();
        expectedDomain.Setup(a => a.Id).Returns(id);

        var mockFactory = new Mock<ICollaboratorFactory>();

        mockFactory.Setup(f => f.Create(It.Is<ICollaboratorVisitor>(s => s == sourceDataModel)))
                   .Returns(expectedDomain.Object);

        ICollaborator destination = null!;
        ResolutionContext context = null!;

        var converter = new CollaboratorDataModelConverter(mockFactory.Object);

        // Act
        var actualResult = converter.Convert(sourceDataModel, destination, context);

        // Assert
        mockFactory.Verify(f => f.Create(It.Is<ICollaboratorVisitor>(s => s == sourceDataModel)), Times.Once);
        Assert.Equal(expectedDomain.Object, actualResult);
    }
}
