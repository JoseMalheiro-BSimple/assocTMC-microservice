using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class TrainingModuleService : ITrainingModuleService
{
    public ITrainingModuleRepository _tmRepository { get; set; }
    public ITrainingModuleFactory _tmFactory { get; set; }

    public TrainingModuleService(ITrainingModuleRepository trainingModuleRepository, ITrainingModuleFactory trainingModuleFactory)
    {
        _tmRepository = trainingModuleRepository;
        _tmFactory = trainingModuleFactory;
    }

    public async Task SubmitAsync(Guid id)
    {
        ITrainingModule trainingModule;

        trainingModule = _tmFactory.Create(id);
        trainingModule = await _tmRepository.AddAsync(trainingModule);

        if (trainingModule == null)
            throw new Exception("An error as occured!");
    }
}
