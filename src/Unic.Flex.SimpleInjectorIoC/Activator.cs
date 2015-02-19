[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Flex.SimpleInjectorIoC.Activator), "PreStart", Order = 1)]

namespace Unic.Flex.SimpleInjectorIoC
{
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Simple injector configuration activator
    /// </summary>
    public static class Activator
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void PreStart()
        {
            DependencyResolver.SetContainer(new Container());
        }
    }
}
