namespace Unic.Flex.Model.DomainModel.Plugs.SavePlugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Base class for all load plugs
    /// </summary>
    public abstract class SavePlugBase : PlugBase, ISavePlug
    {
        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(Form form);
    }
}
