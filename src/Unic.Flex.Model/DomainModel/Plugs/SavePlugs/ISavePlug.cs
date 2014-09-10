namespace Unic.Flex.Model.DomainModel.Plugs.SavePlugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Interface for defining a save plug
    /// </summary>
    public interface ISavePlug
    {
        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        bool IsAsync { get; }
        
        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        void Execute(Form form);
    }
}
