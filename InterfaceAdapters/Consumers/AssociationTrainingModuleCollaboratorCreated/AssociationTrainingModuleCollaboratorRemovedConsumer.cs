using Application.DTO;
using Application.IServices;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers.AssociationTrainingModuleCollaboratorCreated;

public class AssociationTrainingModuleCollaboratorRemovedConsumer : IConsumer<AssociationTrainingModuleCollaboratorRemovedMessage>
{
    private readonly IAssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorRemovedConsumer(IAssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    public async Task Consume(ConsumeContext<AssociationTrainingModuleCollaboratorRemovedMessage> context)
    {
        var msg = context.Message;
        await _associationTrainingModuleCollaboratorService.Remove(new RemoveAssociationTrainingModuleCollaboratorDTO(msg.Id));
    }
}
