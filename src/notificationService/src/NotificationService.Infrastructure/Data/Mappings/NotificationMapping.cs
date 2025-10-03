using Insurence.Platform.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Data.Mappings;

/// <summary>
/// Fornece a configuração do Entity Framework Core para o tipo de entidade Notification.
/// </summary>
/// <remarks>Esta classe define o mapeamento do esquema do banco de dados, restrições e configurações de propriedades para
/// entidades Notification usando a interface IEntityTypeConfiguration. Destina-se ao uso na camada de acesso a dados da aplicação
/// para garantir o mapeamento consistente entre o modelo de domínio Notification e a estrutura subjacente do banco de dados.</remarks>
internal sealed class NotificationMapping : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable($"{nameof(Notification)}s".ToSnakeCase());

        builder.HasKey(n => n.Id)
            .HasName($"pk{nameof(Notification)}".ToSnakeCase());

        builder.Property(n => n.Id)
            .HasColumnName(nameof(Notification.Id).ToSnakeCase());

        builder.Property(n => n.ExternalId)
            .HasColumnName(nameof(Notification.ExternalId).ToSnakeCase())
            .IsRequired();

        builder.HasIndex(n => n.ExternalId)
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Notification)}{nameof(Notification.ExternalId)}".ToSnakeCase());

        builder.Property(n => n.Type)
            .HasConversion<string>()
            .HasColumnName(nameof(Notification.Type).ToSnakeCase())
            .IsRequired();

        builder.Property(n => n.Channel)
            .HasConversion<string>()
            .HasColumnName(nameof(Notification.Channel).ToSnakeCase())
            .IsRequired();

        builder.Property(n => n.Subject)
            .HasColumnName(nameof(Notification.Subject).ToSnakeCase())
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Body)
            .HasColumnName(nameof(Notification.Body).ToSnakeCase())
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(n => n.Email)
            .HasColumnName(nameof(Notification.Email).ToSnakeCase())
            .HasMaxLength(100);

        builder.Property(n => n.PhoneNumber)
            .HasColumnName(nameof(Notification.PhoneNumber).ToSnakeCase())
            .HasMaxLength(20);

        builder.Property(n => n.Status)
            .HasConversion<string>()
            .HasColumnName(nameof(Notification.Status).ToSnakeCase())
            .IsRequired();

        builder.Property(n => n.SendError)
            .HasColumnName(nameof(Notification.SendError).ToSnakeCase())
            .HasMaxLength(100);

        builder.Property(n => n.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName(nameof(Notification.CreatedAt).ToSnakeCase())
            .IsRequired();

        builder.Property(n => n.UpdatedAt)
            .HasColumnName(nameof(Notification.UpdatedAt).ToSnakeCase());

        builder.Property(n => n.SendIn)
            .HasColumnName(nameof(Notification.SendIn).ToSnakeCase());

    }
}
