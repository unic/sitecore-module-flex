namespace Unic.Flex.Mapping
{
    using System.Collections.Generic;

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
        /// Determines whether a form is stored in the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns><c>true</c> if the form is currently stored in the storage provider, <c>false</c> otherwise.</returns>
        bool IsFormStored(string formId);

        /// <summary>
        /// Determines whether a specific field from a form is actually stored in the storage provider or not.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns><c>true</c> if the field is curretly stored in the storage provider, <c>false</c> otherwise.</returns>
        bool IsFieldStored(string formId, string fieldId);

        /// <summary>
        /// Clears the form values completely out of the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        void ClearForm(string formId);

        /// <summary>
        /// Completes the step.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        void CompleteStep(string formId, int stepNumber);

        /// <summary>
        /// Reverts to step and invaldiate all completed step until the give one.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        void RevertToStep(string formId, int stepNumber);

        /// <summary>
        /// Determines whether a step number has been completed.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        /// <returns>Boolean value if the given step has been completed or not</returns>
        bool IsStepCompleted(string formId, int stepNumber);

        /// <summary>
        /// Gets the values from the session for a specific form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>Key-Value based dictionary with values</returns>
        IDictionary<string, object> GetFormValues(string formId);
    }
}
