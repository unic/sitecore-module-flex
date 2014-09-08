namespace Unic.Flex.Implementation.Database
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Unic.Flex.Database;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Entities;
    using Form = Unic.Flex.Model.DomainModel.Forms.Form;

    /// <summary>
    /// Service for saving form to database.
    /// </summary>
    public class SaveToDatabaseServiceService : ISaveToDatabaseService
    {
        /// <summary>
        /// The unit of workThe unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveToDatabaseServiceService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SaveToDatabaseServiceService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Saves the specified form to the database.
        /// </summary>
        /// <param name="form">The form.</param>
        public void Save(Form form)
        {
            Assert.ArgumentNotNull(form,  "form");

            // get/add the form
            var formEntity = this.unitOfWork.FormRepository.Get(f => f.ItemId == form.ItemId).FirstOrDefault();
            if (formEntity == null)
            {
                formEntity = new Model.Entities.Form { ItemId = form.ItemId, Sessions = new Collection<Session>() };
                this.unitOfWork.FormRepository.Insert(formEntity);
            }

            // create current session
            var sessionEntity = new Session { Language = form.Language, Timestamp = DateTime.Now, Fields = new Collection<Field>() };
            formEntity.Sessions.Add(sessionEntity);

            // add fields
            foreach (var field in form.GetSections().SelectMany(section => section.Fields))
            {
                //// todo: handle blobs

                sessionEntity.Fields.Add(new Field { ItemId = field.ItemId, Value = field.TextValue });
            }

            // save data to database
            this.unitOfWork.Save();
        }
    }
}
