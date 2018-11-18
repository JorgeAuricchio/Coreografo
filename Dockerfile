FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY AccountingService/AccountingService/AccountingService.csproj AccountingService/AccountingService/
COPY AMQ/AMQ/AMQ.csproj AMQ/AMQ/
COPY ElasticSearch/ElasticSearch.csproj ElasticSearch/
RUN dotnet restore AccountingService/AccountingService/AccountingService.csproj
COPY . ./
WORKDIR /src/AccountingService
RUN dotnet build AccountingService/AccountingService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccountingService/AccountingService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccountingService.dll"]