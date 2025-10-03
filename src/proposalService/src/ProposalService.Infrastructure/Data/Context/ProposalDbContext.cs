using Microsoft.EntityFrameworkCore;
using ProposalService.Domain.Entities;

namespace ProposalService.Infrastructure.Data.Context;

/// <summary>
/// Representa o contexto de banco de dados do Entity Framework Core para gerenciar propostas e clientes dentro da aplicação.
/// </summary>
/// <remarks>
/// Este contexto fornece acesso às entidades de proposta e cliente por meio de propriedades DbSet fortemente tipadas.
/// Destina-se a ser utilizado com injeção de dependência e suporta configuração via método OnModelCreating.
/// O contexto é sealed e não pode ser herdado.
/// </remarks>
/// <param name="options">
/// As opções a serem utilizadas pelo contexto de banco de dados, incluindo configurações como o provedor de banco de dados e a string de conexão.
/// </param>
public sealed class ProposalDbContext(
    DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Obtém o conjunto de propostas rastreadas pelo contexto.
    /// </summary>
    /// <remarks>
    /// Use esta propriedade para consultar, adicionar, atualizar ou remover propostas no banco de dados através do Entity Framework Core.
    /// As alterações feitas no conjunto são rastreadas e persistidas quando SaveChanges é chamado.
    /// </remarks>
    public DbSet<Proposal> Proposals { get; private set; } = default!;

    /// <summary>
    /// Obtém o conjunto de clientes rastreados pelo contexto.
    /// </summary>
    /// <remarks>
    /// Use esta propriedade para consultar, adicionar, atualizar ou remover entidades de cliente no banco de dados.
    /// As alterações feitas no conjunto são rastreadas pelo contexto e persistidas quando SaveChanges é chamado.
    /// </remarks>
    public DbSet<Client> Clients { get; private set; } = default!;

    /// <summary>
    /// Configura o modelo para o contexto aplicando as configurações de entidade do assembly atual.
    /// </summary>
    /// <remarks>
    /// Este método é chamado pelo Entity Framework durante a inicialização do contexto para configurar o modelo.
    /// Ele aplica automaticamente todas as implementações de IEntityTypeConfiguration encontradas no assembly que contém o contexto.
    /// Sobrescreva este método para personalizar a configuração do modelo conforme necessário.
    /// </remarks>
    /// <param name="modelBuilder">
    /// O construtor utilizado para construir o modelo para o contexto. Fornece opções de configuração para tipos de entidade, relacionamentos e mapeamentos de banco de dados.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProposalDbContext).Assembly);
    }
}
