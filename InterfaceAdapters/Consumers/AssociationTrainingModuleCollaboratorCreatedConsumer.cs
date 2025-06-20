using Application.Services;
using Domain.Messaging;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class AssociationTrainingModuleCollaboratorCreatedConsumer : IConsumer<AssociationTrainingModuleCollaboratorCreated>
{
    private readonly AssociationTrainingModuleCollaboratorService _assocService;

    public AssociationTrainingModuleCollaboratorCreatedConsumer(AssociationTrainingModuleCollaboratorService assocService)
    {
        _assocService = assocService;
    }

    public async Task Consume(ConsumeContext<AssociationTrainingModuleCollaboratorCreated> context)
    {
        var msg = context.Message;
        await _assocService.CreateWithNoValidations(msg.id, msg.trainingModuleId, msg.collaboratorId, msg.PeriodDate);
    }
}
