using Application.DTO;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/associationsTMC")]
[ApiController]
public class AssociationTrainingModuleCollaboratorController : ControllerBase
{
    private readonly IAssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorController(IAssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    [HttpPost]
    public async Task<ActionResult<AssociationTrainingModuleCollaboratorDTO>> Create([FromBody] AssociationTrainingModuleCollaboratorCreationValuesDTO assocDTO)
    {
        var assocCreated = await _associationTrainingModuleCollaboratorService.Create(new CreateAssociationTrainingModuleCollaboratorDTO(assocDTO.CollaboratorId, assocDTO.TrainingModuleId, assocDTO.PeriodDate));

        return assocCreated.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var dto = new RemoveAssociationTrainingModuleCollaboratorDTO(id);
        var result = await _associationTrainingModuleCollaboratorService.Remove(dto);

        return result.ToActionResult();
    }
}

