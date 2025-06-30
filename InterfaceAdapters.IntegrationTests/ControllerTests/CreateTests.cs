using Application.DTO;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
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
                Id = trainingModuleId
            });

            context.Collaborators.Add(new CollaboratorDataModel
            {
                Id = collaboratorId
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
        var response = await PostAndDeserializeAsync<AssociationTrainingModuleCollaboratorDTO>("api/associationsTMC", createDto);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(trainingModuleId, response!.TrainingModuleId);
        Assert.Equal(collaboratorId, response.CollaboratorId);
        Assert.NotEqual(Guid.Empty, response.Id);

    }

}
