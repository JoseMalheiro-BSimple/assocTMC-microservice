using Application.DTO;
using Application.IServices;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

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

    public async Task SubmitUpdateAsync(UpdateConsumedTrainingModuleDTO updateDTO)
    {
        var trainingModule = await _tmRepository.GetByIdAsync(updateDTO.Id);
        if (trainingModule == null)
            throw new ArgumentException("TrainingModule not found.");

        var updatedTrainingModule = new TrainingModule(updateDTO.Id, updateDTO.Periods);
        var updated = await _tmRepository.UpdateAsync(updatedTrainingModule);
    }
}
