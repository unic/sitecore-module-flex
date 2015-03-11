namespace Unic.Flex.Model.Plugs.LoadPlugs
{
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Base class for all load plugs
    /// </summary>
    public abstract class LoadPlugBase : PlugBase, ILoadPlug
    {
        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(Form form);
    }
}
