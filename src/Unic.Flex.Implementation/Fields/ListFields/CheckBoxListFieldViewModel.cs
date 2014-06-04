namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class CheckBoxListFieldViewModel : ListFieldViewModel<string[]>
    {
        // todo: the checkbox list doesn't work property, we need to make this working...
        // know issues:
        // - required valdiator (one of the checkbox must be checked) -> client- and server-side
        // - it always add's validator to each of the checkbox, which is wrong
        // - model binding isn't correct, because the model state is always invalid actually (couldn't convert the .Items property or something linke that)
    }
}
