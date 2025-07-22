using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CollaboratorRepositoryEF : GenericRepositoryEF<ICollaborator, Collaborator, CollaboratorDataModel>, ICollaboratorRepository
{
    private readonly IMapper _mapper;
    public CollaboratorRepositoryEF(AssocTMCContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override ICollaborator? GetById(Guid id)
    {
        var collabDM = _context.Set<CollaboratorDataModel>()
                    .FirstOrDefault(c => c.Id == id);

        if (collabDM == null)
            return null;

        return _mapper.Map<CollaboratorDataModel, ICollaborator>(collabDM);
    }

    public override async Task<ICollaborator?> GetByIdAsync(Guid id)
    {
        var collabDM = await _context.Set<CollaboratorDataModel>()
                    .FirstOrDefaultAsync(c => c.Id == id);

        if (collabDM == null)
            return null;

        return _mapper.Map<CollaboratorDataModel, ICollaborator>(collabDM);
    }

    public async Task<Collaborator?> UpdateCollaborator(ICollaborator collab)
    {
        var collaboratorDM = await _context.Set<CollaboratorDataModel>()
            .FirstOrDefaultAsync(c => c.Id == collab.Id);

        if (collaboratorDM == null) return null;

        collaboratorDM.Period = collab.Period;

        _context.Set<CollaboratorDataModel>().Update(collaboratorDM);
        await _context.SaveChangesAsync();
        return _mapper.Map<CollaboratorDataModel, Collaborator>(collaboratorDM);
    }
}
