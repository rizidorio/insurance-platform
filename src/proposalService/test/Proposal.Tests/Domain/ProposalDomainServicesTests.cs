using FluentAssertions;

namespace Proposal.Tests.Domain;

public sealed class ProposalDomainServicesTests
{
    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidAmountAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Life;
        var amount = 0m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();

        // Act
        Action act = () => proposalValidation.Validate(proposal);

        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Valor da proposta deve ser maior que zero.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidClientIdAreProvided()
    {
        // Arrange
        var clientId = 0;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Life;
        var amount = 10000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Identificador do cliente inválido.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidInsuranceTypeAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = (ProposalService.Domain.Enums.InsuranceType)999;
        var amount = 10000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Tipo de seguro inválido.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidInsuranceTypeNoneAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.None;
        var amount = 10000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Tipo de seguro não pode ser 'Desconhecido'.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidAmountForLifeInsuranceTypeAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Life;
        var amount = 200000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Valor máximo para seguro de vida é 100.000.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidAmountForVehicleInsuranceTypeAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Vehicle;
        var amount = 60000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Valor máximo para seguro de veículo é 50.000.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidAmountForPropertyInsuranceTypeAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Property;
        var amount = 300000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Valor máximo para seguro de propriedade é 200.000.");
    }

    [Fact]
    public void CreateProposal_ShouldReturnDomainException_WhenInvalidAmountForHealthInsuranceTypeAreProvided()
    {
        // Arrange
        var clientId = 1;
        var insuranceType = ProposalService.Domain.Enums.InsuranceType.Health;
        var amount = 200000m;
        var proposal = ProposalService.Domain.Entities.Proposal.Create(
            clientId: clientId,
            insuranceType: insuranceType,
            amount: amount);
        var proposalValidation = new ProposalService.Domain.Services.ProposalValidation();
        // Act
        Action act = () => proposalValidation.Validate(proposal);
        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Valor máximo para seguro de saúde é 150.000.");
    }
}
