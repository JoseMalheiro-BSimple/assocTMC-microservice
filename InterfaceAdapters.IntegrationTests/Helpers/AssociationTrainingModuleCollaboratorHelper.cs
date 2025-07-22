using Application.DTO;
using Domain.ValueObjects;

namespace InterfaceAdapters.IntegrationTests.Helpers;

public static class AssociationTrainingModuleCollaboratorHelper
{
    public static CreateAssociationTrainingModuleCollaboratorDTO GenerateCreateAssociationTrainingModuleCollaboratorDTO(Guid collabId, Guid trainingModuleId, PeriodDate periodDate)
    {
        return new CreateAssociationTrainingModuleCollaboratorDTO
        (
            collabId,
            trainingModuleId,
            periodDate
        );
    }
}