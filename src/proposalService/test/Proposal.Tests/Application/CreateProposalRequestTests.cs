using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProposalService.Application.DataTransferObjects.Requests;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Extensions;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Domain.Interfaces.Services;

namespace Proposal.Tests.Application;

public sealed class CreateProposalRequestTests
{
    private readonly Mock<IProposalRepository> _proposalRepositoryMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IClientValidation> _clientValidationMock;
    private readonly Mock<IProposalValidation> _proposalValidationMock;
    private readonly Mock<IRiskAnalysisService> _riskAnalysisServiceMock;
    private readonly Mock<ILogger<ProposalService.Application.Services.ProposalService>> _loggerMock;
    private readonly ProposalService.Application.Services.ProposalService _proposalServiceMock;

    public CreateProposalRequestTests()
    {
        _proposalRepositoryMock = new Mock<IProposalRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _clientValidationMock = new Mock<IClientValidation>();
        _proposalValidationMock = new Mock<IProposalValidation>();
        _riskAnalysisServiceMock = new Mock<IRiskAnalysisService>();
        _loggerMock = new Mock<ILogger<ProposalService.Application.Services.ProposalService>>();

        _proposalServiceMock = new ProposalService.Application.Services.ProposalService(
            proposalValidation: _proposalValidationMock.Object,
            clientValidation: _clientValidationMock.Object,
            riskAnalysisService: _riskAnalysisServiceMock.Object,
            clientRepository: _clientRepositoryMock.Object,
            proposalRepository: _proposalRepositoryMock.Object,
            logger: _loggerMock.Object);
    }

    [Fact]
    public async Task CreateProposal_ShouldReturnSuccess_WhenValidDataAreProvided()
    {
        // Arrange
        var client = Client.Create(
            name: "Test Client",
            documentNumber: "87514883533",
            email: "test@test.com",
            birthDate: new DateTime(1990, 1, 1));
        _clientRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(client.ExternalId, CancellationToken.None))
            .ReturnsAsync(client);

        var insuranceType = InsuranceType.Life;
        var amount = 10000m;
        var request = new CreateProposalRequest(
            ClientId: client.ExternalId,
            ClientName: null,
            ClientEmail: null,
            ClientDocumentNumber: null,
            ClientBirthDate: null,
            Amount: amount,
            InsuranceType: insuranceType);

        // Act
        var response = await _proposalServiceMock.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Proposta criada com sucesso.");
        response.Data.Should().NotBeNull();
        response.Data.Client.Should().NotBeNull();
        response.Data.Amount.Should().Be(amount);
        response.Data.InsuranceType.Should().Be(insuranceType.ToDisplayName());
    }

    [Fact]
    public async Task CreateProposal_ShouldReturnFail_WhenClientDoesNotExistAndNoClientDataAreProvided()
    {
        // Arrange
        var nonExistentClientId = Guid.NewGuid();
        _clientRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(nonExistentClientId, CancellationToken.None))
            .ReturnsAsync((Client?)null);

        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Life;
        var amount = 10000m;
        var request = new CreateProposalRequest(
            ClientId: nonExistentClientId,
            ClientName: null,
            ClientEmail: null,
            ClientDocumentNumber: null,
            ClientBirthDate: null,
            Amount: amount,
            InsuranceType: insuranceType);

        // Act
        var response = await _proposalServiceMock.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be("Cliente não encontrado.");
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task CreateProposal_ShouldReturnFail_WhenClientDoesNotExistAndIncompleteClientDataAreProvided()
    {
        // Arrange
        var nonExistentClientId = Guid.NewGuid();
        _clientRepositoryMock.Setup(repo => repo.GetByExternalIdAsync(nonExistentClientId, CancellationToken.None))
            .ReturnsAsync((Client?)null);
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Life;
        var amount = 10000m;
        var request = new CreateProposalRequest(
            ClientId: nonExistentClientId,
            ClientName: "Test Client",
            ClientEmail: null,
            ClientDocumentNumber: "87514883533",
            ClientBirthDate: null,
            Amount: amount,
            InsuranceType: insuranceType);
        // Act
        var response = await _proposalServiceMock.CreateAsync(request, CancellationToken.None);
        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
    }
}
