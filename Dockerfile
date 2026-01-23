# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution
COPY . .

# Restore & publish ONLY the Host project
RUN dotnet restore src/Lucien.HttpApi.Host/Lucien.HttpApi.Host.csproj
RUN dotnet publish src/Lucien.HttpApi.Host/Lucien.HttpApi.Host.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Render requirement
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Lucien.HttpApi.Host.dll"]
