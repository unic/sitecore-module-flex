[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Flex.NinjectIoC.Activator), "PreStart", Order = 1)]

namespace Unic.Flex.NinjectIoC
{
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Ninject configuration activator
    /// </summary>
    public static class Activator
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void PreStart()
        {
            DependencyResolver.SetContainer(new NinjectContainer());
        }
    }
}
