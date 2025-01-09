using FIAP.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Infrastructure.Extensions;

/// <summary>
/// Extension methods for finding entities asynchronously in a DbSet.
/// </summary>
public static class FindEntityExtensions
{
    /// <summary>
    /// Asynchronously finds an entity based on the given identifier.
    /// </summary>
    /// <param name="dbSet">The DbSet instance to search within.</param>
    /// <param name="entityId">The identifier of the entity to find.</param>
    /// <param name="nullIdMessage">The message to include in the exception if the entityId is null.</param>
    /// <param name="notFoundMessage">The message to include in the exception if the entity is not found.</param>
    /// <typeparam name="TData">The type of the entity to find.</typeparam>
    /// <returns>The entity found in the DbSet.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entityId is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the entity is not found.</exception>
    public static async Task<TData> FindEntityAsync<TData>
    (this DbSet<TData> dbSet
        , uint? entityId
        , string? notFoundMessage
        , string? nullIdMessage = "Id obrigatório!"
    ) where TData : class
    {
        var id = entityId ?? throw new ArgumentNullException(nameof(entityId), nullIdMessage);
        var entity = await dbSet.FindAsync(id);
        return entity ?? throw new ArgumentException(notFoundMessage);
    }
    
    /// <summary>
    /// Asynchronously finds an entity of type <see cref="Contact"/> based on the given identifier.
    /// </summary>
    /// <param name="dbSet">The DbSet instance to search within.</param>
    /// <param name="entityId">The identifier of the entity to find.</param>
    /// <returns>The Contact entity found in the DbSet.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entityId is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the entity is not found.</exception>
    public static Task<Contact> FindEntityAsync(this DbSet<Contact> dbSet, uint? entityId)
        => FindEntityAsync(dbSet, entityId, "Contato não encontrado!");
}