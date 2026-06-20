FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FinanceTracker.UI/FinanceTracker.UI.csproj FinanceTracker.UI/
COPY FinanceTracker.Contracts/FinanceTracker.Contracts.csproj FinanceTracker.Contracts/

RUN dotnet restore FinanceTracker.UI/FinanceTracker.UI.csproj

COPY . .
RUN dotnet publish FinanceTracker.UI/FinanceTracker.UI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceTracker.UI.dll"]
