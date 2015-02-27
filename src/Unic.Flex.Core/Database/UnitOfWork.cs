namespace Unic.Flex.Core.Database
{
    using System;
    using Unic.Flex.Model.Entities;

    /// <summary>
    /// Implementing unit of work for accessing database entities
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly FlexDbContext context;
        
        /// <summary>
        /// Disposed member.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The form repository
        /// </summary>
        private IDatabaseRepository<Form> formRepository;

        /// <summary>
        /// The job repository
        /// </summary>
        private IDatabaseRepository<Job> jobRepository;

        /// <summary>
        /// The task repository
        /// </summary>
        private IDatabaseRepository<Task> taskRepository; 

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        public UnitOfWork()
        {
            this.context = new FlexDbContext();
        }

        /// <summary>
        /// Gets the form repository.
        /// </summary>
        /// <value>
        /// The form repository.
        /// </value>
        public virtual IDatabaseRepository<Form> FormRepository
        {
            get
            {
                return this.formRepository ?? (this.formRepository = new DatabaseRepository<Form>(this.context));
            }
        }

        /// <summary>
        /// Gets the jobs repository.
        /// </summary>
        /// <value>
        /// The jobs repository.
        /// </value>
        public virtual IDatabaseRepository<Job> JobRepository
        {
            get
            {
                return this.jobRepository ?? (this.jobRepository = new DatabaseRepository<Job>(this.context));
            }
        }

        /// <summary>
        /// Gets the task repository.
        /// </summary>
        /// <value>
        /// The task repository.
        /// </value>
        public virtual IDatabaseRepository<Task> TaskRepository
        {
            get
            {
                return this.taskRepository ?? (this.taskRepository = new DatabaseRepository<Task>(this.context));
            }
        }

        /// <summary>
        /// Saves changes to the data provider.
        /// </summary>
        public virtual void Save()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
