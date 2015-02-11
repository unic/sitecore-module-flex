namespace Unic.Flex.Core.Database
{
    using System;
    using Unic.Flex.Model.Entities;

    /// <summary>
    /// Interface for a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the form repository.
        /// </summary>
        /// <value>
        /// The form repository.
        /// </value>
        IDatabaseRepository<Form> FormRepository { get; }

        /// <summary>
        /// Gets the job repository.
        /// </summary>
        /// <value>
        /// The job repository.
        /// </value>
        IDatabaseRepository<Job> JobRepository { get; }

        /// <summary>
        /// Saves changes to the data provider.
        /// </summary>
        void Save();
    }
}
