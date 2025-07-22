using Application.IServices;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers.CollaboratorCreated;

public class CollaboratorUpdatedConsumer : IConsumer<CollaboratorUpdatedMessage>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorUpdatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorUpdatedMessage> context)
    {
        var msg = context.Message;
        await _collaboratorService.EditCollaborator(new CollaboratorDTO(msg.Id, msg.PeriodDateTime));
    }
}
