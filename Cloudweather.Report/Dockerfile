# -------------------------------
# 1) Build stage: restore, build & publish
# -------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file to leverage Docker cache
COPY Cloudweather.Report.csproj ./
RUN dotnet restore --disable-parallel

# Copy all source code and publish the app
COPY . ./
RUN dotnet publish "Cloudweather.Report.csproj" \
    -c Release \
    -o /app/publish

# -------------------------------
# 2) Runtime stage: slim image with only runtime + published app
# -------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish ./

# Configure the app to listen on port 5000
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Entry point to launch your app DLL
ENTRYPOINT ["dotnet", "Cloudweather.Report.dll"]