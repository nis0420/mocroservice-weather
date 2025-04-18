using Microsoft.EntityFrameworkCore;

namespace Cloudweather.Temperature.DataAccess;

public class TempratureDbContext : DbContext
{

    public TempratureDbContext()
    {
    }
    public TempratureDbContext(DbContextOptions<TempratureDbContext> options) : base(options)
    {
    }

    public DbSet<Temprature> Tempratures { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SankeCaseIdentityTableNames(modelBuilder);
    }

    private static void SankeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Temprature>().ToTable("temprature");
    }
}