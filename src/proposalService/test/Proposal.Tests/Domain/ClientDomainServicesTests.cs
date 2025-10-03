using FluentAssertions;
using ProposalService.Domain.Entities;

namespace Proposal.Tests.Domain;

public sealed class ClientDomainServicesTests
{
    [Fact]
    public void ClientValidation_ShouldThrowDomainException_WhenBirthDateIsInTheFuture()
    {
        // Arrange
        var name = "Test Client";
        var email = "test@test.com";
        var documentNumber = "87514883533";
        var birthDate = DateTime.UtcNow.AddDays(1);

        var client = Client.Create(
            name: name,
            documentNumber: documentNumber,
            email: email,
            birthDate: birthDate);

        var clientValidation = new ProposalService.Domain.Services.ClientValidation();

        // Act
        Action act = () => clientValidation.Validate(client);

        // Assert
        act.Should().Throw<ProposalService.Domain.Exceptions.DomainException>()
            .WithMessage("Data de nascimento não pode ser no futuro.");
    }

}
