namespace Unic.Flex.Model.Fields.ListFields
{
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// Class for field with multiple values to check
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TType">The type of the data item.</typeparam>
    public abstract class MulticheckListField<TValue, TType> : ListField<TValue, TType> where TType : IDataItem
    {
    }
}
