using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssociationTrainingModuleCollaboratorRepositoryEF : GenericRepositoryEF<IAssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaborator, AssociationTrainingModuleCollaboratorDataModel>, IAssociationTrainingModuleCollaboratorsRepository
    {
        private readonly IMapper _mapper;
        public AssociationTrainingModuleCollaboratorRepositoryEF(AssocTMCContext context, IMapper mapper) : base(context, mapper)
        {
            _mapper = mapper;
        }
        public override IAssociationTrainingModuleCollaborator? GetById(Guid id)
        {
            var trainingModuleCollabDM = _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                    .FirstOrDefault(t => t.Id == id);

            if (trainingModuleCollabDM == null)
                return null;

            return _mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(trainingModuleCollabDM);
        }

        public override async Task<IAssociationTrainingModuleCollaborator?> GetByIdAsync(Guid id)
        {
            var trainingModuleCollabDM = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                    .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingModuleCollabDM == null)
                return null;

            return _mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>(trainingModuleCollabDM);
        }

        public async Task<IEnumerable<IAssociationTrainingModuleCollaborator>> GetByCollabAndTrainingModule(Guid collabId, Guid trainingModuleId)
        {
            var assocsDM = await _context.Set<AssociationTrainingModuleCollaboratorDataModel>()
                                         .Where(a => a.CollaboratorId == collabId && a.TrainingModuleId == trainingModuleId)
                                         .ToListAsync();

            return assocsDM.Select(_mapper.Map<AssociationTrainingModuleCollaboratorDataModel, IAssociationTrainingModuleCollaborator>);
        }

    }
}
