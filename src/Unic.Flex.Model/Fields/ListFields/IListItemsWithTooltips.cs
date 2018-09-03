namespace Unic.Flex.Model.Fields.ListFields
{
    using DataProviders;

    public interface IListItemsWithTooltips : IListItems<ListItem>
    {
        bool HasSeparateTooltips { get; }
    }
}
