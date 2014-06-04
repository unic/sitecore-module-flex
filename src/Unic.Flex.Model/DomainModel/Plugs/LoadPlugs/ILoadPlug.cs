namespace Unic.Flex.Model.DomainModel.Plugs.LoadPlugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Interface for defining a load plug
    /// </summary>
    public interface ILoadPlug
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        void Execute(Form form);
    }
}
