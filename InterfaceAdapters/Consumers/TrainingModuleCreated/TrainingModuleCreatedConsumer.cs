using Application.DTO;
using Application.IServices;
using Domain.Messages;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class TrainingModuleCreatedConsumer : IConsumer<TrainingModuleCreatedMessage>
{
    private readonly ITrainingModuleService _trainingModuleService;

    public TrainingModuleCreatedConsumer(ITrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }

    public async Task Consume(ConsumeContext<TrainingModuleCreatedMessage> context)
    {
        var msg = context.Message;
        await _trainingModuleService.AddConsumed(new CreateTrainingModuleDTO(msg.Id, msg.Periods));
    }
}
