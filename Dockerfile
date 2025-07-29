#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG version
WORKDIR /src
COPY ["/", "/src"]
RUN dotnet build Garage.sln -c Release
RUN dotnet test --no-build -c Release
WORKDIR "/src/src/Garage"

FROM build AS publish
RUN dotnet publish "Garage.csproj" -c Release --no-build -o /app/publish /p:UseAppHost=false
# Overwrite version.json in the build output with the value from the build arg
RUN echo "{ \"Settings\": { \"version\": \"$version\" }}" > /app/publish/version.json

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Garage.dll"]