namespace Unic.Flex.Mapping
{
    /// <summary>
    /// Repository containg data access to user generated form data
    /// </summary>
    public interface IUserDataRepository
    {
        /// <summary>
        /// Gets the value from the storage.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>Value loaded from the storage provider</returns>
        object GetValue(string formId, string fieldId);

        /// <summary>
        /// Sets and stores the value for a field in the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="value">The value.</param>
        void SetValue(string formId, string fieldId, object value);

        /// <summary>
        /// Clears the form values completely out of the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        void ClearForm(string formId);
    }
}
