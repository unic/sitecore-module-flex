namespace Unic.Flex.Implementation.Database
{
    using AutoMapper;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Unic.Flex.Database;
    using Unic.Flex.Globalization;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Model.DomainModel.Fields.ListFields;
    using Unic.Flex.Model.Entities;
    using Unic.Flex.Utilities;
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
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveToDatabaseService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dictionaryRepository">The dictionary repository.</param>
        public SaveToDatabaseService(IUnitOfWork unitOfWork, IDictionaryRepository dictionaryRepository)
        {
            this.unitOfWork = unitOfWork;
            this.dictionaryRepository = dictionaryRepository;
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
                    fieldEntity.File = Mapper.Map<File>(fileUploadField.Value);
                }

                sessionEntity.Fields.Add(fieldEntity);
            }

            // save data to database
            this.unitOfWork.Save();
        }

        /// <summary>
        /// Determines whether the specified form identifier has entries.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>
        /// Boolean value wheather the form has entries in the database.
        /// </returns>
        public virtual bool HasEntries(Guid formId)
        {
            var form = this.unitOfWork.FormRepository.Get(f => f.ItemId == formId).FirstOrDefault();
            if (form == null) return false;

            return form.Sessions != null && form.Sessions.Any();
        }

        /// <summary>
        /// Determines whether the curent user has permission to export the given form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>
        /// Boolean value wheather the user has permission to export the form.
        /// </returns>
        public virtual bool HasExportPermissions(Guid formId)
        {
            if (Sitecore.Context.User.IsAdministrator) return true;

            var item = Sitecore.Context.Database.GetItem(formId.ToString());
            return Sitecore.Context.User.IsAuthenticated && item.Security.CanWrite(Sitecore.Context.User);
        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="fileName">Name of the file.</param>
        public virtual void ExportForm(Form form, string fileName)
        {
            Assert.ArgumentNotNull(form, "form");
            
            // get the form data
            var formData = this.unitOfWork.FormRepository.Get(f => f.ItemId == form.ItemId).FirstOrDefault();
            if (formData == null || !formData.Sessions.Any()) return;

            // generate worksheet
            var file = new FileInfo(fileName);
            using (var package = new ExcelPackage(file))
            {
                // get worksheet
                var worksheet = package.Workbook.Worksheets.Add(form.Title);

                // get list of field id's added
                var fields = new List<FieldItem>();

                // add header
                worksheet.Cells[1, 1].Value = this.dictionaryRepository.GetText("Column Timestamp");
                worksheet.Cells[1, 2].Value = this.dictionaryRepository.GetText("Column Language");

                var column = 3;
                foreach (var field in form.GetSections().SelectMany(s => s.Fields).Where(f => !string.IsNullOrWhiteSpace(f.Label)))
                {
                    worksheet.Cells[1, column].Value = field.Label;
                    fields.Add(new FieldItem { Id = field.ItemId, Type = field.GetType() });
                    column++;
                }

                // format header row
                var cells = worksheet.Cells[1, 1, 1, column - 1];
                cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                cells.Style.Font.Bold = true;

                // add content
                var row = 2;
                foreach (var session in formData.Sessions)
                {
                    worksheet.Cells[row, 1].Value = session.Timestamp;
                    worksheet.Cells[row, 1].Style.Numberformat.Format = "m/d/yy h:mm";
                    worksheet.Cells[row, 2].Value = session.Language;

                    column = 3;
                    foreach (var item in fields)
                    {
                        var field = session.Fields.FirstOrDefault(f => f.ItemId == item.Id);
                        if (field == null) continue;

                        worksheet.Cells[row, column++].Value = this.GetExportValue(item, field);
                    }

                    row++;
                }

                // autofit cells
                worksheet.Cells.AutoFitColumns();
                worksheet.Column(1).Width += 2;

                // save
                package.Save();
            }
        }

        /// <summary>
        /// Gets the export value.
        /// </summary>
        /// <param name="fieldItem">The field item.</param>
        /// <param name="field">The field.</param>
        /// <returns>The formatted exported value</returns>
        private string GetExportValue(FieldItem fieldItem, Field field)
        {
            if (fieldItem == null) return "-";
            if (TypeHelper.IsSubclassOfRawGeneric(typeof(ListField<>), fieldItem.Type)) return field.Value.Replace(Environment.NewLine, ", ");
            return field.Value;
        }

        /// <summary>
        /// A container class for a field item.
        /// </summary>
        private class FieldItem
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public Type Type { get; set; }
        }
    }
}
