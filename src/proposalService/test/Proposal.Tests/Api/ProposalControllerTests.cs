using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProposalService.Domain.Enums;
using System.Net.Http.Json;


namespace Proposal.Tests.Api;

public sealed class ProposalControllerTests(
    WebApplicationFactory<ProposalService.Api.Program> factory) : IClassFixture<WebApplicationFactory<ProposalService.Api.Program>>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task GetProposals_ShouldReturnOk()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/proposal");
        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_CreateProposal_ShouldReturnCreated()
    {
        // Arrange
        var newProposal = new
        {
            ClientId = Guid.Parse("1be65af4-e7fc-46a3-acd7-077972c183b9"),
            Amount = 10000m,
            InsuranceType = InsuranceType.Life
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/proposal", newProposal);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_CreateProposal_ShouldReturnBadRequest_WhenInvalidData()
    {
        // Arrange
        var newProposal = new
        {
            ClientId = Guid.Empty, // Invalid ClientId
            Amount = -10000m, // Invalid Amount
            InsuranceType = "InvalidType" // Invalid InsuranceType
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/proposal", newProposal);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
