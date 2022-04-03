#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Lab2work/Lab2work.csproj", "Lab2work/"]
RUN dotnet restore "Lab2work/Lab2work.csproj"
COPY . .
WORKDIR "/src/Lab2work"
RUN dotnet build "Lab2work.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lab2work.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab2work.dll"]