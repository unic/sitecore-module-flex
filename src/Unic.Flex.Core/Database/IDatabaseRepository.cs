namespace Unic.Flex.Core.Database
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
        /// <param name="disableTracking">Defines whether the tracking of the returned entities should be disabled</param>
        /// <returns>List of entities</returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool disableTracking = false);

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with this id</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Determines whether any entity exists with the given filter
        /// </summary>
        /// <param name="filter">Optional filter</param>
        /// <returns>
        ///     <c>true</c> if there is any entity that matches the filter; otherwise, <c>false</c>.
        /// </returns>
        bool Any(Expression<Func<TEntity, bool>> filter = null);
    }
}
