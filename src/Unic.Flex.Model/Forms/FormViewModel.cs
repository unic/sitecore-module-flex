namespace Unic.Flex.Model.Forms
{
    using Unic.Flex.Model.Steps;

    //[ModelBinder(typeof(FormModelBinder))]
    public class FormViewModel
    {
        public string Title { get; set; }

        public string Introduction { get; set; }

        public StepViewModel Step { get; set; }
    }
}