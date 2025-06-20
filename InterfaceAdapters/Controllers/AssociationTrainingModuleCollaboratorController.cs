using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/associationsTMC")]
[ApiController]
public class AssociationTrainingModuleCollaboratorController : ControllerBase
{
    private readonly AssociationTrainingModuleCollaboratorService _associationTrainingModuleCollaboratorService;

    public AssociationTrainingModuleCollaboratorController(AssociationTrainingModuleCollaboratorService associationTrainingModuleCollaboratorService)
    {
        _associationTrainingModuleCollaboratorService = associationTrainingModuleCollaboratorService;
    }

    [HttpPost]
    public async Task<ActionResult<AssociationTrainingModuleCollaboratorDTO>> Create([FromBody] CreateAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        var assocCreated = await _associationTrainingModuleCollaboratorService.Create(assocDTO);

        return assocCreated.ToActionResult();
    }
}

