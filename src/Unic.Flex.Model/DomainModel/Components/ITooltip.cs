namespace Unic.Flex.Model.DomainModel.Components
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;

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

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        Tooltip Tooltip { get; set; }
    }
}
