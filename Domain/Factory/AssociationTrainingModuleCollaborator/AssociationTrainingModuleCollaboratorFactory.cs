using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;
public class AssociationTrainingModuleCollaboratorFactory : IAssociationTrainingModuleCollaboratorFactory
{
    private readonly ICollaboratorRepository _collabRepository;
    private readonly ITrainingModuleRepository _trainingModuleRepository;
    private readonly IAssociationTrainingModuleCollaboratorsRepository _assocTMCRepository;

    public AssociationTrainingModuleCollaboratorFactory(ICollaboratorRepository collaboratorRepository, ITrainingModuleRepository trainingModuleRepository, IAssociationTrainingModuleCollaboratorsRepository assocTMCRepository)
    {
        _collabRepository = collaboratorRepository;
        _trainingModuleRepository = trainingModuleRepository;
        _assocTMCRepository = assocTMCRepository;
    }

    public async Task<IAssociationTrainingModuleCollaborator> Create(Guid trainingModuleId, Guid collaboratorId, DateOnly initDate, DateOnly endDate)
    {
        ICollaborator? collab = await _collabRepository.GetByIdAsync(collaboratorId);
        ITrainingModule? trainingModule = await _trainingModuleRepository.GetByIdAsync(trainingModuleId);
        
        if(collab == null)
            throw new ArgumentException("Collaborator must exists");

        if (trainingModule == null)
            throw new ArgumentException("TrainingModule must exist");

        PeriodDate periodDate = new PeriodDate(initDate, endDate);

        // Period has to be included in any of the training module periods
        bool isPeriodIncluded = trainingModule.Periods.Any(tmPeriod =>
        {
            // Convert DateTime to DateOnly for comparison
            DateOnly tmPeriodInitDate = DateOnly.FromDateTime(tmPeriod._initDate);
            DateOnly tmPeriodEndDate = DateOnly.FromDateTime(tmPeriod._finalDate);

            // Check if the requested periodDate is fully contained within tmPeriod
            return periodDate.InitDate >= tmPeriodInitDate && periodDate.FinalDate <= tmPeriodEndDate;
        });

        if (!isPeriodIncluded)
        {
            throw new InvalidOperationException("The requested association period is not fully contained within any of the training module's defined periods.");
        }

        // Unicity test
        IEnumerable<IAssociationTrainingModuleCollaborator> assocsSameCollabAndTM = await _assocTMCRepository.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId);

        bool hasOverlap = assocsSameCollabAndTM.Any(a =>
        a.PeriodDate.InitDate <= periodDate.FinalDate &&
        a.PeriodDate.FinalDate >= periodDate.InitDate);

        if (hasOverlap)
            throw new InvalidOperationException("An overlapping association already exists for this collaborator and training module.");

        Guid id = Guid.NewGuid();

        return new AssociationTrainingModuleCollaborator(id, trainingModuleId, collaboratorId, periodDate);
    }

    public IAssociationTrainingModuleCollaborator Create(Guid id, Guid trainingModuleId, Guid collaboratorId, PeriodDate periodDate)
    {
        return new AssociationTrainingModuleCollaborator(id, trainingModuleId, collaboratorId, periodDate);

    }

    public IAssociationTrainingModuleCollaborator Create(IAssociationTrainingModuleCollaboratorVisitor visitor)
    {
        return new AssociationTrainingModuleCollaborator(visitor.Id, visitor.TrainingModuleId, visitor.CollaboratorId, visitor.PeriodDate);
    }
}
