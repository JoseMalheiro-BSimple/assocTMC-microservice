using Application.DTO;
using Domain.Models;

namespace InterfaceAdapters.IntegrationTests.Helpers;

public static class AssociationTrainingModuleCollaboratorHelper
{
    public static CreateAssociationTrainingModuleCollaboratorDTO GenerateCreateAssociationTrainingModuleCollaboratorDTO(Guid collabId, Guid trainingModuleId, PeriodDate periodDate)
    {
        return new CreateAssociationTrainingModuleCollaboratorDTO
        {
            CollaboratorId = collabId,
            TrainingModuleId = trainingModuleId,
            PeriodDate = periodDate
        };
    }
}