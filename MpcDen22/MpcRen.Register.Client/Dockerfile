FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MpcRen.Register.Client/MpcRen.Register.Client.csproj", "MpcRen.Register.Client/"]
COPY ["MpcRen.Register.Infrastructure/MpcRen.Register.Infrastructure.csproj", "MpcRen.Register.Infrastructure/"]
RUN dotnet restore "MpcRen.Register.Client/MpcRen.Register.Client.csproj"
COPY . .
WORKDIR "/src/MpcRen.Register.Client"
RUN dotnet build "MpcRen.Register.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MpcRen.Register.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MpcRen.Register.Client.dll"]
