using FluentAssertions;
using HiringService.Domain.Entities;
using HiringService.Domain.Services;

namespace Hiring.Tests.Domain;

public sealed class ContractDomainServiceTests
{
    [Fact]
    public void CreateContract_ShouldReturnDomainException_WhenInvalidDatesAreProvided()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var effectiveDateStart = DateTime.UtcNow;
        var effectiveDateEnd = effectiveDateStart.AddYears(-1);
        var contract = Contract.Create(
            proposalId: proposalId, 
            clientId: clientId, 
            effectiveDateStart: effectiveDateStart, 
            effectiveDateEnd: effectiveDateEnd);

        var contractValidation = new ContractValidation();

        // Act
        Action act = () => contractValidation.Validate(contract);

        // Assert
        act.Should().Throw<HiringService.Domain.Exceptions.DomainException>()
            .WithMessage("Data de término da vigência deve ser posterior à data de início.");
    }

    [Fact]
    public void CreateContract_ShouldReturnValidContract_WhenValidDataIsProvided()
    {
        // Arrange
        var proposalId = Guid.Parse("86a88f85-e89a-4882-9ffe-bf8546f10c38");
        var clientId = Guid.Parse("1be65af4-e7fc-46a3-acd7-077972c183b9");
        var effectiveDateStart = DateTime.UtcNow;
        var effectiveDateEnd = effectiveDateStart.AddYears(1);
        var contract = Contract.Create(
            proposalId: proposalId, 
            clientId: clientId, 
            effectiveDateStart: effectiveDateStart, 
            effectiveDateEnd: effectiveDateEnd);
        var contractValidation = new ContractValidation();

        // Act
        Action act = () => contractValidation.Validate(contract);

        // Assert
        act.Should().NotThrow();
        contract.ProposalId.Should().Be(proposalId);
        contract.ClientId.Should().Be(clientId);
        contract.EffectiveDateStart.Should().Be(effectiveDateStart);
        contract.EffectiveDateEnd.Should().Be(effectiveDateEnd);
        contract.PolicyNumber.Should().NotBeNullOrWhiteSpace();
        contract.PolicyNumber.Length.Should().BeLessThanOrEqualTo(50);
    }
}
