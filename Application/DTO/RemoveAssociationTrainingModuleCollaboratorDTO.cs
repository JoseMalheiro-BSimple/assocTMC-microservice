namespace Application.DTO;

public record RemoveAssociationTrainingModuleCollaboratorDTO
{
    public Guid Id { get; set; }

    public RemoveAssociationTrainingModuleCollaboratorDTO(Guid id)
    {
        Id = id;
    }
}
