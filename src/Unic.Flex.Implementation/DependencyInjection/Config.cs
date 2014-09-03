namespace Unic.Flex.Implementation.DependencyInjection
{
    using Ninject.Modules;
    using Unic.Flex.Implementation.Mailers;

    /// <summary>
    /// Ninject configuration module.
    /// </summary>
    public class Config : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            // todo: check if all scopes of these injetions are valid

            Bind<ISavePlugMailer>().To<SavePlugMailer>();
        }
    }
}
