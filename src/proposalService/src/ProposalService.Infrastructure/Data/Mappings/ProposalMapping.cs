using Insurence.Platform.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProposalService.Domain.Entities;

namespace ProposalService.Infrastructure.Data.Mappings;

/// <summary>
/// Fornece a configuração do Entity Framework Core para o tipo de entidade Proposal.
/// </summary>
/// <remarks>Esta classe define o mapeamento do esquema do banco de dados, restrições e relacionamentos para entidades Proposal
/// usando a interface IEntityTypeConfiguration. Destina-se ao uso dentro da configuração do modelo do DbContext e
/// normalmente não é utilizada diretamente pelo código da aplicação.</remarks>
internal sealed class ProposalMapping : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.ToTable($"{nameof(Proposal)}s".ToSnakeCase());

        builder.HasKey(p => p.Id)
            .HasName($"pk{nameof(Proposal)}".ToSnakeCase());

        builder.Property(p => p.Id)
            .HasColumnName(nameof(Proposal.Id).ToSnakeCase());

        builder.Property(p => p.ExternalId)
            .HasColumnName(nameof(Proposal.ExternalId).ToSnakeCase())
            .IsRequired();

        builder.HasIndex(builder => builder.ExternalId)
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Proposal)}{nameof(Proposal.ExternalId)}".ToSnakeCase());

        builder.Property(p => p.ClientId)
            .HasColumnName(nameof(Proposal.ClientId).ToSnakeCase())
            .IsRequired();

        builder.HasOne(p => p.Client)
            .WithMany(x => x.Proposals)
            .HasForeignKey(p => p.ClientId)
            .HasConstraintName($"fk{nameof(Proposal)}{nameof(Proposal.ClientId)}".ToSnakeCase())
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Amount)
            .HasColumnName(nameof(Proposal.Amount).ToSnakeCase())
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .HasColumnName(nameof(Proposal.Status).ToSnakeCase())
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.InsuranceType)
            .HasColumnName(nameof(Proposal.InsuranceType).ToSnakeCase())
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.RejectReason)
            .HasColumnName(nameof(Proposal.RejectReason).ToSnakeCase())
            .HasMaxLength(255);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName(nameof(Proposal.CreatedAt).ToSnakeCase())
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName(nameof(Proposal.UpdatedAt).ToSnakeCase());

        builder.HasIndex(p => p.ClientId)
            .HasDatabaseName($"ix{nameof(Proposal)}{nameof(Proposal.ClientId)}".ToSnakeCase());

        builder.HasIndex(p => p.Status)
            .HasDatabaseName($"ix{nameof(Proposal)}{nameof(Proposal.Status)}".ToSnakeCase());

        builder.HasIndex(p => p.InsuranceType)
            .HasDatabaseName($"ix{nameof(Proposal)}{nameof(Proposal.InsuranceType)}".ToSnakeCase());

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName($"ix{nameof(Proposal)}{nameof(Proposal.CreatedAt)}".ToSnakeCase());
    }
}
