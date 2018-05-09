namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System;
    using System.Globalization;
    using Glass.Mapper;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.Fields.InputFields;

    /// <summary>
    /// Field for a date picker
    /// </summary>
    [SitecoreType(TemplateId = "{5CBCDE45-F0A9-4F6D-8273-685249ABDDF6}")]
    public class DatePickerField : InputField<DateTime?>
    {
        private readonly ICultureService cultureService;

        public DatePickerField(ICultureService cultureService)
        {
            this.cultureService = cultureService;
            this.DefaultValidators.Add(new DateValidator());
        }

        [SitecoreField("Default Value")]
        public override DateTime? DefaultValue { get; set; }

        [SitecoreField("Show Year and Month Changer")]
        public virtual bool ShowYearAndMonthChanger { get; set; }

        [SitecoreIgnore]
        public virtual string DateFormat => this.cultureService.GetDateFormat();

        [SitecoreIgnore]
        public override string TextValue => Value?.ToString(this.DateFormat, CultureInfo.InvariantCulture) ?? base.TextValue;

        [SitecoreIgnore]
        public override string ViewName => "Fields/InputFields/DatePicker";

        [SitecoreIgnore]
        public virtual string Locale => Sitecore.Context.Language.CultureInfo.TwoLetterISOLanguageName.ToLowerInvariant();

        public override void SetValue(object value)
        {
            if (!(value is string))
            {
                base.SetValue(value);
                return;
            }

            if (value.ToString() == Model.Definitions.Constants.EmptyFlexFieldDefaultValue)
            {
                this.Value = null;
                return;
            }

            try
            {
                this.Value = DateTime.Parse(value.ToString());
            }
            catch (FormatException ex)
            {
                base.SetValue(value);
            }
        }

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
