# --- Build aşaması ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY subiletbackend.sln ./
COPY subiletbackend/subiletbackend/*.csproj ./subiletbackend/
RUN dotnet restore ./subiletbackend/subiletbackend.csproj
COPY subiletbackend/subiletbackend/. ./subiletbackend/
WORKDIR /src/subiletbackend
RUN dotnet publish subiletbackend.csproj -c Release -o /app/publish

# --- Runtime aşaması ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "subiletbackend.dll"] 