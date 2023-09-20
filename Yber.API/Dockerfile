FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Yber.API/Yber.API.csproj", "Yber.API/"]
RUN dotnet restore "Yber.API/Yber.API.csproj"
COPY . .
WORKDIR "/src/Yber.API"
RUN dotnet build "Yber.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yber.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yber.API.dll"]
