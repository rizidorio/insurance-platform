using FluentAssertions;
using HiringService.Domain.Entities;

namespace Hiring.Tests.Domain;

public sealed class ContractTests
{
    [Fact]
    public void CreateContract_ShouldReturnContractInstance_WhenValidParametersAreProvided()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var effectiveDateStart = DateTime.UtcNow;
        var effectiveDateEnd = effectiveDateStart.AddYears(1);

        // Act
        var contract = Contract.Create(
            proposalId: proposalId, 
            clientId: clientId, 
            effectiveDateStart: effectiveDateStart, 
            effectiveDateEnd: effectiveDateEnd);

        // Assert
        contract.Should().NotBeNull();
        contract.ClientId.Should().Be(clientId);
        contract.ProposalId.Should().Be(proposalId);
        contract.EffectiveDateStart.Should().Be(effectiveDateStart);
        contract.EffectiveDateEnd.Should().Be(effectiveDateEnd);
    }

    [Fact]
    public void DeactivateContract_ShouldSetIsActiveToFalseAndUpdateTimestamp_WhenCalled()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var effectiveDateStart = DateTime.UtcNow;
        var effectiveDateEnd = effectiveDateStart.AddYears(1);
        var contract = Contract.Create(
            proposalId: proposalId, 
            clientId: clientId, 
            effectiveDateStart: effectiveDateStart, 
            effectiveDateEnd: effectiveDateEnd);

        // Act
        contract.Deactivate();

        // Assert
        contract.IsActive.Should().BeFalse();
        contract.UpdatedAt.Should().NotBeNull();
        contract.UpdatedAt.Should().BeAfter(contract.CreatedAt);
    }

    [Fact]
    public void ActivateContract_ShouldNotChangeState_WhenAlreadyInactive()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var effectiveDateStart = DateTime.UtcNow;
        var effectiveDateEnd = effectiveDateStart.AddYears(1);
        var contract = Contract.Create(
            proposalId: proposalId, 
            clientId: clientId, 
            effectiveDateStart: effectiveDateStart, 
            effectiveDateEnd: effectiveDateEnd);
        contract.Deactivate();

        // Act
        contract.Activate();
        var updatedAtAfterActivate = contract.UpdatedAt;
        // Assert
        contract.IsActive.Should().BeTrue();
        contract.UpdatedAt.Should().Be(updatedAtAfterActivate);
    }
}
