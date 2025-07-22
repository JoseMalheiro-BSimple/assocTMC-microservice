using Application.DTO;

namespace Application.IServices;
public interface ICollaboratorService
{
    Task AddConsumed(CreateCollaboratorDTO createDTO);
}

