using Application.DTO;
using Application.IServices;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers.TrainingModuleCreated;

public class TrainingModuleUpdatedConsumer : IConsumer<TrainingModuleUpdatedMessage>
{
    private readonly ITrainingModuleService _trainingModuleService;

    public TrainingModuleUpdatedConsumer(ITrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }

    public async Task Consume(ConsumeContext<TrainingModuleUpdatedMessage> context)
    {
        var msg = context.Message;
        await _trainingModuleService.SubmitUpdateAsync(new UpdateConsumedTrainingModuleDTO(msg.Id, msg.Periods));
    }
}
