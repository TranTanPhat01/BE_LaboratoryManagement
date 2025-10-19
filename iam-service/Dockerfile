# ============================
# STAGE 1: Build .NET Project
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy file csproj trước (để cache restore)
COPY iam-service.csproj ./
RUN dotnet restore

# Copy toàn bộ source code
COPY . .

# Build và publish ra thư mục /app/publish
RUN dotnet publish -c Release -o /app/publish

# ============================
# STAGE 2: Runtime Environment
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy build output từ stage 1 sang
COPY --from=build /app/publish .

# Expose port trùng với ServicePort
EXPOSE 8080

ENTRYPOINT ["dotnet", "iam-service.dll"]
