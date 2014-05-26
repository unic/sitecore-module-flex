namespace Unic.Flex.Model.ViewModel.Fields
{
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Validation;

    public interface IFieldViewModel<TValue> : IFieldViewModel
    {
        new TValue Value { get; set; }
    }

    public interface IFieldViewModel : IPresentationComponent
    {
        object Value { get; set; }

        string Key { get; set; }
    }
}
