FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["clients/clients.csproj", "clients/"]
RUN dotnet restore "clients/clients.csproj"
COPY . .
WORKDIR "/src/clients"
RUN dotnet build "clients.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "clients.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "clients.dll"]
