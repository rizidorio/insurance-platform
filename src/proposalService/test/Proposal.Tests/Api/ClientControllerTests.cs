using FluentAssertions;
using Insurence.Platform.Common.Wrappers;
using Microsoft.AspNetCore.Mvc.Testing;
using ProposalService.Application.DataTransferObjects.Responses;
using System.Net.Http.Json;

namespace Proposal.Tests.Api;

public sealed class ClientControllerTests(
    WebApplicationFactory<ProposalService.Api.Program> factory) : IClassFixture<WebApplicationFactory<ProposalService.Api.Program>>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task CreateClient_ShouldReturnSuccess_WhenValidDataAreProvided()
    {
        // Arrange
        var clientRequest = new
        {
            Name = "Test Client",
            Email = "test@test.com",
            DocumentNumber = "87514883533",
            BirthDate = new DateTime(1990, 1, 1)
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/client", clientRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var clientResponse = await response.Content.ReadFromJsonAsync<ResponseDefault<ClientResponse>>();

        clientResponse.Should().NotBeNull();
        clientResponse!.Success.Should().BeTrue();
        clientResponse.Message.Should().Be("Cliente criado com sucesso.");
    }
}
