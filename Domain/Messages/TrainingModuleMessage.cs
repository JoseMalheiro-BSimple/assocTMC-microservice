using Domain.ValueObjects;
namespace Domain.Messages;

public record TrainingModuleMessage(Guid Id, Guid SubjectId, List<PeriodDateTime> Periods);