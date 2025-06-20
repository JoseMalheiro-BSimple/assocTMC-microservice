using Application.Publishers;
using Domain.Messaging;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Publishers;
public class MassTransitPublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishOrderSubmittedAsync(Guid Id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        await _publishEndpoint.Publish(new AssociationTrainingModuleCollaboratorCreated(Id, trainingModuleId, collaboratorId, periodDate));
    }
}
