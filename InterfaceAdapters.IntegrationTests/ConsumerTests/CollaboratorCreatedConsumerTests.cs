using Application.Services;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class CollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnCollaboratorService()
    {
        // Arrange
        var mockService = new Mock<ICollaboratorService>();
        var consumer = new CollaboratorCreatedConsumer(mockService.Object);

        var message = new CollaboratorCreated( 
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDateTime()
        );

        var mockContext = new Mock<ConsumeContext<CollaboratorCreated>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(message.collabId), Times.Once);
    }
}
