# FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
# COPY bin/Release/net5.0/publish/ App/
# WORKDIR /App
# ENTRYPOINT ["dotnet", "PersonasApp.dll"]
# CMD [ "1" ]


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PersonasApp.csproj", "./"]
RUN dotnet restore "PersonasApp.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "PersonasApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonasApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "PersonasApp.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
# COPY bin/Debug/net5.0 App/
# WORKDIR /App