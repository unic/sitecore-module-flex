namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;

    /// <summary>
    /// Plug for saving data to the database.
    /// </summary>
    [SitecoreType(TemplateId = "{77613F9B-AC9A-4229-B97A-ACD6FBFCB252}")]
    public class SaveToDatabase : SavePlugBase
    {
        /// <summary>
        /// The save to database service
        /// </summary>
        private readonly ISaveToDatabaseService saveToDatabaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveToDatabase"/> class.
        /// </summary>
        /// <param name="saveToDatabaseService">The save to database service.</param>
        public SaveToDatabase(ISaveToDatabaseService saveToDatabaseService)
        {
            this.saveToDatabaseService = saveToDatabaseService;
        }
        
        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");
            this.saveToDatabaseService.Save(form);
        }
    }
}
