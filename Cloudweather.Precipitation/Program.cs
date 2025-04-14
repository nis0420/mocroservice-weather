var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/observation/{zip}", (string zip) => {
    return $"The zip code is {zip}";
});

app.Run();
