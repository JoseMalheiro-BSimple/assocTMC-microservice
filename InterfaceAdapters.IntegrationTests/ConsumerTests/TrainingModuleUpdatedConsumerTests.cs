using Application.DTO;
using Application.IServices;
using Domain.Messages;
using Domain.ValueObjects;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Consumers.TrainingModuleCreated;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class TrainingModuleUpdatedConsumerTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_InstantiateConsumer()
    {
        // Arrange
        Mock<ITrainingModuleService> _collabService = new Mock<ITrainingModuleService>();

        // Act
        var consumer = new TrainingModuleUpdatedConsumer(_collabService.Object);

        // Assert
        Assert.NotNull(consumer);
    }

    [Fact]
    public async Task Consume_WhenCalled_CallsSubmitAsyncOnTrainingModuleService()
    {
        // Arrange
        var mockService = new Mock<ITrainingModuleService>();
        var consumer = new TrainingModuleUpdatedConsumer(mockService.Object);

        var message = new TrainingModuleUpdatedMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<PeriodDateTime> { new PeriodDateTime() }
        );

        var mockContext = new Mock<ConsumeContext<TrainingModuleUpdatedMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(s => s.SubmitUpdateAsync(new UpdateConsumedTrainingModuleDTO(message.Id, message.Periods)), Times.Once);
    }
}
