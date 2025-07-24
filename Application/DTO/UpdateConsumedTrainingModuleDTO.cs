using Domain.ValueObjects;

namespace Application.DTO;

public record UpdateConsumedTrainingModuleDTO(Guid Id, List<PeriodDateTime> Periods);
