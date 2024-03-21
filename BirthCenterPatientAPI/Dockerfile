# Base ASP.NET image for running web applications
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# .NET Core SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BirthCenterPatientAPI.csproj", "."]
RUN dotnet restore "./BirthCenterPatientAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./BirthCenterPatientAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Application build and publishing
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BirthCenterPatientAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Base ASP.NET image for final application runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Adding support for SQLite database
RUN apt-get update \
    && apt-get install -y sqlite3 libsqlite3-dev \
    && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "BirthCenterPatientAPI.dll"]
