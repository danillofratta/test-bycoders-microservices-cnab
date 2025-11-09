FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src ./src
WORKDIR /src/app/Cnab.Consumer.Worker
RUN dotnet restore
RUN dotnet publish -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV RABBIT_HOST=rabbitmq
ENTRYPOINT ["dotnet","Cnab.Consumer.Worker.dll"]
