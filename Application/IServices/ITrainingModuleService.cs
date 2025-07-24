using Application.DTO;

namespace Application.IServices;

public interface ITrainingModuleService
{
    Task AddConsumed(CreateTrainingModuleDTO createDTO);
    Task SubmitUpdateAsync(UpdateConsumedTrainingModuleDTO updateDTO);
}
