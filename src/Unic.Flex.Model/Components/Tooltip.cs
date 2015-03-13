namespace Unic.Flex.Model.Components
{
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// A tooltip.
    /// </summary>
    public class Tooltip : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public virtual string Text { get; set; }

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
                return "Components/Tooltip";
            }
        }
    }
}
