using Domain.ValueObjects;

namespace Application.Publishers;
public interface IAssociationTrainingModuleCollaboratorPublisher
{
    Task PublishAssociationTrainingModuleCollaboratorCreatedMessage(Guid Id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    Task PublishAssociationTrainingModuleCollaboratorRemovedMessage(Guid Id);
}
