using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface IAssociationTrainingModuleCollaboratorFactory
{
    Task<IAssociationTrainingModuleCollaborator> Create(Guid trainingModuleId, Guid collaboratorId, DateOnly initDate, DateOnly endDate);
    IAssociationTrainingModuleCollaborator Create(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
    IAssociationTrainingModuleCollaborator Create(IAssociationTrainingModuleCollaboratorVisitor visitor);
}
