using Domain.Models;

namespace Domain.Messages;
public record TrainingModuleCreated(Guid id, Guid trainingSubjectId, List<PeriodDateTime> periods);
