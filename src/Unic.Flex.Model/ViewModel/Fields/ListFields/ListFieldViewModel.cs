namespace Unic.Flex.Model.ViewModel.Fields.ListFields
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public abstract class ListFieldViewModel<TValue> : FieldBaseViewModel<TValue>
    {
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}
