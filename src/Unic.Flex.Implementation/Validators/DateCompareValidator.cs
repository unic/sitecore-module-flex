namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using Core.Context;
    using Core.DependencyInjection;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Model.Fields;
    using Model.Forms;
    using Model.GlassExtensions.Attributes;
    using Model.Validators;
    using Sitecore.Data;

    /// <summary>
    /// Validator that compare two dates with selected compare type.
    /// </summary>
    [SitecoreType(TemplateId = "{BD19EA8F-BCA6-44DB-AD3E-1BBFC3E0ED50}")]
    public class DateCompareValidator : IValidator
    {
        private readonly Dictionary<CompareTypes, Func<DateTime, DateTime, bool>> Comparators
            = new Dictionary<CompareTypes, Func<DateTime, DateTime, bool>>
            {
                { CompareTypes.Bigger, (firstDate,secondDate) => firstDate > secondDate },
                { CompareTypes.BiggerOrEqual, (firstDate,secondDate) => firstDate >= secondDate },
                { CompareTypes.Smaller,  (firstDate, secondDate) => firstDate < secondDate },
                { CompareTypes.SmallerOrEqual, (firstDate,secondDate) => firstDate <= secondDate },
                { CompareTypes.Equal, (firstDate, secondDate) => firstDate == secondDate },
                { CompareTypes.Default, (firstDate, secondDate) => false }
            };

        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public string DefaultValidationMessageDictionaryKey { get; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "First date is smaller")]
        public string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the other field model.
        /// </summary>
        /// <value>
        /// The other field model.
        /// </value>
        [SitecoreReusableField("Date to Compare", Setting = SitecoreFieldSettings.InferType)]
        public virtual IField OtherFieldModel { get; set; }

        /// <summary>
        /// Gets or sets fields compare type.
        /// </summary>
        /// <value>
        /// Field compare type.
        /// </value>
        [SitecoreField("Compare Type", FieldId = "{DD9189E1-4217-4F40-B0DF-81ABE21EB9CE}")]
        public virtual string CompareTypeField { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public bool IsValid(object value)
        {
            DateTime firstDate = DateTime.MinValue, secondDate = DateTime.MinValue;
            if (value is DateTime)
            {
                firstDate = (DateTime)value;
            }

            var context = DependencyResolver.Resolve<IFlexContext>();
            var otherField = context.Form.GetFieldValue(this.OtherFieldModel);
            if (!DateTime.TryParse(otherField, out secondDate))
            {
                return false;
            }

            if (firstDate == DateTime.MinValue || secondDate == DateTime.MinValue)
            {
                return false;
            }

            var item = Sitecore.Context.Database.GetItem(new ID(this.CompareTypeField));

            var compareType = this.GetCompareType(item.Name.Replace(" ", string.Empty));
            var comparator = this.Comparators[compareType];

            var isValid = comparator(firstDate, secondDate);

            return isValid;
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public IDictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Parse selected compare type.
        /// </summary>
        /// <param name="compareType">Compare type name.</param>
        /// <returns>Predefined enum value of compare types.</returns>
        private CompareTypes GetCompareType(string compareType)
        {
            CompareTypes type;
            if (Enum.TryParse(compareType, true, out type))
            {
                return type;
            }

            return CompareTypes.Default;
        }
    }

    /// <summary>
    /// Predefined compare types.
    /// </summary>
    internal enum CompareTypes
    {
        Bigger,
        BiggerOrEqual,
        Equal,
        Smaller,
        SmallerOrEqual,
        Default
    }
}
