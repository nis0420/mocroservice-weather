# Use .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore --disable-parallel

# Copy the remaining source code
COPY . ./

# Publish the application
RUN dotnet publish "Cloudweather.DataLoader.csproj" \
    -c Release \
    -o /app/publish --no-restore

# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish ./

# Run the data loader
ENTRYPOINT ["dotnet", "Cloudweather.DataLoader.dll"]