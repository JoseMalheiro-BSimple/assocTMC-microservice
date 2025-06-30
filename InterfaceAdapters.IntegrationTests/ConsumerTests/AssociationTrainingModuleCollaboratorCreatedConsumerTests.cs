using Application.Services;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;
public class AssociationTrainingModuleCollaboratorCreatedConsumerTests
{
    [Fact]
    public async Task Consume_WhenValidMessage_CallsCreateWithNoValidations()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();

        var consumer = new AssociationTrainingModuleCollaboratorCreatedConsumer(mockService.Object);

        var message = new AssociationTrainingModuleCollaboratorCreated(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)))
        );

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorCreated>>();
        contextMock.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        mockService.Verify(s => s.CreateWithNoValidations(
            message.id,
            message.trainingModuleId,
            message.collaboratorId,
            message.periodDate
        ), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();
        var consumer = new AssociationTrainingModuleCollaboratorCreatedConsumer(mockService.Object);

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorCreated>>();
        contextMock.Setup(c => c.Message).Returns((AssociationTrainingModuleCollaboratorCreated)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(contextMock.Object));
    }
}
