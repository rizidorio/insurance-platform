# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/hiringService/src/HiringService.Api

# Copia todos os arquivos .csproj das dependências
COPY src/hiringService/src/HiringService.Api/HiringService.Api.csproj .
COPY src/hiringService/src/HiringService.Application/HiringService.Application.csproj ../HiringService.Application/
COPY src/hiringService/src/HiringService.Domain/HiringService.Domain.csproj ../HiringService.Domain/
COPY src/hiringService/src/HiringService.Infrastructure/HiringService.Infrastructure.csproj ../HiringService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/Insurence.Platform.Common.csproj ../../../shared/Insurence.Platform.Common/

# Restaura as dependências
RUN dotnet restore HiringService.Api.csproj

# Copia o restante do código
COPY src/hiringService/src/HiringService.Api/. .
COPY src/hiringService/src/HiringService.Application/. ../HiringService.Application/
COPY src/hiringService/src/HiringService.Domain/. ../HiringService.Domain/
COPY src/hiringService/src/HiringService.Infrastructure/. ../HiringService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/. ../../../shared/Insurence.Platform.Common/

RUN dotnet build "HiringService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HiringService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiringService.Api.dll"]ENTRYPOINT ["dotnet", "HiringService.Api.dll"]