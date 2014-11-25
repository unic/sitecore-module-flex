namespace Unic.Flex.Implementation.Fields.TextOnly
{
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// View model for text only fields
    /// </summary>
    public class TextOnlyFieldViewModel : FieldBaseViewModel<string>, IFieldWithoutPost
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
                return "Fields/TextOnlyFields/TextOnly";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_rtefield");

            if (!string.IsNullOrWhiteSpace(this.Label))
            {
                this.AddCssClass("flex_var_label");
            }
        }
    }
}
