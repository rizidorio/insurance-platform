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
WORKDIR /src/proposalService/src/ProposalService.Api

# Copia todos os arquivos .csproj das dependências
COPY src/proposalService/src/ProposalService.Api/ProposalService.Api.csproj .
COPY src/proposalService/src/ProposalService.Application/ProposalService.Application.csproj ../ProposalService.Application/
COPY src/proposalService/src/ProposalService.Domain/ProposalService.Domain.csproj ../ProposalService.Domain/
COPY src/proposalService/src/ProposalService.Infrastructure/ProposalService.Infrastructure.csproj ../ProposalService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/Insurence.Platform.Common.csproj ../../../shared/Insurence.Platform.Common/

# Restaura as dependências
RUN dotnet restore ProposalService.Api.csproj

# Copia o restante do código
COPY src/proposalService/src/ProposalService.Api/. .
COPY src/proposalService/src/ProposalService.Application/. ../ProposalService.Application/
COPY src/proposalService/src/ProposalService.Domain/. ../ProposalService.Domain/
COPY src/proposalService/src/ProposalService.Infrastructure/. ../ProposalService.Infrastructure/
COPY src/shared/Insurence.Platform.Common/. ../../../shared/Insurence.Platform.Common/

RUN dotnet build "ProposalService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProposalService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProposalService.Api.dll"]ENTRYPOINT ["dotnet", "ProposalService.Api.dll"]