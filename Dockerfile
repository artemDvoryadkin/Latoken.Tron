FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp/049_Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp.csproj Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp/
COPY latoken.CurrencyProvider.Protocol.Tron/200_Latoken.CurrencyProvider.Protocol.Tron.csproj latoken.CurrencyProvider.Protocol.Tron/
COPY Latoken.CurrencyProvider.Common/300_Latoken.CurrencyProvider.Common.csproj Latoken.CurrencyProvider.Common/
RUN dotnet restore Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp/049_Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp.csproj
COPY . .
WORKDIR /src/Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp
RUN dotnet build 049_Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish 049_Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "049_Latoken.CurrencyProvider.Protocol.Tron.JsonRpcApp.dll"]
