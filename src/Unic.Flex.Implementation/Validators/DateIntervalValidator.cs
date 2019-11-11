namespace Post.Core.Forms.Validators
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
        public virtual string DefaultValidationMessageDictionaryKey => "Date is not valid";

        [SitecoreDictionaryFallbackField("Validation Message", "Date is not valid")]
        public virtual string ValidationMessage { get; set; }

        [SitecoreField("Time Amount")]
        public virtual int? TimeAmount { get; set; }

        [SitecoreField("Time Interval Type")]
        public virtual Specification TimeIntervalType { get; set; }

        [SitecoreField("Time Compare Type")]
        public virtual Specification TimeCompareType { get; set; }

        public virtual bool IsValid(object value)
        {
            if (!(value is DateTime)) return true;

            var amount = this.TimeAmount;
            var intervalType = this.TimeIntervalType.Value;

            if (amount == null || intervalType == null) return true;

            var compareType = this.TimeCompareType.Value;
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

            return false;
        }

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

        internal enum IntervalTypes
        {
            Day,
            Month,
            Year
        }

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
