using FluentAssertions;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Exceptions;

namespace Proposal.Tests.Domain;

public sealed class ClientTests
{
    [Fact]
    public void CreateClient_ShouldReturnClientInstance_WhenValidParametersAreProvided()
    {
        // Arrange
        var name = "Test Client";
        var email = "test@test.com";
        var documentNumber = "87514883533";
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        var client = Client.Create(
            name: name,
            documentNumber: documentNumber,
            email: email,
            birthDate: birthDate);

        // Assert
        client.Should().NotBeNull();
        client.Name.ToString().Should().Be(name);
        client.Email.ToString().Should().Be(email);
        client.DocumentNumber.ToString().Should().Be(documentNumber);
        client.BirthDate.Should().Be(birthDate);
        client.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void CreateClient_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var name = "";
        var email = "test@test.com";
        var documentNumber = "87514883533";
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        Action act = () => Client.Create(
            name: name,
            documentNumber: documentNumber,
            email: email,
            birthDate: birthDate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome não pode ser nulo ou vazio");
    }

    [Fact]
    public void CreateClient_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Arrange
        var name = "Test Client";
        var email = "invalid-email";
        var documentNumber = "87514883533";
        var birthDate = new DateTime(1990, 1, 1);
        
        // Act
        Action act = () => Client.Create(
            name: name,
            documentNumber: documentNumber,
            email: email,
            birthDate: birthDate);
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("E-mail com formato inválido");
    }

    [Fact]
    public void CreateClient_ShouldThrowArgumentException_WhenDocumentNumberIsInvalid()
    {
        // Arrange
        var name = "Test Client";
        var email = "test@test.com";
        var documentNumber = "12345678999";
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        Action act = () => Client.Create(
            name: name,
            documentNumber: documentNumber,
            email: email,
            birthDate: birthDate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Número do documento deve ser um CPF ou CNPJ válido");
    }
}
