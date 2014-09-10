namespace Unic.Flex.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Repository for accessing entities over Entity Framework
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IDatabaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Deletes the specified entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(object id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Gets the entites from the data provider.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The sort order.</param>
        /// <param name="includeProperties">The include properties which should be eager loaded.</param>
        /// <returns>List of entities</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with this id</returns>
        TEntity GetById(object id);
    }
}
