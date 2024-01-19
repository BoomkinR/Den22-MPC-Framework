FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MpcRen.Register.Server/MpcRen.Register.Server.csproj", "MpcRen.Register.Server/"]
COPY ["MpcRen.Register.Infrastructure/MpcRen.Register.Infrastructure.csproj", "MpcRen.Register.Infrastructure/"]
RUN dotnet restore "MpcRen.Register.Server/MpcRen.Register.Server.csproj"
COPY . .
WORKDIR "/src/MpcRen.Register.Server"
RUN dotnet build "MpcRen.Register.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MpcRen.Register.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MpcRen.Register.Server.dll"]
