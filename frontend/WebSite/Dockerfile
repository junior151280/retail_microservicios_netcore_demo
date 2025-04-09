FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["WebSite.csproj", "WebSite/"]
RUN dotnet restore "WebSite/WebSite.csproj"
COPY . ./WebSite/
WORKDIR "/src/WebSite"
RUN dotnet build "WebSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSite.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSite.dll"]
