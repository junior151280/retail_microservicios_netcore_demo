FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Pedidos.API.csproj", "./"]
RUN dotnet restore "Pedidos.API.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Pedidos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pedidos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedidos.API.dll"]
