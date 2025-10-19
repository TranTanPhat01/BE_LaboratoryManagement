# ============================
# STAGE 1: Build .NET Project
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy file csproj để restore
COPY api-gateway.csproj ./
RUN dotnet restore

# Copy toàn bộ source
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ============================
# STAGE 2: Runtime Environment
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose cổng đúng với docker-compose
EXPOSE 7000

ENTRYPOINT ["dotnet", "api-gateway.dll"]
