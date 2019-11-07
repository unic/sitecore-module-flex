namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Specifications;
    using Unic.Flex.Model.Validators;

    [SitecoreType(TemplateId = "{43B8BE85-8474-43A9-BEC5-14778B993B45}")]
    public class DateIntervalValidator : IValidator
    {
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public virtual string DefaultValidationMessageDictionaryKey => "Date is not valid";

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Date is not valid")]
        public virtual string ValidationMessage { get; set; }

        [SitecoreField("Amount")]
        public virtual int? Amount { get; set; }

        [SitecoreField("Interval Type")]
        public virtual Specification IntervalType { get; set; }

        [SitecoreField("Time Compare Type")]
        public virtual Specification TimeCompareType { get; set; }

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

            var amount = this.Amount;
            var intervalType = this.IntervalType.Value;
            var compareType = this.TimeCompareType.Value;

            if (amount == null || intervalType == null) return true;

            var dateValue = (DateTime)value;

            switch (this.GetEnumType<TimeCompareTypes>(compareType))
            {
                case TimeCompareTypes.Past:
                    return dateValue > this.CalculateIntervalDate(amount.Value * -1, intervalType);
                case TimeCompareTypes.PastOrEqual:
                    return dateValue >= this.CalculateIntervalDate(amount.Value * -1, intervalType);
                case TimeCompareTypes.EqualPast:
                    return dateValue == this.CalculateIntervalDate(amount.Value * -1, intervalType);
                case TimeCompareTypes.EqualFuture:
                    return dateValue == this.CalculateIntervalDate(amount.Value, intervalType);
                case TimeCompareTypes.Future:
                    return dateValue < this.CalculateIntervalDate(amount.Value, intervalType);
                case TimeCompareTypes.FutureOrEqual:
                    return dateValue <= this.CalculateIntervalDate(amount.Value, intervalType);
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
            return new Dictionary<string, object>();
        }

        private DateTime CalculateIntervalDate(int amount, string intervalType)
        {
            var today = DateTime.Today;

            switch (this.GetEnumType<IntervalTypes>(intervalType))
            {
                case IntervalTypes.Day:
                    return today.AddDays(amount);
                case IntervalTypes.Month:
                    return today.AddMonths(amount);
                case IntervalTypes.Year:
                    return today.AddYears(amount);
            }

            return today;
        }

        /// <summary>
        /// Parse selected interval type.
        /// </summary>
        /// <param name="intervalType">Interval type name.</param>
        /// <returns>Predefined enum value of interval types.</returns>
        private T? GetEnumType<T>(string intervalType)
            where T : struct
        {
            T type;
            if (Enum.TryParse(intervalType, true, out type))
            {
                return type;
            }

            return null;
        }

        /// <summary>
        /// Predefined interval types.
        /// </summary>
        internal enum IntervalTypes
        {
            Day,
            Month,
            Year
        }

        /// <summary>
        /// Predefined compare types.
        /// </summary>
        internal enum TimeCompareTypes
        {
            Past,
            PastOrEqual,
            EqualPast,
            EqualFuture,
            Future,
            FutureOrEqual
        }
    }
}
