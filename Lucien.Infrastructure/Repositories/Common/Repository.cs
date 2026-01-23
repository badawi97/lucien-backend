using Lucien.Domain.Comman.Entities;
using Lucien.Domain.Comman.Interfaces;
using Lucien.Domain.Shared.Entities;
using Lucien.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lucien.Infrastructure.Repositories.Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : AuditableEntity<TEntity> 
    {
        private readonly LucienDbContext _context; 
        private readonly IUserContextService _userContextService;
        private readonly ILogger _logger;

        public Repository(IUserContextService userContextService, LucienDbContext context, ILogger logger)
        {
            _userContextService = userContextService;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">entity to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Void.</returns>
        public async Task<TEntity> CreateAsync(TEntity entity,
                                               CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAsync(): Error creating entity of type {EntityType}", typeof(TEntity).Name);
                throw;
            }
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">id filtered.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Void.</returns>
        public async Task DeleteAsync(Guid id,
                                      CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await GetByIdAsync(id);
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync(): Error deleting entity {EntityType} with Id {EntityId}", typeof(TEntity).Name, id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">id filtered.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>entity.</returns>
        public async Task<TEntity> GetByIdAsync(Guid id,
                                                CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                                 ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} with Id {id} was not found.");
        }

        /// <summary>
        /// Gets a count of entities with optional predicate filtered results predicating support.
        /// </summary>
        /// <param name="predicate">Optional count filtered results logic.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of entities.</returns>
        public async Task<int> GetCountAsync(IQueryable<TEntity> baseQuery,
                                             CancellationToken cancellationToken = default)
        {
            return await baseQuery.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a paged list of entities with optional soft delete and sorting support.
        /// </summary>
        /// <param name="baseQuery">Base query to apply filters on.</param>
        /// <param name="skipCount">How many records to skip.</param>
        /// <param name="maxResultCount">Max number of records to return.</param>
        /// <param name="sortingFunc">Optional sorting logic.</param>
        /// <param name="deletedEntities">Whether to include soft-deleted entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paged list of entities.</returns>
        public async Task<PagedResult<TEntity>> GetListAsync(IQueryable<TEntity> baseQuery,
                                                             int skipCount,
                                                             int maxResultCount,
                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>>? sortingFunc = null,
                                                             bool deletedEntities = false,
                                                             CancellationToken cancellationToken = default)
        {
            if (deletedEntities)
            {
                baseQuery = baseQuery.IgnoreQueryFilters().Where(c => c.IsDeleted);
            }

            if (sortingFunc != null)
            {
                baseQuery = sortingFunc(baseQuery);
            }

            var totalCount = await GetCountAsync(baseQuery, cancellationToken: cancellationToken);

            List<TEntity> entities = new List<TEntity>();
            try
            {
                entities = await baseQuery.AsNoTracking()
                                          .Skip(skipCount)
                                          .Take(maxResultCount)
                                          .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetListAsync(): Error fetching list of entity type {EntityType}", typeof(TEntity).Name);
                throw;
            }

            return new PagedResult<TEntity>(totalCount, entities);
        }


        /// <summary>
        /// Updated entity data.
        /// </summary>
        /// <param name="id">id filtered.</param>
        /// <param name="entity">entity to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>entity.</returns>
        public async Task<TEntity> UpdateAsync(Guid id,
                                               TEntity entity,
                                               CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
                if (existingEntity == null)
                    throw new KeyNotFoundException($"{typeof(TEntity).Name} with Id {id} was not found.");

                existingEntity.Update(entity);

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync(): Error updating entity {EntityType} with Id {EntityId}", typeof(TEntity).Name, id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Checks if an entity exists by ID.
        /// </summary>
        /// <param name="id">id filtered.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>boolean.</returns>
        public async Task<bool> ExistsAsync(Guid id,
                                            CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AnyAsync(e => e.Id == id, cancellationToken);
        }

        /// <summary>
        /// Returns an <see cref="IQueryable{TEntity}"/> for the entity, allowing further filtering, sorting, or projection.
        /// </summary>
        /// <param name="includeSoftDeleted">Whether to include soft-deleted entities by ignoring global query filters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> of the entity type.</returns>
        public IQueryable<TEntity> GetQueryable(bool includeSoftDeleted = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (includeSoftDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }
    }
}
