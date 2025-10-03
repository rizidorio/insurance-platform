using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Data.Contexts;

/// <summary>
/// Representa o contexto de banco de dados do Entity Framework Core para gerenciar entidades de notificação.
/// </summary>
/// <remarks>
/// Este contexto fornece acesso aos dados de notificação através da propriedade <see cref="Notifications"/>.
/// É destinado ao uso com o Entity Framework Core e deve ser configurado com as opções apropriadas para o
/// armazenamento de dados da sua aplicação.
/// </remarks>
/// <param name="options">
/// As opções a serem usadas pelo contexto de banco de dados, incluindo configuração como o provedor de banco de dados e a string de conexão.
/// Não pode ser nulo.
/// </param>
public sealed class NotificationDbContext(
    DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Obtém a coleção de notificações rastreadas pelo contexto.
    /// </summary>
    /// <remarks>
    /// Use esta propriedade para consultar, adicionar, atualizar ou remover entidades de notificação dentro do contexto
    /// de banco de dados. As alterações feitas na coleção são persistidas no banco de dados quando SaveChanges é chamado.
    /// </remarks>
    public DbSet<Notification> Notifications { get; private set; } = default!;

    /// <summary>
    /// Configura o modelo para o contexto aplicando as configurações de entidade do assembly atual.
    /// </summary>
    /// <remarks>
    /// Este método é chamado pelo Entity Framework durante a criação do modelo. Ele aplica todas as implementações de
    /// IEntityTypeConfiguration encontradas no assembly que contém o contexto, permitindo configuração modular das entidades.
    /// </remarks>
    /// <param name="modelBuilder">
    /// O construtor usado para construir o modelo para o contexto. Fornece opções de configuração para tipos de entidade,
    /// relacionamentos e mapeamentos de banco de dados.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    }
}
