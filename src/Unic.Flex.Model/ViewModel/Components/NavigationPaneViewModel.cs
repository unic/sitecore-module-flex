namespace Unic.Flex.Model.ViewModel.Components
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// View model for the navigation pane
    /// </summary>
    public class NavigationPaneViewModel : IPresentationComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPaneViewModel"/> class.
        /// </summary>
        public NavigationPaneViewModel()
        {
            this.Items = new List<NavigationItem>();
        }

        /// <summary>
        /// Gets the navigation items.
        /// </summary>
        /// <value>
        /// The navigation items.
        /// </value>
        public virtual IList<NavigationItem> Items { get; private set; }

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
                return "Components/NavigationPane";
            }
        }
    }
}
