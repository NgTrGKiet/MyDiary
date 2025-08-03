FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MyDiary.API/MyDiary.API.csproj", "MyDiary.API/"]
COPY ["MyDiary.Application/MyDiary.Application.csproj", "MyDiary.Application/"]
COPY ["MyDiary.Infrastructure/MyDiary.Infrastructure.csproj", "MyDiary.Infrastructure/"]
COPY ["MyDiary.BuildingBlocka/MyDiary.BuildingBlock.csproj", "MyDiary.BuildingBlocka/"]
COPY ["MyDiary.Domain/MyDiary.Domain.csproj", "MyDiary.Domain/"]

RUN dotnet restore "MyDiary.API/MyDiary.API.csproj"

COPY . .

# Build
WORKDIR "/src/MyDiary.API"
RUN dotnet build "MyDiary.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyDiary.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyDiary.API.dll"]
