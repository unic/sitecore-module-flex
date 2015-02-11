namespace Unic.Flex.Model.ViewModel.Components
{
    /// <summary>
    /// Navigation item for the navigation pane
    /// </summary>
    public class NavigationItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public virtual string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is linked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is linked; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLinked { get; set; }
    }
}
