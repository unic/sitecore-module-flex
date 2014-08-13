namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class ListBoxFieldViewModel : ListFieldViewModel<string[]>
    {
        // todo: client-side required validator does also mark empty option (i.e. "Please select") as valid

        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/ListBox";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_multipleselectfield");
        }
    }
}
