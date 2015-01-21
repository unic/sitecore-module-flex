namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Field for adding autocompletion.
    /// </summary>
    [SitecoreType(TemplateId = "{A9FD64D4-F178-498F-9BD1-D6264FD4B452}")]
    public class AutoCompleteField : InputField<string>
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override string DefaultValue { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public virtual IList<string> Items
        {
            get
            {
                return this.DataProvider != null ? this.DataProvider.GetItems().Select(item => item.Text).ToList() : new List<string>();
            }
        }

        /// <summary>
        /// Gets or sets the items provider.
        /// </summary>
        /// <value>
        /// The items provider.
        /// </value>
        [SitecoreSharedField("Data Provider", Setting = SitecoreFieldSettings.InferType)]
        public virtual IDataProvider<ListItem> DataProvider { get; set; }
    }
}
