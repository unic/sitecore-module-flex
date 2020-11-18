namespace Unic.Flex.Model.ViewModels
{
    using Presentation;

    public class DatabasePlugExportDownloadViewModel : IPresentationComponent
    {
        public string DownloadUrl { get; set; }

        public string DownloadLinkText { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public string MetaTitle { get; set; }

        public string ViewName => "~/Views/Modules/Flex/Default/Components/DatabasePlugExportDownload.cshtml";
    }
}
