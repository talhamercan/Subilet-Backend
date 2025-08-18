FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/SubiletServer.WebAPI/SubiletServer.WebAPI.csproj", "src/SubiletServer.WebAPI/"]
COPY ["src/SubiletServer.Application/SubiletServer.Application.csproj", "src/SubiletServer.Application/"]
COPY ["src/SubiletServer.Infrastructure/SubiletServer.Infrastructure.csproj", "src/SubiletServer.Infrastructure/"]
COPY ["src/SubiletServer.Domain/SubiletServer.Domain.csproj", "src/SubiletServer.Domain/"]
RUN dotnet restore "src/SubiletServer.WebAPI/SubiletServer.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/SubiletServer.WebAPI"
RUN dotnet build "SubiletServer.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SubiletServer.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubiletServer.WebAPI.dll"] 