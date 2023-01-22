FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fibonacci.BackgroundService/Fibonacci.BackgroundService.csproj", "Fibonacci.BackgroundService/"]
RUN dotnet restore "Fibonacci.BackgroundService/Fibonacci.BackgroundService.csproj"
COPY . .
WORKDIR "/src/Fibonacci.BackgroundService"
RUN dotnet build "Fibonacci.BackgroundService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fibonacci.BackgroundService.csproj" -c Release -o /app/publish

FROM base AS final
COPY /Fibonacci.BackgroundService/wait-for-it.sh .
WORKDIR /app
COPY --from=publish /app/publish .
RUN chmod +x wait-for-it.sh
ENTRYPOINT ["dotnet", "Fibonacci.BackgroundService.dll"]