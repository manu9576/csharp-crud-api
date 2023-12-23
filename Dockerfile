FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5210

ENV ASPNETCORE_URLS=http://+:5210

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["csharp-crud-api/csharp-crud-api.csproj", "csharp-crud-api/"]
RUN dotnet restore "csharp-crud-api/csharp-crud-api.csproj"
COPY . .
WORKDIR "/src/csharp-crud-api"
RUN dotnet build "csharp-crud-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "csharp-crud-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "csharp-crud-api.dll"]
