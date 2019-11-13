namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using Definitions;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Specifications;
    using Unic.Flex.Model.Validators;

    [SitecoreType(TemplateId = "{43B8BE85-8474-43A9-BEC5-14778B993B45}")]
    public class DateIntervalValidator : IValidator
    {
        private readonly ILogger logger;

        public DateIntervalValidator(ILogger logger)
        {
            this.logger = logger;
        }

        public virtual string DefaultValidationMessageDictionaryKey => "Date is not valid";

        [SitecoreDictionaryFallbackField("Validation Message", "Date is not valid")]
        public virtual string ValidationMessage { get; set; }

        [SitecoreSharedField("Time Amount")]
        public virtual int? TimeAmount { get; set; }

        [SitecoreSharedField("Time Interval Type")]
        public virtual Specification TimeIntervalType { get; set; }

        [SitecoreSharedField("Time Compare Type")]
        public virtual Specification TimeCompareType { get; set; }

        public virtual bool IsValid(object value)
        {
            if (!(value is DateTime)) return true;

            var amount = this.TimeAmount;
            if (amount == null)
            {
                this.logger.Warn("Settings property Time Amount does not have a value set", this);
                return false;
            }

            var intervalType = this.TimeIntervalType?.Value;
            if (intervalType == null)
            {
                this.logger.Warn("Settings property Time Interval Type does not have a value set", this);
                return false;
            }

            var compareType = this.TimeCompareType?.Value;
            if (compareType == null)
            {
                this.logger.Warn("Settings property Time Compare Type does not have a value set", this);
                return false;
            }

            var dateValue = (DateTime)value;

            switch (this.GetEnumType<Enums.TimeCompareTypes>(compareType))
            {
                case Enums.TimeCompareTypes.NotOlderThan:
                    return dateValue >= this.CalculateIntervalDate(amount.Value * -1, intervalType);
                case Enums.TimeCompareTypes.PastAndNotOlderThan:
                    return dateValue >= this.CalculateIntervalDate(amount.Value * -1, intervalType) && dateValue <= DateTime.Today;
                case Enums.TimeCompareTypes.FutureAndNotNewerThan:
                    return dateValue >= DateTime.Today && dateValue <= this.CalculateIntervalDate(amount.Value, intervalType);
                case Enums.TimeCompareTypes.NotNewerThan:
                    return dateValue <= this.CalculateIntervalDate(amount.Value, intervalType);
                default:
                    return false;
            }
        }

        public virtual IDictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }

        private DateTime CalculateIntervalDate(int amount, string intervalType)
        {
            var today = DateTime.Today;

            switch (this.GetEnumType<Enums.TimeIntervalTypes>(intervalType))
            {
                case Enums.TimeIntervalTypes.Day:
                    return today.AddDays(amount);
                case Enums.TimeIntervalTypes.Month:
                    return today.AddMonths(amount);
                case Enums.TimeIntervalTypes.Year:
                    return today.AddYears(amount);
                default:
                    return default(DateTime);
            }
        }

        private T? GetEnumType<T>(string intervalType)
            where T : struct
        {
            try
            {
                T type;
                if (Enum.TryParse(intervalType, true, out type))
                {
                    return type;
                }

                return null;
            }
            catch (Exception exception)
            {
                this.logger.Error($"Error while exporting form: {exception.Message}", this, exception);
                return null;
            }
        }
    }
}
