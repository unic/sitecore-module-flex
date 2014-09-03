namespace Unic.Flex.Model.ViewModel.Components
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// View model for showing errors
    /// </summary>
    public class ErrorViewModel : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IEnumerable<string> Messages { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public string ViewName
        {
            get
            {
                return "Components/Error";
            }
        }
    }
}
