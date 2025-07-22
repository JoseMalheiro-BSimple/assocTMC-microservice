using Application.DTO;

namespace Application.IServices;
public interface IAssociationTrainingModuleCollaboratorService
{
    Task<Result<AssociationTrainingModuleCollaboratorDTO>> Create(CreateAssociationTrainingModuleCollaboratorDTO assocDTO);
    Task CreateWithNoValidations(CreateConsumedAssociationTrainingModuleCollaboratorDTO assocDTO);
    Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO);
}
