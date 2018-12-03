namespace Unic.Flex.Model.Fields.ListFields
{
    using System.Collections.Generic;
    using DataProviders;

    public interface IListItems<TType> where TType : IDataItem
    {
        IList<TType> Items { get; }
    }
}
