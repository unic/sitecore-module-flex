namespace Unic.Flex.Implementation.Commands
{
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Web.UI.XamlSharp.Continuations;
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Globalization;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Logging;

    /// <summary>
    /// Command to export form data from databae to excel
    /// </summary>
    public class DatabasePlugExport : Command, ISupportsContinuation
    {
        /// <summary>
        /// The save to database service
        /// </summary>
        private readonly ISaveToDatabaseService saveToDatabaseService;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePlugExport"/> class.
        /// </summary>
        public DatabasePlugExport()
        {
            using (new DatabaseSwitcher(Factory.GetDatabase("master")))
            {
                this.saveToDatabaseService = Container.Resolve<ISaveToDatabaseService>();
                this.logger = Container.Resolve<ILogger>();
                this.dictionaryRepository = Container.Resolve<IDictionaryRepository>();
            }
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            
            // get the item to process
            var item = context.Items.FirstOrDefault();
            Assert.IsNotNull(item, "Item must not be null");

            // generate the excel
            var itemId = item.ID.ToGuid();
            var fileName = this.GetTempFileName();
            ProgressBox.Execute(this.dictionaryRepository.GetText("Exporting form"), this.dictionaryRepository.GetText("Flex Form Export"), this.Export, itemId, fileName);
            
            // dowload the document
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var downloadUrl = urlHelper.Action("DatabasePlugExport", "Flex", new { formId = itemId, fileName = fileName });
            SheerResponse.SetLocation(downloadUrl);
        }

        /// <summary>
        /// Queries the state.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>State of the command</returns>
        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            // get the item to process
            var item = context.Items.FirstOrDefault();
            Assert.IsNotNull(item, "Item must not be null");

            var itemId = item.ID.ToGuid();

            // check if the user has permission to export the form
            if (!this.saveToDatabaseService.HasExportPermissions(itemId))
            {
                return CommandState.Disabled;
            }

            // check if the current form has entries to export in the database
            if (!this.saveToDatabaseService.HasEntries(itemId))
            {
                return CommandState.Disabled;
            }

            return base.QueryState(context);
        }

        /// <summary>
        /// Exports the form to the filesystem.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        protected virtual void Export(params object[] arguments)
        {
            try
            {
                var formId = (Guid)arguments[0];
                var fileName = (string)arguments[1];
                this.saveToDatabaseService.ExportForm(formId, fileName);
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Error while exporting form: {0}", exception.Message), exception);
            }
        }

        /// <summary>
        /// Gets the name of a temporary file.
        /// </summary>
        /// <returns>New temp file</returns>
        protected virtual string GetTempFileName()
        {
            string path;
            do
            {
              path = Path.Combine(MainUtil.MapPath(Settings.TempFolderPath), Path.GetRandomFileName());
            }
            while (File.Exists(path));
        
            return path;
        }
    }
}
