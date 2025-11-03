# Blazor Server (.NET 8)
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects needed for restore/publish
COPY DnpAssignmentG3.sln ./
COPY ApiContracts/ ApiContracts/
COPY BlazorApp/ BlazorApp/

# Restore and publish
RUN dotnet restore BlazorApp/BlazorApp.csproj
RUN dotnet publish BlazorApp/BlazorApp.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Bind to port 8080; API_BASE_URL will be provided via env var
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp.dll"]
