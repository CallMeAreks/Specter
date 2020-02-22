FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Specter.Dashboard/Specter.Dashboard.csproj", "Specter.Dashboard/"]
RUN dotnet restore "Specter.Dashboard/Specter.Dashboard.csproj"
COPY . .
WORKDIR "/src/Specter.Dashboard"
RUN dotnet build "Specter.Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Specter.Dashboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Specter.Dashboard.dll"]