namespace Unic.Flex.Implementation.Commands
{
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Shell.Applications.Dialogs.ProgressBoxes;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web.UI.Sheer;
    using Sitecore.Web.UI.XamlSharp.Continuations;
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Mvc;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Model.Forms;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

    /// <summary>
    /// Command to export form data from databae to excel
    /// </summary>
    public class DatabasePlugExport : Command, ISupportsContinuation
    {
        /// <summary>
        /// The save to database service
        /// </summary>
        private ISaveToDatabaseService saveToDatabaseService;

        /// <summary>
        /// The logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The dictionary repository
        /// </summary>
        private IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// The context service
        /// </summary>
        private IContextService contextService;

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

            // initialize
            this.Initialize();

            using (new LanguageSwitcher(item.Language))
            {
                // generate the excel
                var form = this.contextService.LoadForm(item.ID.ToString());
                if (form == null)
                {
                    Context.ClientPage.ClientResponse.Alert(string.Format("Could not load form in language '{0}'", Context.Language.Name));
                    return;
                }

                var filePath = this.GetTempFileName();
                ProgressBox.Execute(this.dictionaryRepository.GetText("Exporting form"), this.dictionaryRepository.GetText("Flex Form Export"), this.Export, form, filePath);
            
                // dowload the document
                var fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                var hash = SecurityUtil.GetMd5Hash(MD5.Create(), string.Join("_", form.ItemId, fileName));
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var downloadUrl = urlHelper.Action("DatabasePlugExport", "Flex", new { formId = form.ItemId, fileName = fileName, hash = hash, sc_lang = item.Language });
                SheerResponse.SetLocation(downloadUrl);
            }
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
            var service = DependencyResolver.Resolve<ISaveToDatabaseService>();

            // check if the user has permission to export the form
            if (!service.HasExportPermissions(itemId))
            {
                return CommandState.Disabled;
            }

            // check if the current form has entries to export in the database
            if (!service.HasEntries(itemId))
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
                var form = (IForm)arguments[0];
                var fileName = (string)arguments[1];
                this.saveToDatabaseService.ExportForm(form, fileName);
            }
            catch (Exception exception)
            {
                this.logger.Error(string.Format("Error while exporting form: {0}", exception.Message), this, exception);
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

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            using (new DatabaseSwitcher(Factory.GetDatabase("master")))
            {
                this.saveToDatabaseService = DependencyResolver.Resolve<ISaveToDatabaseService>();
                this.logger = DependencyResolver.Resolve<ILogger>();
                this.dictionaryRepository = DependencyResolver.Resolve<IDictionaryRepository>();
                this.contextService = DependencyResolver.Resolve<IContextService>();
            }
        }
    }
}
