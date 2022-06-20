#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim-arm32v7 AS base
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-amd64 AS build
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pi.HifiControl.Blazor/Server/Pi.HifiControl.Blazor.Server.csproj", "Pi.HifiControl.Blazor/Server/"]
RUN dotnet restore "Pi.HifiControl.Blazor/Server/Pi.HifiControl.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Pi.HifiControl.Blazor/Server"
RUN dotnet build "Pi.HifiControl.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pi.HifiControl.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pi.HifiControl.Blazor.Server.dll"]
