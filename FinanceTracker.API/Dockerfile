FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FinanceTracker.sln ./
COPY FinanceTracker.API/FinanceTracker.API.csproj FinanceTracker.API/
COPY FinanceTracker.BusinessLogic/FinanceTracker.BusinessLogic.csproj FinanceTracker.BusinessLogic/
COPY FinanceTracker.Contracts/FinanceTracker.Contracts.csproj FinanceTracker.Contracts/
COPY FinanceTracker.Core/FinanceTracker.Core.csproj FinanceTracker.Core/
COPY FinanceTracker.DataAccess/FinanceTracker.DataAccess.csproj FinanceTracker.DataAccess/

RUN dotnet restore FinanceTracker.API/FinanceTracker.API.csproj

COPY . .
RUN dotnet publish FinanceTracker.API/FinanceTracker.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceTracker.API.dll"]
