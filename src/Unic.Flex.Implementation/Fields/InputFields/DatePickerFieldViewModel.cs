namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System;
    using System.Globalization;
    using Unic.Flex.Model.ViewModel.Fields.InputFields;

    /// <summary>
    /// View model for the date picker field
    /// </summary>
    public class DatePickerFieldViewModel : InputFieldViewModel<DateTime?>
    {
        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        public virtual string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the year and month changer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the year and month changer should be shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowYearAndMonthChanger { get; set; }
        
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
        /// Gets the locale.
        /// </summary>
        /// <value>
        /// The locale.
        /// </value>
        public virtual string Locale
        {
            get
            {
                return Sitecore.Context.Language.CultureInfo.TwoLetterISOLanguageName.ToLowerInvariant();
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

            if (this.Value != null)
            {
                this.Attributes.Add("Value", this.Value.Value.ToString(this.DateFormat, CultureInfo.InvariantCulture));
            }
        }
    }
}
