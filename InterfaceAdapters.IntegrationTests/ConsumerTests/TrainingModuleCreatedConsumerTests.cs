using Application.Services;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingModuleCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnTrainingModuleService()
    {
        // Arrange
        var mockService = new Mock<ITrainingModuleService>();
        var consumer = new TrainingModuleCreatedConsumer(mockService.Object);

        var message = new TrainingModuleCreated(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<PeriodDateTime> { new PeriodDateTime() }
        );

        var mockContext = new Mock<ConsumeContext<TrainingModuleCreated>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitAsync(message.id), Times.Once);
    }
}
