namespace Unic.Flex.Model.Components
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// The navigation pane component
    /// </summary>
    public class NavigationPane : IPresentationComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPane"/> class.
        /// </summary>
        public NavigationPane()
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
