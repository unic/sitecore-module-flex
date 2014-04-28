[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Flex.Website.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Unic.Flex.Website.NinjectConfig), "Stop")]

namespace Unic.Flex.Website
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Unic.Flex.DependencyInjection;

    /// <summary>
    /// Ninject configuration initializer
    /// </summary>
    public static class NinjectConfig 
    {
        /// <summary>
        /// The bootstrapper
        /// </summary>
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = Container.CreateKernel(GetModules().ToArray());
            kernel.Bind<Func<IKernel>>().ToMethod(context => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            return kernel;
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns>List of ninject modules</returns>
        private static IEnumerable<INinjectModule> GetModules()
        {
            yield return new Config();
        }     
    }
}
