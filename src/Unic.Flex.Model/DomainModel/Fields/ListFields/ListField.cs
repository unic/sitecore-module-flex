namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public abstract class ListField<TValue> : FieldBase<TValue>
    {
        protected ListField()
        {
            this.Items = new List<SelectListItem>();
        }
        
        public IList<SelectListItem> Items { get; private set; }
    }
}
