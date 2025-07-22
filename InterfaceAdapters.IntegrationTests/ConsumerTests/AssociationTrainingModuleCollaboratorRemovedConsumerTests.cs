using Application.DTO;
using Application.IServices;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using InterfaceAdapters.Consumers.AssociationTrainingModuleCollaboratorCreated;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class AssociationTrainingModuleCollaboratorRemovedConsumerTests
{
    [Fact]
    public void WhenPassingCorrectDependencies_InstantiateConsumer()
    {
        // Arrange
        Mock<IAssociationTrainingModuleCollaboratorService> _assocService = new Mock<IAssociationTrainingModuleCollaboratorService>();

        // Act
        var consumer = new AssociationTrainingModuleCollaboratorRemovedConsumer(_assocService.Object);

        // Assert
        Assert.NotNull(consumer);
    }

    [Fact]
    public async Task Consume_WhenValidMessage_CallsRemove()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();

        var consumer = new AssociationTrainingModuleCollaboratorRemovedConsumer(mockService.Object);

        var message = new AssociationTrainingModuleCollaboratorRemovedMessage(
            Guid.NewGuid()
        );

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorRemovedMessage>>();
        contextMock.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        mockService.Verify(s => s.Remove(
            new RemoveAssociationTrainingModuleCollaboratorDTO(
                message.Id
        )), Times.Once);
    }

    [Fact]
    public async Task Consume_WhenMessageIsNull_ThrowsException()
    {
        // Arrange
        var mockService = new Mock<IAssociationTrainingModuleCollaboratorService>();
        var consumer = new AssociationTrainingModuleCollaboratorRemovedConsumer(mockService.Object);

        var contextMock = new Mock<ConsumeContext<AssociationTrainingModuleCollaboratorRemovedMessage>>();
        contextMock.Setup(c => c.Message).Returns((AssociationTrainingModuleCollaboratorRemovedMessage)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => consumer.Consume(contextMock.Object));
    }
}
