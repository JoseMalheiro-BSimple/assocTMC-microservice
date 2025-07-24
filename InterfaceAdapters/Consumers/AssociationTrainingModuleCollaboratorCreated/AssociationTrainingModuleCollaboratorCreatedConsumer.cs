using Application.DTO;
using Application.IServices;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class AssociationTrainingModuleCollaboratorCreatedConsumer : IConsumer<AssociationTrainingModuleCollaboratorCreatedMessage>
{
    private readonly IAssociationTrainingModuleCollaboratorService _assocService;

    public AssociationTrainingModuleCollaboratorCreatedConsumer(IAssociationTrainingModuleCollaboratorService assocService)
    {
        _assocService = assocService;
    }

    public async Task Consume(ConsumeContext<AssociationTrainingModuleCollaboratorCreatedMessage> context)
    {
        var msg = context.Message;
        await _assocService.CreateWithNoValidations(new CreateConsumedAssociationTrainingModuleCollaboratorDTO(msg.Id, msg.CollaboratorId, msg.TrainingModuleId, msg.PeriodDate));
    }
}
