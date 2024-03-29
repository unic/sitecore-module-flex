﻿namespace Unic.Flex.SimpleInjectorIoC
{
    using SimpleInjector;
    using System;
    using System.Web;
    using SimpleInjector.Extensions.LifetimeScoping;
    using SimpleInjector.Integration.Web;
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
        public SimpleInjectorContainer() : this(CreateStandardContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public SimpleInjectorContainer(Container container)
        {
            // set container
            this.container = container;
            
            // initialize bindings
            Config.Initialize(this.container);
        }

        /// <summary>
        /// Verifies the container.
        /// </summary>
        /// <returns>
        /// Boolean result from verifying the container
        /// </returns>
        public bool VerifyContainer()
        {
            // suppress warnings and issues
            Config.Suppression(this.container);

            this.container.Verify();
            return true;
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

        public object BeginScope()
        {
            return this.container.BeginLifetimeScope();
        }

        public void EndScope(object scope)
        {
            var disposableScope = scope as IDisposable;
            disposableScope?.Dispose();
        }

        /// <summary>
        /// Creates a standard container.
        /// </summary>
        /// <returns>Dependency injection container</returns>
        private static Container CreateStandardContainer()
        {
            var standardContainer = new Container();
            standardContainer.Options.AllowOverridingRegistrations = true;
            standardContainer.Options.DefaultScopedLifestyle = GetHybridLifestyle();;

            return standardContainer;
        }
        
        private static ScopedLifestyle GetHybridLifestyle()
        {
            return Lifestyle.CreateHybrid(IsInWebContext, new WebRequestLifestyle(), new LifetimeScopeLifestyle());
        }

        private static bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }
    }
}
