using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;
public interface ITrainingModuleRepository : IGenericRepositoryEF<ITrainingModule, TrainingModule, ITrainingModuleVisitor> 
{
    Task<ITrainingModule> UpdateAsync(ITrainingModule entity);
}
