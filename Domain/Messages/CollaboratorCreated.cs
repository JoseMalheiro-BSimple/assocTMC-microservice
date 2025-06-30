using Domain.Models;

namespace Domain.Messages;
public record CollaboratorCreated(Guid userId, Guid collabId, PeriodDateTime periodDateTime);
    
    

