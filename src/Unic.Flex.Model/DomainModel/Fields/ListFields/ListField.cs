namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public abstract class ListField<TValue> : FieldBase<TValue>
    {
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
