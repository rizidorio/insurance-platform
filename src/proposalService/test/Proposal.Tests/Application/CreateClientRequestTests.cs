using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProposalService.Domain.Interfaces.Repositories;
using ProposalService.Domain.Interfaces.Services;

namespace Proposal.Tests.Application;

public class CreateClientRequestTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IClientValidation> _clientValidationMock;
    private readonly Mock<ILogger<ProposalService.Application.Services.ClientService>> _loggerMock;
    private readonly ProposalService.Application.Services.ClientService _clientServiceMock;

    public CreateClientRequestTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _clientValidationMock = new Mock<IClientValidation>();
        _loggerMock = new Mock<ILogger<ProposalService.Application.Services.ClientService>>();

        _clientServiceMock = new ProposalService.Application.Services.ClientService(
            clientValidation: _clientValidationMock.Object,
            clientRepository: _clientRepositoryMock.Object,
            logger: _loggerMock.Object);
    }

    [Fact]
    public async Task CreateClient_ShouldReturnSuccess_WhenValidDataAreProvided()
    {
        // Arrange
        var name = "Test Client";
        var email = "test@test.com";
        var documentNumber = "87514883533";
        var birthDate = new DateTime(1990, 1, 1);

        var request = new ProposalService.Application.DataTransferObjects.Requests.CreateClientRequest(
            Name: name,
            Email: email,
            DocumentNumber: documentNumber,
            BirthDate: birthDate);

        // Act
        var response = await _clientServiceMock.CreateAsync(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Cliente criado com sucesso.");
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().NotBe(Guid.Empty);
        response.Data.Name.Should().Be(name);
        response.Data.Email.Should().Be(email);
        response.Data.DocumentNumber.Should().Be(documentNumber);
        response.Data.BirthDate.Should().Be(birthDate);
    }
}
