using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.AssociationTrainingModuleCollaboratorTests;
public class AssociationTrainingModuleCollaboratorFactoryTests
{
    [Fact]
    public void WhenPassingValidDependencies_ThenInstatiateFactory()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        // Act
        var factory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object); ;

        // Assert
        Assert.NotNull(factory);
    }

    [Fact]
    public async Task WhenPassingValidData_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(collab.Object);

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        var trainingModulePeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31))
        };
        tm.Setup(t => t.Periods).Returns(trainingModulePeriods); 

        tmRepo.Setup(tmr => tmr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(tm.Object);

        IEnumerable<IAssociationTrainingModuleCollaborator> list = new List<IAssociationTrainingModuleCollaborator>();

        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(list);
        
        DateOnly initDate = new DateOnly(2025, 6, 1); 
        DateOnly endDate = new DateOnly(2025, 6, 30); 

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Act
        var assocTMC = await assocTMCFactory.Create(Guid.NewGuid(), Guid.NewGuid(), initDate, endDate); 

        // Assert
        Assert.NotNull(assocTMC);
        Assert.Equal(initDate, assocTMC.PeriodDate.InitDate);
        Assert.Equal(endDate, assocTMC.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task WhenPassingInvalidCollabId_ThenThrowsArgumentException()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        tmRepo.Setup(tmr => tmr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(tm.Object);

        IEnumerable<IAssociationTrainingModuleCollaborator> list = new List<IAssociationTrainingModuleCollaborator>();

        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(list);

        // Example valid dates
        DateOnly initDate = new DateOnly();
        DateOnly endDate = initDate.AddDays(1);

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), initDate, endDate)
        );

        Assert.Equal("Collaborator must exists", exception.Message);
    }

    [Fact]
    public async Task WhenPassingInvalidTrainingModuleId_ThenThrowsArgumentException()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(collab.Object);

        // Example valid dates
        DateOnly initDate = new DateOnly();
        DateOnly endDate = initDate.AddDays(1);

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), initDate, endDate)
        );

        Assert.Equal("TrainingModule must exist", exception.Message);
    }

    [Fact]
    public async Task WhenOverlappingAssociationExists_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(collaboratorId)).ReturnsAsync(collab.Object);

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        var tmPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31))
        };
        tm.Setup(t => t.Periods).Returns(tmPeriods);

        tmRepo.Setup(tmr => tmr.GetByIdAsync(trainingModuleId)).ReturnsAsync(tm.Object);

        // Existing association that overlaps the new period
        var existingPeriod = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 10));
        var existingAssoc = new Mock<IAssociationTrainingModuleCollaborator>();
        existingAssoc.Setup(a => a.PeriodDate).Returns(existingPeriod);
        existingAssoc.Setup(a => a.CollaboratorId).Returns(collaboratorId);
        existingAssoc.Setup(a => a.TrainingModuleId).Returns(trainingModuleId);

        // Return a list containing the overlapping association
        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId))
                 .ReturnsAsync(new List<IAssociationTrainingModuleCollaborator> { existingAssoc.Object });

        var factory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // New period that overlaps existing one (e.g. June 5 - June 15)
        DateOnly initDate = new DateOnly(2025, 6, 5);
        DateOnly endDate = new DateOnly(2025, 6, 15);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            factory.Create(trainingModuleId, collaboratorId, initDate, endDate));

        Assert.Equal("An overlapping association already exists for this collaborator and training module.", exception.Message);
    }

    [Fact]
    public async Task WhenNoOverlapExistsForAdjacentPeriods_ThenCreateSuccessfully()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(collaboratorId)).ReturnsAsync(collab.Object);

        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        var tmPeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2025, 1, 1), new DateTime(2025, 12, 31)) 
        };
        tm.Setup(t => t.Periods).Returns(tmPeriods);
        tmRepo.Setup(tmr => tmr.GetByIdAsync(trainingModuleId)).ReturnsAsync(tm.Object);

        // Existing association ending exactly before new association starts
        var existingPeriod = new PeriodDate(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 10));
        var existingAssoc = new Mock<IAssociationTrainingModuleCollaborator>();
        existingAssoc.Setup(a => a.PeriodDate).Returns(existingPeriod);
        existingAssoc.Setup(a => a.CollaboratorId).Returns(collaboratorId);
        existingAssoc.Setup(a => a.TrainingModuleId).Returns(trainingModuleId);

        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(collaboratorId, trainingModuleId))
                 .ReturnsAsync(new List<IAssociationTrainingModuleCollaborator> { existingAssoc.Object });

        var factory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // New period starts the day after the existing period ends (no overlap)
        DateOnly initDate = new DateOnly(2025, 6, 11);
        DateOnly endDate = new DateOnly(2025, 6, 20);

        // Act
        var result = await factory.Create(trainingModuleId, collaboratorId, initDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingModuleId, result.TrainingModuleId);
        Assert.Equal(collaboratorId, result.CollaboratorId);
        Assert.Equal(initDate, result.PeriodDate.InitDate);
        Assert.Equal(endDate, result.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task WhenAssociationPeriodIsNotIncludedInTrainingModulePeriods_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(collaboratorId)).ReturnsAsync(collab.Object);

        // Setup TrainingModule with a period that DOES NOT include the new association's period
        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        var trainingModulePeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 10))
        };
        tm.Setup(t => t.Periods).Returns(trainingModulePeriods);
        tmRepo.Setup(tmr => tmr.GetByIdAsync(trainingModuleId)).ReturnsAsync(tm.Object);

        // No overlapping associations
        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(It.IsAny<Guid>(), It.IsAny<Guid>()))
                 .ReturnsAsync(new List<IAssociationTrainingModuleCollaborator>());

        var factory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Association period: August 1 to August 5, 2025 (clearly outside TM period)
        DateOnly initDate = new DateOnly(2025, 8, 1);
        DateOnly endDate = new DateOnly(2025, 8, 5);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            factory.Create(trainingModuleId, collaboratorId, initDate, endDate));

        Assert.Equal("The requested association period is not fully contained within any of the training module's defined periods.", exception.Message);
    }

    [Fact]
    public async Task WhenAssociationPeriodIsIncludedInTrainingModulePeriods_ThenCreatesSuccessfully()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        Mock<ICollaborator> collab = new Mock<ICollaborator>();
        collabRepo.Setup(cr => cr.GetByIdAsync(collaboratorId)).ReturnsAsync(collab.Object);

        // Setup TrainingModule with a period that DOES include the new association's period
        Mock<ITrainingModule> tm = new Mock<ITrainingModule>();
        var trainingModulePeriods = new List<PeriodDateTime>
        {
            // Training module period: May 1 to September 30, 2025
            new PeriodDateTime(new DateTime(2025, 5, 1), new DateTime(2025, 9, 30))
        };
        tm.Setup(t => t.Periods).Returns(trainingModulePeriods);
        tmRepo.Setup(tmr => tmr.GetByIdAsync(trainingModuleId)).ReturnsAsync(tm.Object);

        // No overlapping associations
        assocRepo.Setup(a => a.GetByCollabAndTrainingModule(It.IsAny<Guid>(), It.IsAny<Guid>()))
                 .ReturnsAsync(new List<IAssociationTrainingModuleCollaborator>());

        var factory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Association period: June 1 to June 15, 2025 (fully contained)
        DateOnly initDate = new DateOnly(2025, 6, 1);
        DateOnly endDate = new DateOnly(2025, 6, 15);

        // Act
        var result = await factory.Create(trainingModuleId, collaboratorId, initDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trainingModuleId, result.TrainingModuleId);
        Assert.Equal(collaboratorId, result.CollaboratorId);
        Assert.Equal(initDate, result.PeriodDate.InitDate);
        Assert.Equal(endDate, result.PeriodDate.FinalDate);
    }

    [Fact]
    public void WhenPassingValidatedData_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange
        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Act
        var result = assocTMCFactory.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void WhenPassingValidVisitor_ThenCreateAssociationTrainingModuleCollaborator()
    {
        // Arrange 
        Mock<IAssociationTrainingModuleCollaboratorVisitor> visitor = new Mock<IAssociationTrainingModuleCollaboratorVisitor>();

        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.CollaboratorId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.TrainingModuleId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.PeriodDate).Returns(It.IsAny<PeriodDate>());

        Mock<ICollaboratorRepository> collabRepo = new Mock<ICollaboratorRepository>();
        Mock<ITrainingModuleRepository> tmRepo = new Mock<ITrainingModuleRepository>();
        Mock<IAssociationTrainingModuleCollaboratorsRepository> assocRepo = new Mock<IAssociationTrainingModuleCollaboratorsRepository>();

        var assocTMCFactory = new AssociationTrainingModuleCollaboratorFactory(collabRepo.Object, tmRepo.Object, assocRepo.Object);

        // Act
        var assocTMC = assocTMCFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(assocTMC);
    }

}
