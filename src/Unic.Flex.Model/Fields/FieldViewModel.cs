namespace Unic.Flex.Model.Fields
{
    using Unic.Flex.Model;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Presentation;

    public class FieldViewModel : IPresentationComponent, IViewModel
    {
        public FieldViewModel(ItemBase domainModel)
        {
            this.DomainModel = domainModel;
        }
        
        public string Key { get; set; }

        public string Label { get; set; }

        public object Value { get; set; }

        public string ViewName { get; set; }

        public ItemBase DomainModel { get; set; }
    }
}