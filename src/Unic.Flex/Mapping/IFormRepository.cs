namespace Unic.Flex.Mapping
{
    using System;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Repository containg data access for the forms.
    /// </summary>
    public interface IFormRepository
    {
        /// <summary>
        /// Loads a form based on the data source from Sitecore.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The loaded form domain model
        /// </returns>
        Form LoadForm(string dataSource);

        /// <summary>
        /// Loads a validator from the data source.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Validator if found</returns>
        IValidator LoadValidator(Guid id);
    }
}
