using Application.DTO;

namespace Application.IServices;

public interface ITrainingModuleService
{
    public Task AddConsumed(CreateTrainingModuleDTO createDTO);
}
