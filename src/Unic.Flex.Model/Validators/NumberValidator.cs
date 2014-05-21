using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.Validators
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    [SitecoreType(TemplateId = "{29129AFA-3651-4A7F-BA87-CF1DEEDB48A5}")]
    public class NumberValidator : IValidator
    {
        public virtual bool IsValid(object value)
        {
            if (value == null) return false;

            try
            {
                var numberValue = Convert.ToInt32(value);
                if (this.NumberRangeStart > 0 && numberValue < this.NumberRangeStart) return false;
                if (this.NumberRangeEnd > 0 && numberValue > this.NumberRangeEnd) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-number", this.ValidationMessage);
            // todo: handle range attributes
            return attributes;
        }

        [SitecoreDictionaryFallbackField("Validation Message", "Please enter a valid number")]
        public virtual string ValidationMessage { get; set; }

        [SitecoreField("Number Range Start")]
        public virtual int NumberRangeStart { get; set; }

        [SitecoreField("Number Range End")]
        public virtual int NumberRangeEnd { get; set; }
    }
}
