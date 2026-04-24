FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Observability.ConsoleApp/Observability.ConsoleApp.csproj", "Observability.ConsoleApp/"]
RUN dotnet restore "Observability.ConsoleApp/Observability.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/Observability.ConsoleApp"
RUN dotnet build "Observability.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Observability.ConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Observability.ConsoleApp.dll"]
