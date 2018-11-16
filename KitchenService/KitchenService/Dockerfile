FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["KitchenService/KitchenService.csproj", "KitchenService/"]
RUN dotnet restore "KitchenService/KitchenService.csproj"
COPY . .
WORKDIR "/src/KitchenService"
RUN dotnet build "KitchenService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KitchenService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KitchenService.dll"]