﻿namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Plugs.SavePlugs;

    /// <summary>
    /// Plug for saving data to the database.
    /// </summary>
    [SitecoreType(TemplateId = "{77613F9B-AC9A-4229-B97A-ACD6FBFCB252}")]
    public class SaveToDatabase : SavePlugBase
    {
        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            Log.Info("FLEX :: Save to Database Execute() called", this);
        }
    }
}
