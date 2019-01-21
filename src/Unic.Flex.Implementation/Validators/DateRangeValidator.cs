namespace Unic.Flex.Implementation.Validators
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Validates if a date is within a range.
    /// </summary>
    [SitecoreType(TemplateId = "{DA39F16E-79C5-4C6C-8813-A47E22F1D2F4}")]
    public class DateRangeValidator : IValidator
    {
        /// <summary>
        /// The culture service
        /// </summary>
        private readonly ICultureService cultureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeValidator"/> class.
        /// </summary>
        /// <param name="cultureService">The culture service.</param>
        public DateRangeValidator(ICultureService cultureService)
        {
            this.cultureService = cultureService;
        }
        
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public virtual string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Date is not valid";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Date is not valid")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>
        /// The minimum date.
        /// </value>
        [SitecoreField("Minimum Date")]
        public virtual DateTime? MinimumDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>
        /// The maximum date.
        /// </value>
        [SitecoreField("Maximum Date")]
        public virtual DateTime? MaximumDate { get; set; }

        /// <summary>
        /// Gets or sets the dynamic year and today range. 
        /// </summary>
        /// <value>
        /// The dynamic year and today range.
        /// </value>
        [SitecoreField("Dynamic Year And Today Range")]
        public virtual int? DynamicYearAndTodayRange { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public virtual bool IsValid(object value)
        {
            if (!(value is DateTime)) return true;

            var dateValue = (DateTime)value;
            if (this.MinimumDate.HasValue && dateValue < this.MinimumDate.Value) return false;
            if (this.MaximumDate.HasValue && dateValue > this.MaximumDate.Value) return false;

            if (this.DynamicYearAndTodayRange.HasValue)
            {
                var today = DateTime.Today;
                var dynamicDate = today.AddYears(this.DynamicYearAndTodayRange.Value);
                if (this.DynamicYearAndTodayRange > 0) 
                {
                    return today <= dateValue && dateValue <= dynamicDate;
                }
                else 
                {
                    return dynamicDate <= dateValue && dateValue <= today;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public virtual IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();

            attributes.Add("data-val-daterange", this.ValidationMessage);

            var dateFormat = this.cultureService.GetDateFormat();
            if (this.MinimumDate.HasValue)
            {
                attributes.Add("data-val-daterange-min", this.MinimumDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
            }

            if (this.MaximumDate.HasValue)
            {
                attributes.Add("data-val-daterange-max", this.MaximumDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
            }

            if (this.DynamicYearAndTodayRange.HasValue)
            {
                attributes.Add("data-val-daterange-range", this.DynamicYearAndTodayRange);
            }

            return attributes;
        }
    }
}
