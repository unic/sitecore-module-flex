namespace Unic.Flex.Model.DomainModel.Fields
{
    using System.Collections.Generic;
    using Glass.Mapper.Configuration.Attributes;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Base class for all available fields
    /// </summary>
    public abstract class FieldBase : ItemBase, IPresentationComponent
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [SitecoreField("Label")]
        public virtual string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this field is required; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Is Required")]
        public virtual bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Field is required")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the validators.
        /// </summary>
        /// <value>
        /// The validators.
        /// </value>
        [SitecoreQuery("./Validators/*", InferType = true, IsRelative = true, IsLazy = true)]
        public virtual IEnumerable<IValidator> Validators { get; set; }
    }
}
