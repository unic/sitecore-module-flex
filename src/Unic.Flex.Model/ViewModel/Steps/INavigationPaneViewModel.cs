namespace Unic.Flex.Model.ViewModel.Steps
{
    using Unic.Flex.Model.ViewModel.Components;

    /// <summary>
    /// View model with a navigation pane
    /// </summary>
    public interface INavigationPaneViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the navigation pane.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane should be shown; otherwise, <c>false</c>.
        /// </value>
        bool ShowNavigationPane { get; set; }

        /// <summary>
        /// Gets or sets the navigation pane.
        /// </summary>
        /// <value>
        /// The navigation pane.
        /// </value>
        NavigationPaneViewModel NavigationPane { get; set; }
    }
}
