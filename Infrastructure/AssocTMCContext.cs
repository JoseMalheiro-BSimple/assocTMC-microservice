using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class AssocTMCContext : DbContext
{
    public virtual DbSet<AssociationTrainingModuleCollaboratorDataModel> AssociationTrainingModuleCollaborators { get; set; }
    public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
    public virtual DbSet<TrainingModuleDataModel> TrainingModules { get; set; }

    public AssocTMCContext(DbContextOptions<AssocTMCContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssociationTrainingModuleCollaboratorDataModel>()
            .OwnsOne(a => a.PeriodDate);

        modelBuilder.Entity<CollaboratorDataModel>()
            .OwnsOne(a => a.Period);

        modelBuilder.Entity<TrainingModuleDataModel>().OwnsMany(
            tm => tm.Periods);

        base.OnModelCreating(modelBuilder);
    }
}
