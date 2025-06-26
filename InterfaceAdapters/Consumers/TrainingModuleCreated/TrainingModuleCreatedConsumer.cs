using Application.Services;
using Domain.Messaging;
using MassTransit;

namespace InterfaceAdapters.Consumers;
public class TrainingModuleCreatedConsumer : IConsumer<TrainingModuleCreated>
{
    private readonly ITrainingModuleService _trainingModuleService;

    public TrainingModuleCreatedConsumer(ITrainingModuleService trainingModuleService)
    {
        _trainingModuleService = trainingModuleService;
    }

    public async Task Consume(ConsumeContext<TrainingModuleCreated> context)
    {
        var msg = context.Message;
        await _trainingModuleService.SubmitAsync(msg.id);
    }
}
