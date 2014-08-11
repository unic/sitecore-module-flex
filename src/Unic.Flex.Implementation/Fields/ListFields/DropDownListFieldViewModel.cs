namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    /// <summary>
    /// Dropdown list view model
    /// </summary>
    public class DropDownListFieldViewModel : ListFieldViewModel<string>
    {
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/DropDownList";
            }
        }
    }
}
