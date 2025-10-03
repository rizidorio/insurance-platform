# Build context: raiz do projeto (insurance-platform)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/notificationService/src/NotificationService.Worker

COPY src/notificationService/src/NotificationService.Worker/NotificationService.Worker.csproj .
COPY src/notificationService/src/NotificationService.Application/NotificationService.Application.csproj ../NotificationService.Application/
COPY src/notificationService/src/NotificationService.Domain/NotificationService.Domain.csproj ../NotificationService.Domain/
COPY src/notificationService/src/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj ../NotificationService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/Insurence.Platform.Common.csproj ../../../shared/Insurence.Platform.Common/

RUN dotnet restore NotificationService.Worker.csproj

COPY src/notificationService/src/NotificationService.Worker/. .
COPY src/notificationService/src/NotificationService.Application/. ../NotificationService.Application/
COPY src/notificationService/src/NotificationService.Domain/. ../NotificationService.Domain/
COPY src/notificationService/src/NotificationService.Infrastructure/. ../NotificationService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/. ../../../shared/Insurence.Platform.Common/

RUN dotnet build "NotificationService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotificationService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Worker.dll"]