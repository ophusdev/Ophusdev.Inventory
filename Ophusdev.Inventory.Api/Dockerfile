# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Ophusdev.Inventory.Api/Ophusdev.Inventory.Api.csproj", "Ophusdev.Inventory.Api/"]
COPY ["Ophusdev.Inventory.Business/Ophusdev.Inventory.Business.csproj", "Ophusdev.Inventory.Business/"]
COPY ["Ophusdev.Inventory.Repository/Ophusdev.Inventory.Repository.csproj", "Ophusdev.Inventory.Repository/"]
COPY ["Ophusdev.Inventory.Shared/Ophusdev.Inventory.Shared.csproj", "Ophusdev.Inventory.Shared/"]

RUN dotnet restore "./Ophusdev.Inventory.Api/Ophusdev.Inventory.Api.csproj"

COPY . .
WORKDIR "/src/Ophusdev.Inventory.Api"
RUN dotnet build "./Ophusdev.Inventory.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ophusdev.Inventory.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ophusdev.Inventory.Api.dll"]