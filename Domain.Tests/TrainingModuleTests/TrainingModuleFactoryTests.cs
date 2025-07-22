using Domain.Models;
using Domain.ValueObjects;
using Moq;

namespace Domain.Tests.TrainingModuleTests;

public class TrainingModuleFactoryTests
{
    [Fact]
    public void WhenPassingValidGUID_ThenReturnTrainingModule()
    {
        // Act
        new TrainingModule(It.IsAny<Guid>(), It.IsAny<List<PeriodDateTime>>());
    }
}
