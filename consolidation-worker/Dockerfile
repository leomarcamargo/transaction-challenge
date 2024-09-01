FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/Consolidations.Worker/*.csproj ./src/Consolidations.Worker/
COPY src/Consolidations.Domain/*.csproj ./src/Consolidations.Domain/
COPY src/Consolidations.Infrastructure.Data/*.csproj ./src/Consolidations.Infrastructure.Data/
COPY src/Consolidations.Infrastructure.CrossCutting.Redis/*.csproj ./src/Consolidations.Infrastructure.CrossCutting.Redis/
COPY src/Consolidations.Infrastructure.Core/*.csproj ./src/Consolidations.Infrastructure.Core/

RUN dotnet restore ./src/Consolidations.Worker/Consolidations.Worker.csproj

COPY . ./
RUN dotnet publish ./src/Consolidations.Worker/Consolidations.Worker.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Consolidations.Worker.dll"]