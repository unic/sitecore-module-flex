﻿namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// A hidden input field
    /// </summary>
    [SitecoreType(TemplateId = "{C2E610E1-B3C7-4BDD-8E5C-F1869BFF461A}")]
    public class HiddenField : InputField<string>
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override string DefaultValue { get; set; }
    }
}
