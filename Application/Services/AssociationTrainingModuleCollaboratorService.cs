using Application.DTO;
using Application.Publishers;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;

namespace Application.Services;

public class AssociationTrainingModuleCollaboratorService
{
    public IAssociationTrainingModuleCollaboratorsRepository _assocTMCRepository;
    public IAssociationTrainingModuleCollaboratorFactory _assocTMCFactory;
    private readonly IMessagePublisher _publisher;
    public AssociationTrainingModuleCollaboratorService(IAssociationTrainingModuleCollaboratorsRepository associationTrainingModuleCollaboratorsRepository, IAssociationTrainingModuleCollaboratorFactory associationTrainingModuleCollaboratorFactory, IMessagePublisher messagePublisher)
    {
        _assocTMCRepository = associationTrainingModuleCollaboratorsRepository;
        _assocTMCFactory = associationTrainingModuleCollaboratorFactory;
        _publisher = messagePublisher;
    }

    public async Task<Result<AssociationTrainingModuleCollaboratorDTO>> Create(CreateAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        IAssociationTrainingModuleCollaborator tmc;

        try
        {
            tmc = await _assocTMCFactory.Create(assocDTO.TrainingModuleId, assocDTO.CollaboratorId, assocDTO.PeriodDate.InitDate, assocDTO.PeriodDate.FinalDate);
            tmc = await _assocTMCRepository.AddAsync(tmc);
        }
        catch (ArgumentException a)
        {
            return Result<AssociationTrainingModuleCollaboratorDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception e)
        {
            return Result<AssociationTrainingModuleCollaboratorDTO>.Failure(Error.BadRequest(e.Message));
        }

        // Publish results - new association has been created
        await _publisher.PublishOrderSubmittedAsync(tmc.Id, tmc.TrainingModuleId, tmc.CollaboratorId, tmc.PeriodDate);

        var result = new AssociationTrainingModuleCollaboratorDTO();
        result.Id = tmc.Id;
        result.CollaboratorId = tmc.CollaboratorId;
        result.TrainingModuleId = tmc.TrainingModuleId;
        result.PeriodDate = tmc.PeriodDate;

        return Result<AssociationTrainingModuleCollaboratorDTO>.Success(result);
    }

    public async Task CreateWithNoValidations(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        // There is no data validation, but there is validation to no insert duplicate values on table
        IAssociationTrainingModuleCollaborator? assoc = await _assocTMCRepository.GetByIdAsync(id);

        if (assoc == null)
        {
            IAssociationTrainingModuleCollaborator tmc;

            tmc = _assocTMCFactory.Create(id, trainingModuleId, collaboratorId, periodDate);
            await _assocTMCRepository.AddAsync(tmc);
        }
    }
}