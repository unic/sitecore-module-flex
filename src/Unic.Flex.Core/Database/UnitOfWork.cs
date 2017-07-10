namespace Unic.Flex.Core.Database
{
    using System;
    using Unic.Flex.Model.Entities;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlexDbContext context;
        
        private bool disposed;

        private IDatabaseRepository<Form> formRepository;

        private IDatabaseRepository<Session> sessionRepository;

        private IDatabaseRepository<Job> jobRepository;

        private IDatabaseRepository<Task> taskRepository; 

        public UnitOfWork()
        {
            this.context = new FlexDbContext();
        }

        public virtual IDatabaseRepository<Form> FormRepository => this.formRepository ?? (this.formRepository = new DatabaseRepository<Form>(this.context));

        public virtual IDatabaseRepository<Session> SessionRepository => this.sessionRepository ?? (this.sessionRepository = new DatabaseRepository<Session>(this.context));

        public virtual IDatabaseRepository<Job> JobRepository => this.jobRepository ?? (this.jobRepository = new DatabaseRepository<Job>(this.context));

        public virtual IDatabaseRepository<Task> TaskRepository => this.taskRepository ?? (this.taskRepository = new DatabaseRepository<Task>(this.context));

        public virtual void Save()
        {
            this.context.SaveChanges();
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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
