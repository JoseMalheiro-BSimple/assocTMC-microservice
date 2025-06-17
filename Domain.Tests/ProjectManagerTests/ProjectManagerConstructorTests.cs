using Domain.Models;
using Moq;

namespace Domain.Tests.ProjectManagerTests;

public class ProjectManagerConstructorTests
{
    [Fact]
    public void WhenCreatingProjectManagerWithValidPeriod_ThenProjectManagerIsCreatedCorrectly()
    {
        //arrange

        //act
        new ProjectManager(It.IsAny<Guid>(), It.IsAny<PeriodDateTime>());
        //assert
    }

    [Fact]
    public void WhenCreatingProjectManagerWithValidInitDate_ThenProjectManagerIsCreatedCorrectly()
    {
        //arrange

        //act
        var ProjectManager = new ProjectManager(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDateTime>());
        //assert
        Assert.NotNull(ProjectManager);
    }
}
