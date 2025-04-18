using Microsoft.EntityFrameworkCore;

namespace Cloudweather.Temperature.DataAccess;

public class WeatherReportDbContext : DbContext
{
    public WeatherReportDbContext(DbContextOptions<WeatherReportDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherReport> WeatherReports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SankeCaseIdentityTableNames(modelBuilder);
    }

    private static void SankeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherReport>().ToTable("weather_report");
    }
}