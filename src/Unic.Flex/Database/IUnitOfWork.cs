namespace Unic.Flex.Database
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
        /// Saves changes to the data provider.
        /// </summary>
        void Save();
    }
}
