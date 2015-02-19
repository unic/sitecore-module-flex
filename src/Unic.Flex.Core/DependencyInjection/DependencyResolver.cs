namespace Unic.Flex.Core.DependencyInjection
{
    using System;

    /// <summary>
    /// Dependency injection container.
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// The container
        /// </summary>
        private static IContainer container;

        /// <summary>
        /// Sets the kernel.
        /// </summary>
        /// <param name="newContainer">The new container.</param>
        public static void SetContainer(IContainer newContainer)
        {
            container = newContainer;
        }

        /// <summary>
        /// Adds a new binding to the container.
        /// </summary>
        /// <typeparam name="TFrom">The type of bind.</typeparam>
        /// <typeparam name="TTo">The type of resolve.</typeparam>
        public static void Bind<TFrom, TTo>() where TTo : TFrom
        {
            container.Bind<TFrom, TTo>();
        }

        /// <summary>
        /// Resolves an instance of given type from the container.
        /// </summary>
        /// <typeparam name="T">Type of the class to resolve</typeparam>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public static object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}
