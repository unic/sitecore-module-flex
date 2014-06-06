namespace Unic.Flex.Implementation.Validators
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    [SitecoreType(TemplateId = "{0E0AAF18-7D98-4A59-AF1F-45A0652DF4C9}")]
    public class RegularExpressionValidator : IValidator
    {
        /// <summary>
        /// The validation message dictionary key
        /// </summary>
        public const string ValidationMessageDictionaryKey = "Input is invalid";

        [SitecoreDictionaryFallbackField("Validation Message", ValidationMessageDictionaryKey)]
        public virtual string ValidationMessage { get; set; }

        public bool IsValid(object value)
        {
            return false;
        }

        public IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            //attributes.Add("data-val-number", this.ValidationMessage);
            return attributes;
        }
    }
}
