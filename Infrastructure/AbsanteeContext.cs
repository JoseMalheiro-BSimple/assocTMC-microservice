﻿using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public virtual DbSet<AssociationProjectCollaboratorDataModel> AssociationsProjectCollaborator { get; set; }
        public virtual DbSet<HolidayPlanDataModel> HolidayPlans { get; set; }
        public virtual DbSet<ProjectDataModel> Projects { get; set; }
        public virtual DbSet<UserDataModel> Users { get; set; }
        public virtual DbSet<TrainingSubjectDataModel> TrainingSubjects { get; set; }
        public virtual DbSet<TrainingModuleDataModel> TrainingModules { get; set; }
        public virtual DbSet<AssociationTrainingModuleCollaboratorDataModel> AssociationTrainingModuleCollaborators { get; set; }
        public virtual DbSet<TrainingPeriodDataModel> TrainingPeriods { get; set; }
        public virtual DbSet<HRManagerDataModel> HRManagers { get; set; }


        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDataModel>()
                .OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<AssociationProjectCollaboratorDataModel>()
                .OwnsOne(a => a.PeriodDate);
            modelBuilder.Entity<HRManagerDataModel>()
            .OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<TrainingPeriodDataModel>()
                .OwnsOne(a => a.PeriodDate);

            modelBuilder.Entity<CollaboratorDataModel>()
                .OwnsOne(a => a.PeriodDateTime);


            modelBuilder.Entity<ProjectDataModel>()
                .OwnsOne(a => a.PeriodDate);

            modelBuilder.Entity<HolidayPeriodDataModel>(entity =>
            {
                entity.Property(h => h.Id)
                      .ValueGeneratedNever();

                entity.OwnsOne(h => h.PeriodDate);
            });

            modelBuilder.Entity<TrainingModuleDataModel>()
                .OwnsMany(t => t.Periods);

            base.OnModelCreating(modelBuilder);
        }
    }
}
