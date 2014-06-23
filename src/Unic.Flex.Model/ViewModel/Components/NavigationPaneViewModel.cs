namespace Unic.Flex.Model.ViewModel.Components
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;

    public class NavigationPaneViewModel : IPresentationComponent
    {
        public NavigationPaneViewModel()
        {
            this.Items = new List<NavigationItem>();
        }
        
        public virtual IList<NavigationItem> Items { get; private set; } 
        
        public virtual string ViewName
        {
            get
            {
                return "Components/NavigationPane";
            }
        }
    }
}
