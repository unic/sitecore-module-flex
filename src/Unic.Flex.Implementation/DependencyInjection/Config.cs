namespace Unic.Flex.Implementation.DependencyInjection
{
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Implementation.Mailers;

    /// <summary>
    /// Ninject configuration module.
    /// </summary>
    public class Config : Flex.DependencyInjection.Config
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ISavePlugMailer>().To<SavePlugMailer>();
            Bind<ISaveToDatabaseService>().To<SaveToDatabaseService>();
        }
    }
}
