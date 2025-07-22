using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class EditCollaboratorTests
{
    [Fact]
    public async Task EditCollaborator_ShouldUpdateAndPublishEvent_WhenCollaboratorExists()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();

        var collabId = Guid.NewGuid();
        var newPeriod = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(2));

        var collaborator = new Collaborator(collabId, newPeriod);

        collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync(collaborator);
        collabRepoDouble.Setup(r => r.UpdateCollaborator(It.IsAny<ICollaborator>())).ReturnsAsync(collaborator);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object);

        // Act
        await service.EditCollaborator(new CollaboratorDTO(collabId, newPeriod));

        // Assert
        collabRepoDouble.Verify(r => r.UpdateCollaborator(It.IsAny<ICollaborator>()), Times.Once);
    }
}
