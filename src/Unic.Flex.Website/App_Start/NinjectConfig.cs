[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Unic.Flex.Website.App_Start.NinjectConfig), "Stop")]

namespace Unic.Flex.Website.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Unic.Flex.Core.DependencyInjection;

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
            if (UseCustomContainer()) return;
            
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            if (UseCustomContainer()) return;
            
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
            yield return new Unic.Flex.Implementation.DependencyInjection.Config();
        }

        /// <summary>
        /// Check if a custom container is used.
        /// </summary>
        /// <returns>Boolean value if a custom container is used</returns>
        private static bool UseCustomContainer()
        {
            return Sitecore.Configuration.Settings.GetBoolSetting("Flex.IoC.UseCustomContainer", false);
        }
    }
}
