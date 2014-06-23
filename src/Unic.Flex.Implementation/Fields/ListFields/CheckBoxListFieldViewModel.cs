namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class CheckBoxListFieldViewModel : ListFieldViewModel<string[]>
    {
        // todo: the checkbox list doesn't work property, we need to make this working...
        // know issues:
        // - required valdiator (one of the checkbox must be checked) -> client- and server-side (the validator html attributes aren't currently written)
        // - values won't be resettet if no checkbox is selected and the step is called again
    }
}
