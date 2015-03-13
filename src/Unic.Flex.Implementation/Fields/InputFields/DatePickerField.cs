﻿namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System;
    using System.Globalization;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.Fields.InputFields;

    /// <summary>
    /// Field for a date picker
    /// </summary>
    [SitecoreType(TemplateId = "{5CBCDE45-F0A9-4F6D-8273-685249ABDDF6}")]
    public class DatePickerField : InputField<DateTime?>
    {
        /// <summary>
        /// The culture service
        /// </summary>
        private readonly ICultureService cultureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerField" /> class.
        /// </summary>
        /// <param name="cultureService">The culture service.</param>
        public DatePickerField(ICultureService cultureService)
        {
            this.cultureService = cultureService;
            this.DefaultValidators.Add(new DateValidator());
        }
        
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override DateTime? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the year and month changer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the year and month changer should be shown; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Show Year and Month Changer")]
        public virtual bool ShowYearAndMonthChanger { get; set; }

        /// <summary>
        /// Gets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        [SitecoreIgnore]
        public virtual string DateFormat
        {
            get
            {
                return this.cultureService.GetDateFormat();
            }
        }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [SitecoreIgnore]
        public override string TextValue
        {
            get
            {
                return !this.Value.HasValue ? base.TextValue : this.Value.Value.ToString(this.DateFormat, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
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
        [SitecoreIgnore]
        public virtual string Locale
        {
            get
            {
                return Sitecore.Context.Language.CultureInfo.TwoLetterISOLanguageName.ToLowerInvariant();
            }
        }

        /// <summary>
        /// Binds the properties.
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
