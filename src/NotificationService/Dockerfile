FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY src/NotificationService/NotificationService.csproj ./src/NotificationService/
COPY src/Contracts/Contracts.csproj ./src/Contracts/


RUN dotnet restore ./src/NotificationService/NotificationService.csproj

# below command copies everything to the working directory
COPY . .  

# publish the application to the docker /app/publish folder in release mode
RUN dotnet publish ./src/NotificationService/NotificationService.csproj -c Release -o /app/publish

# runs the above published version of the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]
