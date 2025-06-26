using Application.DTO;
using Domain.Models;

namespace Application.Services;
public interface IAssociationTrainingModuleCollaboratorService
{
    public Task<Result<AssociationTrainingModuleCollaboratorDTO>> Create(CreateAssociationTrainingModuleCollaboratorDTO assocDTO);
    public Task CreateWithNoValidations(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate);
}
