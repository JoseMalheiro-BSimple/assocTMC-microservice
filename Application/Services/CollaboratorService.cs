using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class CollaboratorService : ICollaboratorService
{
    public ICollaboratorRepository _collaboratorRepository { get; set; }
    public ICollaboratorFactory _collaboratorFactory { get; set; }

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
    }

    public async Task SubmitAsync(Guid id)
    {
        ICollaborator collaborator;

        collaborator =  _collaboratorFactory.Create(id);
        collaborator = await _collaboratorRepository.AddAsync(collaborator);

        if (collaborator == null)
            throw new Exception("An error as occured!");
    }
}
