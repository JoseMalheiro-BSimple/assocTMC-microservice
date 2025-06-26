using MassTransit;

namespace InterfaceAdapters.Consumers;

public class TrainingModuleConsumerDefinition : ConsumerDefinition<TrainingModuleCreatedConsumer>
{
    public TrainingModuleConsumerDefinition()
    {
        var random = new Random();
        var instanceId = random.Next(1000, 9999);
        EndpointName = $"cmd-trainingModule-created-events-{instanceId}";
    }
}
