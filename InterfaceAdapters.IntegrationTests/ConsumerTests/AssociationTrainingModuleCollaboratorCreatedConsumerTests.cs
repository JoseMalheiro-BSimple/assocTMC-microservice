using Application.DTO;
using Application.IServices;
using Domain.Messages;
using Domain.ValueObjects;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;
public class AssociationTrainingModuleCollaboratorCreatedConsumerTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_InstantiateConsumer()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorService> _assocService = new Mock<IAssociationTrainingModuleCollaboratorService>();

        // Act
        var consumer = new AssociationTrainingModuleCollaboratorCreatedConsumer(_assocService.Object);

        // Assert
        Assert.NotNull(consumer);
    }

    [Fact]
    public async Task Consume_WhenValidMessage_CallsCreateWithNoValidations()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();

        var consumer = new AssociationTrainingModuleCollaboratorCreatedConsumer(mockService.Object);

        var message = new AssociationTrainingModuleCollaboratorCreatedMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)))
        );

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorCreatedMessage>>();
        contextMock.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        mockService.Verify(s => s.CreateWithNoValidations(
            new CreateConsumedAssociationTrainingModuleCollaboratorDTO(
                message.Id,
                message.TrainingModuleId,
                message.CollaboratorId,
                message.PeriodDate
        )), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();
        var consumer = new AssociationTrainingModuleCollaboratorCreatedConsumer(mockService.Object);

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorCreatedMessage>>();
        contextMock.Setup(c => c.Message).Returns((AssociationTrainingModuleCollaboratorCreatedMessage)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(contextMock.Object));
    }
}
