using MassTransit;

namespace InterfaceAdapters.Consumers;

public class CollaboratorConsumerDefinition : ConsumerDefinition<CollaboratorCreatedConsumer>
{
    public CollaboratorConsumerDefinition()
    {
        var random = new Random();
        var instanceId = random.Next(1000, 9999); 
        EndpointName = $"cmd-collaborator-created-events-{instanceId}";
    }
}
