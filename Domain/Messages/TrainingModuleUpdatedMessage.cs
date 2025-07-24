using Domain.ValueObjects;

namespace Domain.Messages;
public record TrainingModuleUpdatedMessage(Guid Id, Guid SubjectId, List<PeriodDateTime> Periods);