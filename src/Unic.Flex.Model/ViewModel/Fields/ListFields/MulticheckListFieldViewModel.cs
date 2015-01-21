namespace Unic.Flex.Model.ViewModel.Fields.ListFields
{
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// View model for fields with multiple values to check
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TType">The type of the data item.</typeparam>
    public abstract class MulticheckListFieldViewModel<TValue, TType> : ListFieldViewModel<TValue, TType> where TType : IDataItem
    {
    }
}
