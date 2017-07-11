namespace Unic.Flex.Core.Database
{
    using System;
    using Unic.Flex.Model.Entities;

    public interface IUnitOfWork : IDisposable
    {
        IDatabaseRepository<Form> FormRepository { get; }

        IDatabaseRepository<Session> SessionRepository { get; }

        IDatabaseRepository<Job> JobRepository { get; }

        IDatabaseRepository<Task> TaskRepository { get; }

        void Save();
    }
}
