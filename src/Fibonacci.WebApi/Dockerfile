FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fibonacci.WebApi/Fibonacci.WebApi.csproj", "Fibonacci.WebApi/"]
RUN dotnet restore "Fibonacci.WebApi/Fibonacci.WebApi.csproj"
COPY . .
WORKDIR "/src/Fibonacci.WebApi"
RUN dotnet build "Fibonacci.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fibonacci.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fibonacci.WebApi.dll"]
