namespace Unic.Flex.Model.ViewModel.Components
{
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Interface for a tooltip information.
    /// </summary>
    public class TooltipViewModel : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the tooltip title.
        /// </summary>
        /// <value>
        /// The tooltip title.
        /// </value>
        public virtual string TooltipTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        public virtual string TooltipText { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the tooltip.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tooltip should be shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowTooltip
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.TooltipText);
            }
        }

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
