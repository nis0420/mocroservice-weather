# Use .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies


# Copy all source and publish
COPY . .
RUN dotnet restore "Cloudweather.Temperature.csproj" --disable-parallel
RUN dotnet publish "Cloudweather.Temperature.csproj" -c Release -o /app/publish --no-restore

# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Expose ports and configure URL
EXPOSE 5000

# Copy published output and set entrypoint

ENTRYPOINT ["dotnet", "Cloudweather.Temperature.dll"]