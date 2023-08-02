FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RemindMe.Reminder/RemindMe.Reminder.csproj", "RemindMe.Reminder/"]
COPY ["RemindMe.Contracts/RemindMe.Contracts.csproj", "RemindMe.Contracts/"]
RUN dotnet restore "RemindMe.Reminder/RemindMe.Reminder.csproj"
COPY . .
WORKDIR "/src/RemindMe.Reminder"
RUN dotnet build "RemindMe.Reminder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RemindMe.Reminder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "RemindMe.Reminder.dll"]
