using Application.DTO;

namespace Application.IServices;
public interface ICollaboratorService
{
    public Task AddConsumed(CreateCollaboratorDTO createDTO);
}

