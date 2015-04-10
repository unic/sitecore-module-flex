namespace Unic.Flex.Core.DependencyInjection
{
    using System;

    /// <summary>
    /// Interface for an IoC container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Verifies the container.
        /// </summary>
        /// <returns>Boolean result from verifying the container</returns>
        bool VerifyContainer();
        
        /// <summary>
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        void Bind<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        TService Resolve<TService>() where TService : class;

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        object Resolve(Type type);
    }
}
