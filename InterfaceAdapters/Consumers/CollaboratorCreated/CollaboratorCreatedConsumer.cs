using Application.Services;
using Domain.Messaging;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class CollaboratorCreatedConsumer : IConsumer<CollaboratorCreated>
{
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorCreatedConsumer(ICollaboratorService collaboratorService)
    {
        _collaboratorService = collaboratorService;
    }

    public async Task Consume(ConsumeContext<CollaboratorCreated> context)
    {
        var msg = context.Message;
        await _collaboratorService.SubmitAsync(msg.collabId);
    }
}
