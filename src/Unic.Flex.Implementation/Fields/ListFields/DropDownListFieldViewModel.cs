namespace Unic.Flex.Implementation.Fields.ListFields
{
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.ViewModels.Fields.ListFields;

    /// <summary>
    /// Dropdown list view model
    /// </summary>
    public class DropDownListFieldViewModel : ListFieldViewModel<string, ListItem>
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/DropDownList";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singleselectfield");
        }
    }
}
