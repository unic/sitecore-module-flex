namespace Unic.Flex.Implementation.Database
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Sitecore.Diagnostics;
    using Unic.Flex.Database;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Model.Entities;
    using File = Unic.Flex.Model.Entities.File;
    using Form = Unic.Flex.Model.DomainModel.Forms.Form;

    /// <summary>
    /// Service for saving form to database.
    /// </summary>
    public class SaveToDatabaseService : ISaveToDatabaseService
    {
        /// <summary>
        /// The unit of workThe unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveToDatabaseService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SaveToDatabaseService(IUnitOfWork unitOfWork)
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
                // create field entity
                var fieldEntity = new Field { ItemId = field.ItemId, Value = field.TextValue };
                
                // add blobs
                var fileUploadField = field as FileUploadField;
                if (fileUploadField != null && fileUploadField.Value != null)
                {
                    fieldEntity.File = new File
                                           {
                                               ContentLength = fileUploadField.Value.ContentLength,
                                               ContentType = fileUploadField.Value.ContentType,
                                               FileName = fileUploadField.Value.FileName
                                           };

                    var stream = new MemoryStream();
                    fileUploadField.Value.InputStream.CopyTo(stream);
                    fieldEntity.File.Data = stream.ToArray();
                }

                sessionEntity.Fields.Add(fieldEntity);
            }

            // save data to database
            this.unitOfWork.Save();
        }
    }
}
