using Application.Publishers;
using Domain.ValueObjects;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class FakeAssociationTrainingModuleCollaboratorPublisher : IAssociationTrainingModuleCollaboratorPublisher
{
    public Task PublishAssociationTrainingModuleCollaboratorCreatedMessage(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        return Task.CompletedTask;
    }

    public Task PublishAssociationTrainingModuleCollaboratorRemovedMessage(Guid id)
    {
        return Task.CompletedTask;
    }
}
