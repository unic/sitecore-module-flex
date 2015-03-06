namespace Unic.Flex.Model.DomainModel.Plugs.SavePlugs
{
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Base class for all load plugs
    /// </summary>
    public abstract class SavePlugBase : PlugBase, ISavePlug
    {
        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous. By default, plugs are executed async.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsAsync
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(Form form);
    }
}
