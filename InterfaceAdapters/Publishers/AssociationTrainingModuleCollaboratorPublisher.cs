using Application.Publishers;
using Domain.Messages;
using Domain.ValueObjects;
using MassTransit;

namespace InterfaceAdapters.Publishers;
public class AssociationTrainingModuleCollaboratorPublisher : IAssociationTrainingModuleCollaboratorPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public AssociationTrainingModuleCollaboratorPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAssociationTrainingModuleCollaboratorCreatedMessage(Guid Id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        await _publishEndpoint.Publish(new AssociationTrainingModuleCollaboratorCreatedMessage(Id, trainingModuleId, collaboratorId, periodDate));
    }

    public async Task PublishAssociationTrainingModuleCollaboratorRemovedMessage(Guid Id)
    {
        await _publishEndpoint.Publish(new AssociationTrainingModuleCollaboratorRemovedMessage(Id));
    }
}
