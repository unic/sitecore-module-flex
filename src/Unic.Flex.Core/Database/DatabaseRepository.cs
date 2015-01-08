﻿namespace Unic.Flex.Core.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Database repository for accessing entities over the Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class DatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DatabaseRepository(FlexDbContext context)
        {
            this.Context = context;
            this.DatabaseSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected virtual FlexDbContext Context { get; set; }

        /// <summary>
        /// Gets or sets the database set.
        /// </summary>
        /// <value>
        /// The database set.
        /// </value>
        protected virtual DbSet<TEntity> DatabaseSet { get; set; }
        
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Insert(TEntity entity)
        {
            this.DatabaseSet.Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = this.DatabaseSet.Find(id);
            this.Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.DatabaseSet.Attach(entityToDelete);
            }

            this.DatabaseSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            this.DatabaseSet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Gets the entites from the data provider.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The sort order.</param>
        /// <param name="includeProperties">The include properties which should be eager loaded.</param>
        /// <returns>List of entities</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.DatabaseSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with this id</returns>
        public virtual TEntity GetById(object id)
        {
            return this.DatabaseSet.Find(id);
        }
    }
}
