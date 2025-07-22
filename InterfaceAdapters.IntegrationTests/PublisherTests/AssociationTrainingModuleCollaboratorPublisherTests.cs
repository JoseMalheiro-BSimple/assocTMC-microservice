using Domain.Messages;
using Domain.ValueObjects;
using InterfaceAdapters.Publishers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.PublisherTests;

public class AssociationTrainingModuleCollaboratorPublisherTests
{
    [Fact]
    public async Task WhenCalledWithValidParameters_CallPublishWithCorrectCreatedMessage()
    {
        // Arrange
        var mockEndpoint = new Mock<IPublishEndpoint>();
        var publisher = new AssociationTrainingModuleCollaboratorPublisher(mockEndpoint.Object);

        var orderId = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var periodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));

        // Act
        await publisher.PublishAssociationTrainingModuleCollaboratorCreatedMessage(orderId, trainingModuleId, collabId, periodDate);

        // Assert
        mockEndpoint.Verify(p => p.Publish(
            It.Is<AssociationTrainingModuleCollaboratorCreatedMessage>(a => a.Id == orderId && a.TrainingModuleId == trainingModuleId && a.CollaboratorId == collabId && a.PeriodDate == periodDate),
            It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Fact]
    public async Task WhenCalledWithValidParameters_CallPublishWithCorrectRemovedMessage()
    {
        // Arrange
        var mockEndpoint = new Mock<IPublishEndpoint>();
        var publisher = new AssociationTrainingModuleCollaboratorPublisher(mockEndpoint.Object);

        var orderId = Guid.NewGuid();
        
        // Act
        await publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(orderId);

        // Assert
        mockEndpoint.Verify(p => p.Publish(
            It.Is<AssociationTrainingModuleCollaboratorRemovedMessage>(a => a.Id == orderId),
            It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}
