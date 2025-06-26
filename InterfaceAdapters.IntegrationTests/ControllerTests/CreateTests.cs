using Xunit;

namespace InterfaceAdapters.IntegrationTests.Tests;
public class CreateTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    public CreateTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient()) { }

    [Fact]
    public async Task CreateAssociation_ReturnsCreatedResult()
    {
        
    }
}
