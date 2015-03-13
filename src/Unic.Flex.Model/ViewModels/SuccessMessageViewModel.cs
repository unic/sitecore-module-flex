namespace Unic.Flex.Model.ViewModels
{
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// View model for showing the success message
    /// </summary>
    public class SuccessMessageViewModel : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public virtual string Message { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName
        {
            get
            {
                return "Components/SuccessMessage";
            }
        }
    }
}
