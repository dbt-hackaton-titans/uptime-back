#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Titans.Uptime.Api/Titans.Uptime.Api.csproj", "Titans.Uptime.Api/"]
COPY ["Titans.Uptime.Application/Titans.Uptime.Application.csproj", "Titans.Uptime.Application/"]
COPY ["Titans.Uptime.Domain/Titans.Uptime.Domain.csproj", "Titans.Uptime.Domain/"]
COPY ["Titans.Uptime.Persistence/Titans.Uptime.Persistence.csproj", "Titans.Uptime.Persistence/"]
RUN dotnet restore "./Titans.Uptime.Api/Titans.Uptime.Api.csproj"
COPY . .
WORKDIR "/src/Titans.Uptime.Api"
RUN dotnet build "./Titans.Uptime.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Titans.Uptime.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "Titans.Uptime.Api.dll"]