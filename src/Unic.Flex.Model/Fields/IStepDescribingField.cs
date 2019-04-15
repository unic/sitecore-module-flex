namespace Unic.Flex.Model.Fields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{1BF1A0C5-3FC4-4A6B-A30A-EFED98B5FD4A}")]
    public interface IStepDescribingField
    { 
        /// <summary>
        /// Gets or sets the value used in aria-describedby field of the submit button.
        /// </summary>
        [SitecoreField("Describes the Form Step")]
        bool DescribesFormStep { get; set; }
    }
}
