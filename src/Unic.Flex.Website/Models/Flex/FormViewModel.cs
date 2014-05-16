namespace Unic.Flex.Website.Models.Flex
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Flex.Website.ModelBinding;

    [ModelBinder(typeof(FormModelBinder))]
    public class FormViewModel
    {
        public string Title { get; set; }

        public string Introduction { get; set; }

        public StepViewModel Step { get; set; }
    }
}