namespace Unic.Flex.Implementation.Database
{
    using System;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Service for saving form to database.
    /// </summary>
    public interface ISaveToDatabaseService
    {
        /// <summary>
        /// Saves the specified form to the database.
        /// </summary>
        /// <param name="form">The form.</param>
        int Save(IForm form);

        /// <summary>
        /// Determines whether the specified form identifier has entries.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>Boolean value wheather the form has entries in the database.</returns>
        bool HasEntries(Guid formId);

        /// <summary>
        /// Determines whether the curent user has permission to export the given form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>
        /// Boolean value wheather the user has permission to export the form.
        /// </returns>
        bool HasExportPermissions(Guid formId);

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="fileName">Name of the file.</param>
        void ExportForm(IForm form, string fileName);
    }
}
