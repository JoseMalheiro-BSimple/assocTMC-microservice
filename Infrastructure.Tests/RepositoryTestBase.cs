using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly AssocTMCContext context;

    protected RepositoryTestBase()
    {
        _mapper = new Mock<IMapper>();

        // Configure in-memory EF Core context
        var options = new DbContextOptionsBuilder<AssocTMCContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // isolate DB per test
            .Options;

        context = new AssocTMCContext(options);
    }
}
