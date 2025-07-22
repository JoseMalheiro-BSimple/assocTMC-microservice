using Application.DTO;
using Application.Publishers;
using Domain.Factory;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Data.Entity;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.Tests;
public class CreateTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public CreateTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }


    [Fact]
    public async Task CreateAssociation_ReturnsCreatedResult()
    {
        // Arrange - insert TrainingModule and Collaborator manually to DB
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        DateOnly associationInitDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(10));
        DateOnly associationEndDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(20));
        var associationPeriod = new PeriodDate(associationInitDate, associationEndDate);

        DateTime collabInitDate = DateTime.UtcNow.Date.AddDays(5).ToUniversalTime();
        DateTime collabEndDate = DateTime.UtcNow.Date.AddDays(30).ToUniversalTime();
        var collaboratorPeriod = new PeriodDateTime(collabInitDate, collabEndDate);

        var trainingModulePeriods = new List<PeriodDateTime>
        {
            new PeriodDateTime(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc)),
            new PeriodDateTime(DateTime.UtcNow.Date.AddDays(5).ToUniversalTime(), DateTime.UtcNow.Date.AddDays(30).ToUniversalTime())
        };

        var mockPublisher = new Mock<IAssociationTrainingModuleCollaboratorPublisher>();

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.TrainingModules.Add(new TrainingModuleDataModel
            {
                Id = trainingModuleId,
                Periods = trainingModulePeriods,
            });

            context.Collaborators.Add(new CollaboratorDataModel
            {
                Id = collaboratorId,
                Period = collaboratorPeriod
            });

            await context.SaveChangesAsync();
        }

        var createDto = new CreateAssociationTrainingModuleCollaboratorDTO
        (
            collaboratorId,
            trainingModuleId,
            associationPeriod
        );

        // Act
        var response = await PostAndDeserializeAsync<AssociationTrainingModuleCollaboratorDTO>("api/associationsTMC", createDto);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(trainingModuleId, response!.TrainingModuleId);
        Assert.Equal(collaboratorId, response.CollaboratorId);
        Assert.NotEqual(Guid.Empty, response.Id);
    }
}
