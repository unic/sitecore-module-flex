namespace Unic.Flex.Plugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Service for the plug framework
    /// </summary>
    public interface IPlugsService
    {
        /// <summary>
        /// Executes the load plugs.
        /// </summary>
        /// <param name="form">The form.</param>
        void ExecuteLoadPlugs(Form form);

        /// <summary>
        /// Executes the save plugs.
        /// </summary>
        /// <param name="form">The form.</param>
        void ExecuteSavePlugs(Form form);
    }
}
