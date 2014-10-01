namespace Unic.Flex.Implementation.Database
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using AutoMapper;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Sitecore.Rules.Conditions.ItemConditions;
    using Unic.Flex.Database;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Globalization;
    using Unic.Flex.Implementation.Fields.InputFields;
    using Unic.Flex.Mapping;
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
        /// Initializes a new instance of the <see cref="SaveToDatabaseService" /> class.
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
            // todo: abstract Sitecore.Context
            
            if (Sitecore.Context.User.IsAdministrator) return true;

            var item = Sitecore.Context.Database.GetItem(formId.ToString());
            return Sitecore.Context.User.IsAuthenticated && item.Security.CanWrite(Sitecore.Context.User);
        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        public virtual void ExportForm(Guid formId, string fileName)
        {
            var formRepository = Container.Resolve<IFormRepository>();
            var dictionaryRepository = Container.Resolve<IDictionaryRepository>();
            
            // get the form
            var form = formRepository.LoadForm(formId.ToString());
            if (form == null || form.ItemId != formId) return;

            // get the form data
            var formData = this.unitOfWork.FormRepository.Get(f => f.ItemId == formId).FirstOrDefault();
            if (formData == null || !formData.Sessions.Any()) return;

            // generate worksheet
            var file = new FileInfo(fileName);
            using (var package = new ExcelPackage(file))
            {
                // get worksheet
                var worksheet = package.Workbook.Worksheets.Add(form.Title);

                // get list of field id's added
                var fields = new List<Guid>();

                // add header
                worksheet.Cells[1, 1].Value = dictionaryRepository.GetText("Column Timestamp");
                worksheet.Cells[1, 2].Value = dictionaryRepository.GetText("Column Language");

                var column = 3;
                foreach (var field in form.GetSections().SelectMany(s => s.Fields).Where(f => !string.IsNullOrWhiteSpace(f.Label)))
                {
                    worksheet.Cells[1, column].Value = field.Label;
                    worksheet.Cells[1, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, column].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[1, column].Style.Font.Bold = true;

                    // todo: format the cells somehow different (also don't miss the timestamp and language cell
                    
                    fields.Add(field.ItemId);
                    column++;
                }

                // add content
                var row = 2;
                foreach (var session in formData.Sessions)
                {
                    // todo: correctly format timestamp cell as a date
                    worksheet.Cells[row, 1].Value = session.Timestamp;
                    worksheet.Cells[row, 1].Style.Numberformat.Format = "mm-dd-yy";
                    worksheet.Cells[row, 2].Value = session.Language;

                    column = 3;
                    foreach (var fieldId in fields)
                    {
                        var field = session.Fields.FirstOrDefault(f => f.ItemId == fieldId);
                        var fieldValue = field != null ? field.Value : "-";
                        worksheet.Cells[row, column++].Value = fieldValue;
                    }

                    row++;
                }

                // autofit cells
                worksheet.Cells.AutoFitColumns();

                // save
                package.Save();
            }
        }
    }
}
