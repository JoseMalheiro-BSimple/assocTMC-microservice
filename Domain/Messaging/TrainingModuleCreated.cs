using Domain.Models;

namespace Domain.Messaging;
public record TrainingModuleCreated(Guid id, Guid trainingSubjectId, List<PeriodDateTime> periods);
