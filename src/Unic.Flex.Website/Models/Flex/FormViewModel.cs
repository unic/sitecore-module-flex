namespace Unic.Flex.Website.Models.Flex
{
    using System.Web.Mvc;
    using Unic.Flex.DomainModel.Steps;

    [ModelBinder(typeof(FlexModelBinder))]
    public class FormViewModel
    {
        public string Title { get; set; }

        public string Introduction { get; set; }

        public IStepBase Step { get; set; }
    }
}