using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

    public async Task<ITrainingModule> UpdateAsync(ITrainingModule entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var existingTrainingModuleDM = await _context.Set<TrainingModuleDataModel>()
                                                     .Include(dm => dm.Periods)
                                                     .FirstOrDefaultAsync(dm => dm.Id == entity.Id);

        if (existingTrainingModuleDM == null)
        {
            throw new ArgumentException($"TrainingModule with ID {entity.Id} not found for update.");
        }

        existingTrainingModuleDM.Periods.Clear();
        if (entity.Periods != null)
        {
            existingTrainingModuleDM.Periods.AddRange(entity.Periods);
        }

        await _context.SaveChangesAsync();

        var result = _mapper.Map<TrainingModuleDataModel, ITrainingModule>(existingTrainingModuleDM);

        return result;
    }
}
