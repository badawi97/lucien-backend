
namespace Lucien.Domain.Shared.Entities
{
    public abstract class AuditableEntity<TEntity> : AuditableEntityBase where TEntity : class
    {
        /// <summary>
        /// Updates the current entity with values from the given entity.
        /// </summary>
        /// <param name="entity">The entity to copy values from.</param>
        /// <returns>The updated entity.</returns>
        public abstract TEntity Update(TEntity entity);
    }
}
