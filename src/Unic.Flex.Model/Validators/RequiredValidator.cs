using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.Validators
{
    public class RequiredValidator : IValidator
    {
        public virtual bool IsValid(object value)
        {
            if (value == null) return false;

            var stringValue = value as string;
            return stringValue != null && !string.IsNullOrWhiteSpace(stringValue);
        }

        public IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-required", this.ValidationMessage);
            return attributes;
        }

        public virtual string ValidationMessage { get; set; }
    }
}
