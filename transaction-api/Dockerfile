FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY src/Transactions.API/*.csproj ./src/Transactions.API/
COPY src/Transactions.Application/*.csproj ./src/Transactions.Application/
COPY src/Transactions.Infrastructure.Data/*.csproj ./src/Transactions.Infrastructure.Data/
COPY src/Transactions.Infrastructure.Core/*.csproj ./src/Transactions.Infrastructure.Core/
COPY src/Transaction.Infrastructure.CrossCutting.Kafka/*.csproj ./src/Transaction.Infrastructure.CrossCutting.Kafka/
COPY src/Transactions.Domain/*.csproj ./src/Transactions.Domain/

RUN dotnet restore ./src/Transactions.API/Transactions.API.csproj

COPY . ./
RUN dotnet publish ./src/Transactions.API/Transactions.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Transactions.API.dll"]