# -------------------------------
# 1) Build stage: restore, build & publish
# -------------------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    
    # Copy the project file and restore dependencies
    COPY *.csproj ./
    RUN dotnet restore --disable-parallel
    
    # Copy all source code
    COPY . ./
    
    # Publish (allowing implicit restore)
    RUN dotnet publish "Cloudweather.Temperature.csproj" \
        -c Release \
        -o /app/publish
    
    # -------------------------------
    # 2) Runtime stage: runtime + published app
    # -------------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    WORKDIR /app
    
    # Copy the published app from build stage
    COPY --from=build /app/publish ./
    
    # Configure the app to listen on port 5000
    ENV ASPNETCORE_URLS=http://+:5000
    EXPOSE 5000
    
    # Entry point
    ENTRYPOINT ["dotnet", "Cloudweather.Temperature.dll"]
    