using Application.DTO;
using Docker.DotNet.Models;
using Domain.Factory;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
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

        // Insert TrainingModule and Collaborator into DB
        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();

            context.TrainingModules.Add(new TrainingModuleDataModel
            {
                Id = trainingModuleId,
                // set required fields if any
            });

            context.Collaborators.Add(new CollaboratorDataModel
            {
                Id = collaboratorId,
                // set required fields if any
            });

            await context.SaveChangesAsync();
        }

        var createDto = new CreateAssociationTrainingModuleCollaboratorDTO
        {
            TrainingModuleId = trainingModuleId,
            CollaboratorId = collaboratorId,
            PeriodDate = new PeriodDate(
                DateOnly.FromDateTime(DateTime.UtcNow.Date),
                DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(6))
            )
        };

        // Act
        var response = await Client.PostAsJsonAsync("api/associationsTMC", createDto);

        // Assert
        response.EnsureSuccessStatusCode();

        var resultDto = await response.Content.ReadFromJsonAsync<AssociationTrainingModuleCollaboratorDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(trainingModuleId, resultDto!.TrainingModuleId);
        Assert.Equal(collaboratorId, resultDto.CollaboratorId);
        Assert.NotEqual(Guid.Empty, resultDto.Id);

    }
}
