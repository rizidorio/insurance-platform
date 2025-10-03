using FluentAssertions;
using Insurence.Platform.Common.Notification.Enums;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Tests;

public class NotificationTests
{
    [Fact]
    public void CreateNotification_WithValidParameters_ShouldCreateNotification()
    {
        // Arrange
        var type = NotificationType.ApproveProposal;
        var channel = NotificationChannel.Email;
        var subject = "Test Subject";
        var body = "Test Body";
        var email = "test@test.com";

        // Act
        var notification = Notification.Create(type, channel, subject, body, email: email);

        // Assert
        notification.Should().NotBeNull();
        notification.Type.Should().Be(type);
        notification.Channel.Should().Be(channel);
        notification.Subject.Should().Be(subject);
        notification.Body.Should().Be(body);
        notification.Email.Should().Be(email);
        notification.Status.Should().Be(NotificationStatus.Pending);
    }

    [Fact]
    public void MarkAsSent_ShouldUpdateStatusToSent()
    {
        // Arrange
        var type = NotificationType.ApproveProposal;
        var channel = NotificationChannel.Email;
        var subject = "Test Subject";
        var body = "Test Body";
        var email = "test@test.com";
        var notification = Notification.Create(type, channel, subject, body, email: email);

        // Act
        notification.MarkAsSent();

        // Assert
        notification.Status.Should().Be(NotificationStatus.Sent);
        notification.SendError.Should().BeNull();
    }

    [Fact]
    public void MarkAsFailed_ShouldUpdateStatusToFailedAndSetErrorMessage()
    {
        // Arrange
        var type = NotificationType.ApproveProposal;
        var channel = NotificationChannel.Email;
        var subject = "Test Subject";
        var body = "Test Body";
        var email = "test@test.com";
        var notification = Notification.Create(type, channel, subject, body, email: email);
        var errorMessage = "Falha ao enviar notificação";

        // Act
        notification.MarkAsFailed(errorMessage);

        // Assert
        notification.Status.Should().Be(NotificationStatus.Failed);
        notification.SendError.Should().Be(errorMessage);
    }

    [Theory]
    [InlineData(NotificationType.ApproveProposal, NotificationChannel.Email, "Test Suject", "Test Body", null, "+5521999995555")]
    [InlineData(NotificationType.CreateContract, NotificationChannel.SMS, "Test Suject", "Test Body", "test@test.com", null)]
    public void CreateNotification_WithInvalidParameters_ShouldThrowArgumentException(
        NotificationType type,
        NotificationChannel channel,
        string subject,
        string body,
        string? email,
        string? phoneNumber)
    {
        // Act
        Action act = () => Notification.Create(type, channel, subject, body, email, phoneNumber);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}