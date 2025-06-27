using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class TrainingModuleRepositoryEF : GenericRepositoryEF<ITrainingModule, TrainingModule, TrainingModuleDataModel>, ITrainingModuleRepository
{
    private readonly IMapper _mapper;
    public TrainingModuleRepositoryEF(AssocTMCContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override ITrainingModule? GetById(Guid id)
    {
        var tmDM = _context.Set<TrainingModuleDataModel>()
                .FirstOrDefault(tm => tm.Id == id);

        if (tmDM == null)
            return null;

        return _mapper.Map<TrainingModuleDataModel, ITrainingModule>(tmDM);
    }

    public override async Task<ITrainingModule?> GetByIdAsync(Guid id)
    {
        var tmDM = await _context.Set<TrainingModuleDataModel>()
                .FirstOrDefaultAsync(tm => tm.Id == id);

        if (tmDM == null)
            return null;

        return _mapper.Map<TrainingModuleDataModel, ITrainingModule>(tmDM);
    }
}
