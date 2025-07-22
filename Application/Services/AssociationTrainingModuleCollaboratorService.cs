using Application.DTO;
using Application.IServices;
using Application.Publishers;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class AssociationTrainingModuleCollaboratorService : IAssociationTrainingModuleCollaboratorService
{
    private readonly IAssociationTrainingModuleCollaboratorsRepository _assocTMCRepository;
    private readonly IAssociationTrainingModuleCollaboratorFactory _assocTMCFactory;
    private readonly IAssociationTrainingModuleCollaboratorPublisher _publisher;
    public AssociationTrainingModuleCollaboratorService(IAssociationTrainingModuleCollaboratorsRepository associationTrainingModuleCollaboratorsRepository, IAssociationTrainingModuleCollaboratorFactory associationTrainingModuleCollaboratorFactory, IAssociationTrainingModuleCollaboratorPublisher messagePublisher)
    {
        _assocTMCRepository = associationTrainingModuleCollaboratorsRepository;
        _assocTMCFactory = associationTrainingModuleCollaboratorFactory;
        _publisher = messagePublisher;
    }

    public async Task<Result<AssociationTrainingModuleCollaboratorDTO>> Create(CreateAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        IAssociationTrainingModuleCollaborator tmc = null!;
        try
        {
            tmc = await _assocTMCFactory.Create(assocDTO.TrainingModuleId, assocDTO.CollaboratorId, assocDTO.PeriodDate.InitDate, assocDTO.PeriodDate.FinalDate);
            tmc = _assocTMCRepository.AddWithoutSavingAsync(tmc);

            await _publisher.PublishAssociationTrainingModuleCollaboratorCreatedMessage(tmc.Id, tmc.TrainingModuleId, tmc.CollaboratorId, tmc.PeriodDate);

            // Only save my changes if the publish occurs with no errors
            await _assocTMCRepository.SaveChangesAsync();

            var resultDto = new AssociationTrainingModuleCollaboratorDTO();
            resultDto.Id = tmc.Id;
            resultDto.CollaboratorId = tmc.CollaboratorId;
            resultDto.TrainingModuleId = tmc.TrainingModuleId;
            resultDto.PeriodDate = tmc.PeriodDate;

            return Result<AssociationTrainingModuleCollaboratorDTO>.Success(resultDto);
        }
        catch (ArgumentException a)
        {
            return Result<AssociationTrainingModuleCollaboratorDTO>.Failure(Error.BadRequest(a.Message));
        }
        catch (Exception e)
        {
            return Result<AssociationTrainingModuleCollaboratorDTO>.Failure(Error.InternalServerError($"An unexpected error occurred during creation: {e.Message}"));
        }
    }

    public async Task CreateWithNoValidations(CreateConsumedAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        // There is no data validation, but there is validation to no insert duplicate values on table
        IAssociationTrainingModuleCollaborator? assoc = await _assocTMCRepository.GetByIdAsync(assocDTO.Id);

        if (assoc == null)
        {
            IAssociationTrainingModuleCollaborator tmc;

            tmc = _assocTMCFactory.Create(assocDTO.Id, assocDTO.TrainingModuleId, assocDTO.CollaboratorId, assocDTO.PeriodDate);
            tmc = await _assocTMCRepository.AddAsync(tmc);

            if (tmc == null)
                throw new Exception("An error occured!");
        }
    }

    public async Task<Result> Remove(RemoveAssociationTrainingModuleCollaboratorDTO assocDTO)
    {
        IAssociationTrainingModuleCollaborator? associationToRemove;
        try
        {
            associationToRemove = await _assocTMCRepository.GetByIdAsync(assocDTO.Id);

            if (associationToRemove == null)
            {
                return Result.Failure(Error.NotFound("AssociationTrainingModuleCollaborator not found."));
            }

            await _assocTMCRepository.RemoveWithoutSavingAsync(associationToRemove);

            await _publisher.PublishAssociationTrainingModuleCollaboratorRemovedMessage(assocDTO.Id);

            // Only save removal after the publish goes through
            await _assocTMCRepository.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.InternalServerError($"An unexpected error occurred: {ex.Message}"));
        }
    }
}