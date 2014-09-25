namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System;
    using System.Threading;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for the date picker field
    /// </summary>
    public class DatePickerFieldViewModel : InputFieldViewModel<DateTime?>
    {
        /// <summary>
        /// The date format
        /// </summary>
        private readonly string dateFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerFieldViewModel"/> class.
        /// </summary>
        public DatePickerFieldViewModel()
        {
            this.dateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }
        
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
                return "Fields/InputFields/DatePicker";
            }
        }

        /// <summary>
        /// Gets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        public virtual string DateFormat
        {
            get
            {
                return "{0:" + this.dateFormat + "}";
            }
        }

        /// <summary>
        /// Binds the needed attributes and properties after converting from domain model to the view model
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_datefield");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
        }
    }
}
