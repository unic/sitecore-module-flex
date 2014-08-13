namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class CheckBoxListFieldViewModel : ListFieldViewModel<string[]>
    {
        // todo: client-side validator does not work actually -> needed attributes are missing

        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/CheckBoxList";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_checkboxgroup");
        }
    }
}
