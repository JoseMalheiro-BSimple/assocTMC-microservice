using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;
public class TrainingModuleService : ITrainingModuleService
{
    private readonly ITrainingModuleRepository _tmRepository;
    private readonly ITrainingModuleFactory _tmFactory; 

    public TrainingModuleService(ITrainingModuleRepository trainingModuleRepository, ITrainingModuleFactory trainingModuleFactory)
    {
        _tmRepository = trainingModuleRepository;
        _tmFactory = trainingModuleFactory;
    }

    public async Task AddConsumed(CreateTrainingModuleDTO createDTO)
    {
        ITrainingModule trainingModule;

        trainingModule = _tmFactory.Create(createDTO.Id, createDTO.Periods);
        trainingModule = await _tmRepository.AddAsync(trainingModule);

        if (trainingModule == null)
            throw new Exception("An error as occured!");
    }
}
