# Web API (.NET 8)
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects needed for restore/publish
COPY DnpAssignmentG3.sln ./
COPY ApiContracts/ ApiContracts/
COPY Server/Entities/ Server/Entities/
COPY Server/RepositoryContracts/ Server/RepositoryContracts/
COPY Server/FileRepositories/ Server/FileRepositories/
COPY Server/WebApi/ Server/WebApi/

# Restore and publish
RUN dotnet restore Server/WebApi/WebApi.csproj
RUN dotnet publish Server/WebApi/WebApi.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render will route traffic to this port; bind Kestrel accordingly
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
