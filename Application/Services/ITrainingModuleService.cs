namespace Application.Services;

public interface ITrainingModuleService
{
    public Task SubmitAsync(Guid id);
}
