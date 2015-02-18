namespace Unic.Flex.Core.DependencyInjection
{
    using System;

    /// <summary>
    /// Interface for an IoC container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        T Resolve<T>(string name = null);

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        object Resolve(Type type, string name = null);
    }
}
