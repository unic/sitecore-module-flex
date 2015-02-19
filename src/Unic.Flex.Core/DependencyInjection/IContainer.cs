namespace Unic.Flex.Core.DependencyInjection
{
    using System;

    /// <summary>
    /// Interface for an IoC container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TFrom">The type of bind.</typeparam>
        /// <typeparam name="TTo">The type of resolve.</typeparam>
        void Bind<TFrom, TTo>() where TTo : TFrom;

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        T Resolve<T>();

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
