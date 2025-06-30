using Application.Services;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class AssociationTrainingModuleCollaboratorCreatedConsumer : IConsumer<AssociationTrainingModuleCollaboratorCreated>
{
    private readonly IAssociationTrainingModuleCollaboratorService _assocService;

    public AssociationTrainingModuleCollaboratorCreatedConsumer(IAssociationTrainingModuleCollaboratorService assocService)
    {
        _assocService = assocService;
    }

    public async Task Consume(ConsumeContext<AssociationTrainingModuleCollaboratorCreated> context)
    {
        var msg = context.Message;
        await _assocService.CreateWithNoValidations(msg.id, msg.trainingModuleId, msg.collaboratorId, msg.periodDate);
    }
}
