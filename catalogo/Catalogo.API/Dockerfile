FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Catalogo.API.csproj", "./"]
RUN dotnet restore "Catalogo.API.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Catalogo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalogo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalogo.API.dll"]
