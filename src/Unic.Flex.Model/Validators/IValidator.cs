namespace Unic.Flex.Model.Validators
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a validator.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        string ValidationMessage { get; set; }
        
        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value entered is valid, <c>false</c> otherwise</returns>
        bool IsValid(object value);

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>Key-Value based dictionary with additional html attributes</returns>
        IDictionary<string, object> GetAttributes();
    }
}
