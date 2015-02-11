namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for file upload fields
    /// </summary>
    public class FileUploadFieldViewModel : InputFieldViewModel<UploadedFile>
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
                return "Fields/InputFields/FileUpload";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_fileinputfield");
        }
    }
}
