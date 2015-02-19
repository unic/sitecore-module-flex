namespace Unic.Flex.SimpleInjectorIoC
{
    using SimpleInjector;
    using System;
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Simple Injector IoC container
    /// </summary>
    public class SimpleInjectorContainer : IContainer
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly Container container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorContainer"/> class.
        /// </summary>
        public SimpleInjectorContainer()
        {
            // create the container
            this.container = new Container();
            this.container.Options.AllowOverridingRegistrations = true;
            
            // initialize bindings
            Bootstrapper.Initialize(this.container);
        }

        /// <summary>
        /// Binds this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void Bind<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            this.container.Register<TService, TImplementation>();
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>Instance of the type</returns>
        public TService Resolve<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }

        /// <summary>
        /// Resolves the specified type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>
        /// Instance of the type
        /// </returns>
        public object Resolve(Type type)
        {
            return this.container.GetInstance(type);
        }
    }
}
