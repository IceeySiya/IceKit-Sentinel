using Microsoft.EntityFrameworkCore;

namespace IceKitSentinel.Api.Data;

public class IceKitDbContext : DbContext
{
    public IceKitDbContext(DbContextOptions<IceKitDbContext> options)
        : base(options)
    {
    }
}

