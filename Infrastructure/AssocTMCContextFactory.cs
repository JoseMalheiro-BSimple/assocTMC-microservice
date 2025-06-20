using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class AssocTMCContextFactory : IDesignTimeDbContextFactory<AssocTMCContext>
{
    public AssocTMCContext CreateDbContext(string[] args)
    {
        // Path to your WebApi project folder (where appsettings.json lives)
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../InterfaceAdapters");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AssocTMCContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new AssocTMCContext(optionsBuilder.Options);
    }
}
