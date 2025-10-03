using Insurence.Platform.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;

namespace ProposalService.Infrastructure.Data.Mappings;

/// <summary>
/// Fornece a configuração do Entity Framework Core para o tipo de entidade Client.
/// </summary>
/// <remarks>Esta classe define como a entidade Client é mapeada para o esquema do banco de dados, incluindo nome da tabela,
/// chaves primárias e únicas, configurações de propriedades e tipos agregados. Destina-se ao uso com o model builder do Entity Framework
/// Core e não deve ser utilizada diretamente no código da aplicação.</remarks>
internal sealed class ClientMapping : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable($"{nameof(Client)}s".ToSnakeCase());

        builder.HasKey(c => c.Id)
            .HasName($"pk{nameof(Client)}".ToSnakeCase());

        builder.Property(c => c.Id)
            .HasColumnName(nameof(Client.Id).ToSnakeCase());

        builder.Property(c => c.ExternalId)
            .HasColumnName(nameof(Client.ExternalId).ToSnakeCase())
            .IsRequired();

        builder.HasIndex(c => c.ExternalId)
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Client)}{nameof(Client.ExternalId)}".ToSnakeCase());

        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName(nameof(Client.Name).ToSnakeCase())
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(c => c.DocumentNumber, documentNumber =>
        {
            documentNumber.Property(d => d.Value)
                .HasColumnName(nameof(Client.DocumentNumber).ToSnakeCase())
                .HasMaxLength(20)
                .IsRequired();
            documentNumber.HasIndex(d => d.Value)
                .IsUnique()
                .HasDatabaseName($"ux{nameof(Client)}{nameof(Client.DocumentNumber)}".ToSnakeCase());
        });

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName(nameof(Client.Email).ToSnakeCase())
                .HasMaxLength(100);
            email.HasIndex(e => e.Value)
                .IsUnique()
                .HasDatabaseName($"ux{nameof(Client)}{nameof(Client.Email)}".ToSnakeCase());
        });

        builder.Property(c => c.BirthDate)
            .HasColumnName(nameof(Client.BirthDate).ToSnakeCase());

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName(nameof(Client.CreatedAt).ToSnakeCase())
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName(nameof(Client.UpdatedAt).ToSnakeCase());
    }
}
