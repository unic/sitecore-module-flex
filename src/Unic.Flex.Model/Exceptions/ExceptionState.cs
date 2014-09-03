namespace Unic.Flex.Model.Exceptions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class to hold exceptions messages
    /// </summary>
    public class ExceptionState : IExceptionState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionState"/> class.
        /// </summary>
        public ExceptionState()
        {
            this.Messages = new List<string>();
        }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error messages.
        /// </value>
        public IList<string> Messages { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.Messages.Any();
            }
        }
    }
}
