using Domain.ValueObjects;

namespace Domain.Messages;
public record CollaboratorCreatedMessage(Guid Id, Guid UserId, PeriodDateTime PeriodDateTime);
    
    

