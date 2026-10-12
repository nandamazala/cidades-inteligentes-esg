FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Sus.Api/Sus.Api.csproj", "src/Sus.Api/"]
RUN dotnet restore "src/Sus.Api/Sus.Api.csproj"
COPY . .
WORKDIR "/src/src/Sus.Api"
RUN dotnet publish "Sus.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Sus.Api.dll"]
