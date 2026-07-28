using Microsoft.EntityFrameworkCore;
using IceKitSentinel.Api.Models;

namespace IceKitSentinel.Api.Data;

public class IceKitDbContext : DbContext
{
    public IceKitDbContext(DbContextOptions<IceKitDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}

