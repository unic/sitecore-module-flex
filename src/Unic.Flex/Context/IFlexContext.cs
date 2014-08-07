namespace Unic.Flex.Context
{
    using Sitecore.Data.Items;
    using Unic.Flex.Model.DomainModel;
    using Unic.Flex.Model.DomainModel.Forms;

    public interface IFlexContext
    {
        ItemBase Item { get; set; }

        Form Form { get; set; }

        void SetContextItem(Item contextItem);
    }
}
