namespace Unic.Flex.Implementation.Database
{
    using Unic.Flex.Model.DomainModel.Forms;

    /// <summary>
    /// Service for saving form to database.
    /// </summary>
    public interface ISaveToDatabaseService
    {
        /// <summary>
        /// Saves the specified form to the database.
        /// </summary>
        /// <param name="form">The form.</param>
        void Save(Form form);
    }
}
