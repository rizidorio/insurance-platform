using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data.Contexts;

/// <summary>
/// Representa o contexto de banco de dados do Entity Framework Core para o domínio de contratação, fornecendo acesso às entidades relacionadas a contratos.
/// </summary>
/// <remarks>
/// Este contexto é destinado ao uso com o Entity Framework Core e gerencia o conjunto de entidades Contracts.
/// A entidade Proposal é excluída das migrações e não terá seu esquema gerenciado por operações de migração.
/// Esta classe é sealed e não pode ser herdada.
/// </remarks>
/// <param name="options">
/// As opções a serem usadas pelo contexto de banco de dados, incluindo configuração como o provedor de banco de dados e a string de conexão. Não pode ser nulo.
/// </param>
public sealed class HiringDbContext(
    DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Obtém o conjunto de entidades de contrato para consulta e salvamento.
    /// </summary>
    /// <remarks>
    /// Esta propriedade fornece acesso aos contratos no contexto do banco de dados.
    /// Use consultas LINQ para recuperar, adicionar, atualizar ou remover registros de contrato.
    /// As alterações feitas no conjunto são rastreadas pelo contexto e persistidas no banco de dados quando SaveChanges é chamado.
    /// </remarks>
    public DbSet<Contract> Contracts { get; private set; } = default!;

    /// <summary>
    /// Configura o modelo para o contexto aplicando configurações de entidade e customizando o mapeamento das entidades.
    /// </summary>
    /// <remarks>
    /// Este método aplica todas as configurações de entidade do assembly atual e exclui a entidade 'Proposal' das migrações.
    /// Sobrescreva este método para customizar a criação do modelo para o contexto.
    /// </remarks>
    /// <param name="modelBuilder">
    /// O construtor usado para construir o modelo para o contexto. Fornece configuração para tipos de entidade, relacionamentos e mapeamento de banco de dados.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HiringDbContext).Assembly);
    }
}
