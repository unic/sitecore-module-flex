[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Flex.NinjectIoC.Registration), "Start", Order = 1)]

namespace Unic.Flex.NinjectIoC
{
    using Unic.Flex.Core.DependencyInjection;

    /// <summary>
    /// Ninject configuration initializer
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DependencyResolver.SetContainer(new Container());
        }
    }
}
