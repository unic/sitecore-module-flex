namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public abstract class ListField<TValue> : FieldBase<TValue>
    {
        protected ListField()
        {
            this.Items = new List<SelectListItem>();
        }
        
        public IList<SelectListItem> Items { get; private set; }

        public override string TextValue
        {
            get
            {
                var value = string.Empty;
                
                // check the current value
                if (Equals(this.Value, null)) return base.TextValue;
                if (typeof(TValue) == typeof(string[])) value = this.GetTextValue(this.Value as string[]);
                if (typeof(TValue) == typeof(string)) value = this.GetTextValue(this.Value as string);
                
                // return
                return !string.IsNullOrWhiteSpace(value) ? value : base.TextValue;
            }
        }

        private string GetTextValue(string value)
        {
            var selectedItem = this.Items.FirstOrDefault(item => item.Value == value);
            return selectedItem != null ? selectedItem.Text : string.Empty;
        }

        private string GetTextValue(IEnumerable<string> value)
        {
            return string.Join(
                ", ",
                this.Items.Where(item => !string.IsNullOrWhiteSpace(item.Value))
                    .Where(item => value.Contains(item.Value))
                    .Select(item => item.Text));
        }
    }
}
