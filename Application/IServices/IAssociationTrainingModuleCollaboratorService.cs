using Application.DTO;

namespace Application.IServices;
public interface IAssociationTrainingModuleCollaboratorService
{
    public Task<Result<AssociationTrainingModuleCollaboratorDTO>> Create(CreateAssociationTrainingModuleCollaboratorDTO assocDTO);
    public Task CreateWithNoValidations(CreateConsumedAssociationTrainingModuleCollaboratorDTO assocDTO);
    public Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO);
}
