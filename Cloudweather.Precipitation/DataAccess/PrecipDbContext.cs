using Microsoft.EntityFrameworkCore;

namespace Cloudweather.Precipitation.DataAccess;

public class PrecipDbContext : DbContext
{
    public PrecipDbContext()
    {
    }

    public PrecipDbContext(DbContextOptions<PrecipDbContext> options) : base(options)
    {
    }

    public DbSet<Precipitation> Precipitations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SankeCaseIdentityTableNames(modelBuilder);
    }

    private static void SankeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Precipitation>().ToTable("precipitation");
    }
}