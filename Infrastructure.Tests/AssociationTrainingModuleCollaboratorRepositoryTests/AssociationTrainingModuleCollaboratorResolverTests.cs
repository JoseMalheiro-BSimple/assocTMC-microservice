using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;
using Moq;

namespace Infrastructure.Tests.AssociationTrainingModuleCollaboratorRepositoryTests;

public class AssociationTrainingModuleCollaboratorResolverTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_ThenInstatiateResolver()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorFactory> factory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        // Act
        var result = new AssociationTrainingModuleCollaboratorDataModelConverter(factory.Object);

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
        var period = new PeriodDate();

        var sourceDataModel = new AssociationTrainingModuleCollaboratorDataModel
        {
            Id = id,
            TrainingModuleId = tmId,
            CollaboratorId = cID,
            PeriodDate = period 
        };

        var expectedDomain = new Mock<IAssociationTrainingModuleCollaborator>();
        expectedDomain.Setup(a => a.Id).Returns(id);
        expectedDomain.Setup(a => a.TrainingModuleId).Returns(tmId);
        expectedDomain.Setup(a => a.CollaboratorId).Returns(cID);
        expectedDomain.Setup(a => a.PeriodDate).Returns(period);

        var mockFactory = new Mock<IAssociationTrainingModuleCollaboratorFactory>();

        mockFactory.Setup(f => f.Create(It.Is<IAssociationTrainingModuleCollaboratorVisitor>(s => s == sourceDataModel)))
                   .Returns(expectedDomain.Object);

        IAssociationTrainingModuleCollaborator destination = null!; 
        ResolutionContext context = null!; 

        var converter = new AssociationTrainingModuleCollaboratorDataModelConverter(mockFactory.Object);

        // Act
        var actualResult = converter.Convert(sourceDataModel, destination, context);

        // Assert
        mockFactory.Verify(f => f.Create(It.Is<IAssociationTrainingModuleCollaboratorVisitor>(s => s == sourceDataModel)), Times.Once);
        Assert.Equal(expectedDomain.Object, actualResult);
    }
}
