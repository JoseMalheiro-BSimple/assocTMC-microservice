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
}
