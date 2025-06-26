using Domain.Models;

namespace Domain.Messaging;
public record CollaboratorCreated(Guid userId, Guid collabId, PeriodDateTime periodDateTime);
    
    

