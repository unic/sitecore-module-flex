namespace Unic.Flex.Model.Exceptions
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for holding exception messages.
    /// </summary>
    public interface IExceptionState
    {
        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        IList<string> Messages { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        bool HasErrors { get; }
    }
}
