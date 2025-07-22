using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;
public interface IAssociationTrainingModuleCollaboratorsRepository : IGenericRepositoryEF<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaborator, IAssociationTrainingModuleCollaboratorVisitor>
{
    Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId);
    IAssociationTrainingModuleCollaborator AddWithoutSavingAsync(IAssociationTrainingModuleCollaborator entity);
    Task RemoveWithoutSavingAsync(IAssociationTrainingModuleCollaborator entity);
}
