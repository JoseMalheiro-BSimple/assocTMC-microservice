using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly ICollaboratorFactory _collaboratorFactory;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
    }

    public async Task AddConsumed(CreateCollaboratorDTO createDTO)
    {
        ICollaborator collaborator;

        collaborator =  _collaboratorFactory.Create(createDTO.Id, createDTO.Period);
        collaborator = await _collaboratorRepository.AddAsync(collaborator);

        if (collaborator == null)
            throw new Exception("An error as occured!");
    }

    public async Task EditCollaborator(CollaboratorDTO dto)
    {
        var collab = await _collaboratorRepository.GetByIdAsync(dto.Id);
        if (collab == null)
            throw new Exception("Collaborator not found.");

        collab.UpdatePeriod(dto.PeriodDateTime);

        var updateCollabDetails = await _collaboratorRepository.UpdateCollaborator(collab);

        if (updateCollabDetails == null)
            throw new Exception("Failed to update collaborator.");
    }
}
