using HiringService.Domain.Entities;
using Insurence.Platform.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiringService.Infrastructure.Data.Mappings;

/// <summary>
/// Configura o mapeamento da entidade <see cref="Contract"/> no modelo do Entity Framework.
/// </summary>
/// <remarks>
/// Esta configuração define nomes de tabelas, chaves primárias, mapeamento de propriedades, índices e restrições para a entidade <see cref="Contract"/>.
/// Deve ser registrada com o model builder do Entity Framework para garantir que o esquema do banco de dados corresponda ao modelo de domínio.
/// Esta classe é normalmente utilizada no método <c>OnModelCreating</c> de uma implementação de <c>DbContext</c>.
/// </remarks>
public sealed class ContractMapping : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable($"{nameof(Contract)}s".ToSnakeCase());

        builder.HasKey(c => c.Id)
            .HasName($"pk{nameof(Contract)}".ToSnakeCase());

        builder.Property(c => c.Id)
            .IsRequired()
            .HasColumnName(nameof(Contract.Id).ToSnakeCase());

        builder.Property(c => c.ExternalId)
            .IsRequired()
            .HasColumnName(nameof(Contract.ExternalId).ToSnakeCase());

        builder.Property(c => c.ProposalId)
            .IsRequired()
            .HasColumnName(nameof(Contract.ProposalId).ToSnakeCase());

        builder.Property(c => c.ClientId)
            .IsRequired()
            .HasColumnName(nameof(Contract.ClientId).ToSnakeCase());

        builder.Property(c => c.HiringDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired()
            .HasColumnName(nameof(Contract.HiringDate).ToSnakeCase());

        builder.Property(c => c.EffectiveDateStart)
            .IsRequired()
            .HasColumnName(nameof(Contract.EffectiveDateStart).ToSnakeCase());

        builder.Property(c => c.EffectiveDateEnd)
            .IsRequired()
            .HasColumnName(nameof(Contract.EffectiveDateEnd).ToSnakeCase());

        builder.Property(c => c.PolicyNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName(nameof(Contract.PolicyNumber).ToSnakeCase());

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasColumnName(nameof(Contract.IsActive).ToSnakeCase());

        builder.Ignore(c => c.Status);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired()
            .HasColumnName(nameof(Contract.CreatedAt).ToSnakeCase());

        builder.Property(x => x.UpdatedAt)
            .HasColumnName(nameof(Contract.UpdatedAt).ToSnakeCase());

        builder.HasIndex(c => c.ExternalId)
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.ExternalId)}".ToSnakeCase());

        builder.HasIndex(c => c.ProposalId)
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ProposalId)}".ToSnakeCase());

        builder.HasIndex(c => c.ClientId)
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ClientId)}".ToSnakeCase());

        builder.HasIndex(c => c.PolicyNumber)
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.PolicyNumber)}".ToSnakeCase());

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ProposalId, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ProposalId)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.PolicyNumber })
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.PolicyNumber)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ProposalId, c.PolicyNumber })
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.ProposalId)}{nameof(Contract.PolicyNumber)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.ProposalId })
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.ProposalId)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.ProposalId, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.ProposalId)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.PolicyNumber, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.PolicyNumber)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ProposalId, c.PolicyNumber, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ProposalId)}{nameof(Contract.PolicyNumber)}{nameof(Contract.IsActive)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.ProposalId, c.PolicyNumber })
            .IsUnique()
            .HasDatabaseName($"ux{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.ProposalId)}{nameof(Contract.PolicyNumber)}".ToSnakeCase());

        builder.HasIndex(c => new { c.ClientId, c.ProposalId, c.PolicyNumber, c.IsActive })
            .HasDatabaseName($"ix{nameof(Contract)}{nameof(Contract.ClientId)}{nameof(Contract.ProposalId)}{nameof(Contract.PolicyNumber)}{nameof(Contract.IsActive)}".ToSnakeCase());
    }
}
