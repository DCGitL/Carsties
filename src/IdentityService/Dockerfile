FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY src/IdentityService/IdentityService.csproj ./src/IdentityService/

RUN dotnet restore ./src/IdentityService/IdentityService.csproj

# below command copies everything to the working directory
COPY . .  

# publish the application to the docker /app/publish folder in release mode
RUN dotnet publish ./src/IdentityService/IdentityService.csproj -c Release -o /app/publish

# runs the above published version of the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.dll"]
