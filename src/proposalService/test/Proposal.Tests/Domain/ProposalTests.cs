using FluentAssertions;
using ProposalService.Domain.Enums;

namespace Proposal.Tests.Domain;

public sealed class ProposalTests
{
    [Fact]
    public void CreateProposal_ShouldReturnProposalInstance_WhenValidParametersAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = InsuranceType.Life;
        var amount = 10000m;

        // Act
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);

        // Assert
        proposal.Should().NotBeNull();
        proposal.ClientId.Should().Be(clientId);
        proposal.InsuranceType.Should().Be(insuranceType);
        proposal.Amount.Should().Be(amount);
    }

    [Fact]
    public void ApproveProposal_ShouldReturnApproveProposalInstance__WhenValidParametersAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = InsuranceType.Life;
        var amount = 10000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        // Act
        proposal.Approve();

        // Assert
        proposal.Should().NotBeNull();
        proposal.Status.Should().Be(ProposalStatus.Approved);
    }

    [Fact]
    public void RejectProposal_ShouldReturnRejectProposalInstance__WhenValidParametersAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = InsuranceType.Life;
        var amount = 10000m;
        var rejectReason = "Reason";
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);

        // Act
        proposal.Reject(rejectReason);

        // Assert
        proposal.Should().NotBeNull();
        proposal.Status.Should().Be(ProposalStatus.Rejected);
        proposal.RejectReason.Should().Be(rejectReason);
    }
}
