using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Infrastructure.DataModel;

[Table("ProjectManager")]
public class ProjectManagerDataModel : IProjectManagerVisitor
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public ProjectManagerDataModel(ProjectManager projectManager)
    {
        Id = projectManager.Id;
        UserId = projectManager.UserId;
        PeriodDateTime = projectManager.PeriodDateTime;
    }
}