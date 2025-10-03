using FluentAssertions;
using Insurence.Platform.Common.Messaging.RabbitMq.Interfaces.Notification;
using Microsoft.Extensions.Logging;
using Moq;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Extensions;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Domain.Interfaces.Services;

namespace Proposal.Tests.Application;

public sealed class ApproveProposalRequestTest
{
    private readonly Mock<IProposalRepository> _proposalRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IClientValidation> _clientValidationMock;
    private readonly Mock<IProposalValidation> _proposalValidationMock;
    private readonly Mock<IRiskAnalysisService> _riskAnalysisServiceMock;
    private readonly Mock<INotificationMessagePublish> _notificationMessagePublishMock;
    private readonly Mock<ILogger<ProposalService.Application.Services.ProposalService>> _loggerMock;
    private readonly ProposalService.Application.Services.ProposalService _proposalServiceMock;

    public ApproveProposalRequestTest()
    {
        _proposalRepositoryMock = new Mock<IProposalRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _clientValidationMock = new Mock<IClientValidation>();
        _proposalValidationMock = new Mock<IProposalValidation>();
        _riskAnalysisServiceMock = new Mock<IRiskAnalysisService>();
        _notificationMessagePublishMock = new Mock<INotificationMessagePublish>();
        _loggerMock = new Mock<ILogger<ProposalService.Application.Services.ProposalService>>();

        _proposalServiceMock = new ProposalService.Application.Services.ProposalService(
            proposalValidation: _proposalValidationMock.Object,
            clientValidation: _clientValidationMock.Object,
            riskAnalysisService: _riskAnalysisServiceMock.Object,
            clientRepository: _clientRepositoryMock.Object,
            proposalRepository: _proposalRepositoryMock.Object,
            notificationMessagePublish: _notificationMessagePublishMock.Object,
            logger: _loggerMock.Object);
    }

    [Fact]
    public async Task ApproveProposal_ShouldReturnSuccess_WhenValidDataAreProvided()
    {
        // Arrange
        var client = ProposalService.Domain.Entities.Client.Create(
            name: "Test Client",
            documentNumber: "87514883533",
            email: "test@test.com",
            birthDate: new DateTime(1990, 1, 1));
        _clientRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(client.ExternalId, CancellationToken.None))
            .ReturnsAsync(client);

        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: client.Id,
            insuranceType: InsuranceType.Life,
            amount: 10000m);

        _proposalRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(proposal.ExternalId, CancellationToken.None))
            .ReturnsAsync(proposal);
        
        proposal.SetClient(client);
        var request = new ProposalService.Application.DataTransferObjects.Requests.ApproveProposalRequest(
            ProposalId: proposal.ExternalId);

        // Act
        var result = await _proposalServiceMock.ApproveAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(ProposalStatus.Approved.ToDisplayName());
    }

    [Fact]
    public async Task ApproveProposal_ShouldReturnFail_WhenInvalidProposalIdAreProvided()
    {
        // Arrange
        var request = new ProposalService.Application.DataTransferObjects.Requests.ApproveProposalRequest(
            ProposalId: Guid.NewGuid());

        _proposalRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(request.ProposalId, CancellationToken.None))
            .ReturnsAsync((ProposalService.Domain.Entities.Proposal?)null);

        // Act
        var result = await _proposalServiceMock.ApproveAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Proposta não encontrada.");
    }

    [Fact]
    public async Task ApproveProposal_ShouldReturnFail_WhenProposalIsNotInAnalysisStatus()
    {
        // Arrange
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: 1,
            insuranceType: InsuranceType.Life,
            amount: 10000m);
        proposal.Approve();

        _proposalRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(proposal.ExternalId, CancellationToken.None))
            .ReturnsAsync(proposal);

        var request = new ProposalService.Application.DataTransferObjects.Requests.ApproveProposalRequest(
            ProposalId: proposal.ExternalId);

        // Act
        var result = await _proposalServiceMock.ApproveAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("A proposta deve estar em estado 'em analise' para ser aprovada.");
    }
}
