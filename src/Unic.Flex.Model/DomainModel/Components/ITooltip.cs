namespace Unic.Flex.Model.DomainModel.Components
{
    /// <summary>
    /// Interface for a tooltip information.
    /// </summary>
    public interface ITooltip
    {   
        /// <summary>
        /// Gets or sets the tooltip title.
        /// </summary>
        /// <value>
        /// The tooltip title.
        /// </value>
        string TooltipTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        string TooltipText { get; set; }
    }
}
