FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Clear NuGet cache to avoid local DevExpress references
RUN dotnet nuget locals all --clear

# Copy NuGet config to override any local configurations
COPY ./NuGet.Config ./

# Copy all source code
COPY ./src ./src

# Restore and build from the main project
WORKDIR /src/src/app/Cnab.Consumer.Worker
RUN dotnet restore --force --no-cache --verbosity quiet --configfile /src/NuGet.Config
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet","Cnab.Consumer.Worker.dll"]
ENTRYPOINT ["dotnet","Cnab.Consumer.Worker.dll"]
