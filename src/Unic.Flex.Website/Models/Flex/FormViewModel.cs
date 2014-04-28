namespace Unic.Flex.Website.Models.Flex
{
    using Unic.Flex.DomainModel.Steps;

    public class FormViewModel
    {
        public string Title { get; set; }

        public string Introdcution { get; set; }

        public IStepBase Step { get; set; }
    }
}