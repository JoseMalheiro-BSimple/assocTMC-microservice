using Domain.ValueObjects;

namespace Domain.Messages;
public record TrainingModuleCreatedMessage(Guid Id, Guid TrainingSubjectId, List<PeriodDateTime> Periods);
