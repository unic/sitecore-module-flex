namespace Unic.Flex.Model.ViewModel.Forms
{
    using Unic.Flex.Model.ViewModel.Steps;

    public interface IFormViewModel
    {
        IStepViewModel Step { get; set; }

        string Id { get; set; }

        string Language { get; set; }
    }
}
