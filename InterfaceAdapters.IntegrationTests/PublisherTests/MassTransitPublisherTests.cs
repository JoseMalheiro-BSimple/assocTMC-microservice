using Domain.Messaging;
using Domain.Models;
using InterfaceAdapters.Publishers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.PublisherTests;

public class MassTransitPublisherTests
{
    [Fact]
    public async Task Should_publish_order_submitted_event()
    {
        // Arrange
        var mockEndpoint = new Mock<IPublishEndpoint>();
        var publisher = new MassTransitPublisher(mockEndpoint.Object);

        var orderId = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var periodDate = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)));

        // Act
        await publisher.PublishOrderSubmittedAsync(orderId, trainingModuleId, collabId, periodDate);

        // Assert
        mockEndpoint.Verify(p => p.Publish(
            It.Is<AssociationTrainingModuleCollaboratorCreated>(a => a.id == orderId && a.trainingModuleId == trainingModuleId && a.collaboratorId == collabId && a.periodDate == periodDate),
            It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}
