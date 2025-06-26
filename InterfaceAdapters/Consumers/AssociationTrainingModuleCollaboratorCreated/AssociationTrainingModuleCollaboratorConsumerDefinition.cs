using MassTransit;

namespace InterfaceAdapters.Consumers.Definition;

public class AssociationTrainingModuleCollaboratorConsumerDefinition : ConsumerDefinition<AssociationTrainingModuleCollaboratorCreatedConsumer>
{
    public AssociationTrainingModuleCollaboratorConsumerDefinition()
    {
        var random = new Random();
        var instanceId = random.Next(1000, 9999); 
        EndpointName = $"cmd-associationTrainingModuleCollaborator-created-events-{instanceId}";
    }
}
