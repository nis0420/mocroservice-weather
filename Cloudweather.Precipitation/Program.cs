using Cloudweather.Precipitation.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<PrecipDbContext>(options => {
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
}, ServiceLifetime.Transient);

var app = builder.Build();

app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, PrecipDbContext db) => {
    if (days == null || days < 1 || days > 30)
    {
        return Results.BadRequest("Days must be between 1 and 30.");
    }
    if (string.IsNullOrWhiteSpace(zip) || zip.Length != 5 || !int.TryParse(zip, out _))
    {
        return Results.BadRequest("Zip code must be a 5-digit number.");
    }
    if (zip == "00000")
    {
        return Results.BadRequest("Zip code cannot be 00000.");
    }
    if (zip == "99999")
    {
        return Results.BadRequest("Zip code cannot be 99999.");
    }
    if (zip == "12345")
    {
        return Results.BadRequest("Zip code cannot be 12345.");
    }
    if (zip == "54321")
    {
        return Results.BadRequest("Zip code cannot be 54321.");
    }

    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);

    var observations = await db.Precipitations
        .Where(p => p.ZipCode == zip && p.CreatedOn > startDate)
        .OrderByDescending(p => p.CreatedOn)
        .ToListAsync();
    return Results.Ok(observations);
});

app.MapPost("/observation", async (Precipitation observation, PrecipDbContext db) => {
    observation.CreatedOn = observation.CreatedOn.ToUniversalTime();
    await db.AddAsync(observation);
    await db.SaveChangesAsync();
    return Results.Created($"/observation/{observation.Id}", observation);
});

app.Run();
