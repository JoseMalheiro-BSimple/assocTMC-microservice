using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.DataModel;
using InterfaceAdapters.IntegrationTests.Tests;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class RemoveTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public RemoveTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task RemoveAssociation_ReturnsNoContentResult_WhenSuccessful()
    {
        // Arrange
        var associationId = Guid.NewGuid();
        var trainingModuleId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var associationPeriod = new PeriodDate(DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(10)), DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(20)));

        // Seed the database with an association to be removed
        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();
            context.AssociationTrainingModuleCollaborators.Add(new AssociationTrainingModuleCollaboratorDataModel
            {
                Id = associationId,
                TrainingModuleId = trainingModuleId,
                CollaboratorId = collaboratorId,
                PeriodDate = associationPeriod
            });
            await context.SaveChangesAsync();
        }

        // Act
        var response = await Client.DeleteAsync($"api/associationsTMC/{associationId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Verify the association is actually removed from the database
        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AssocTMCContext>();
            var removedAssociation = await context.AssociationTrainingModuleCollaborators.FindAsync(associationId);
            Assert.Null(removedAssociation);
        }
    }
}
