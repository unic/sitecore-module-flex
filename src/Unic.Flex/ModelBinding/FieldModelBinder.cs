﻿namespace Unic.Flex.ModelBinding
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Unic.Flex.Definitions;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// Bind fields
    /// </summary>
    public class FieldModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// The initial value of the model
        /// </summary>
        private object initialValue;

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get initial values
            var initialModel = (IFieldViewModel)bindingContext.Model;
            this.initialValue = initialModel.Value;

            // bind the model with the default model binder or directly request file from post
            object model = null;
            if (initialModel.GetType().GetProperty(Constants.ValueIdSuffix).PropertyType == typeof(UploadedFile))
            {
                var postedFile = controllerContext.HttpContext.Request.Files[string.Join(".", bindingContext.ModelName, Constants.ValueIdSuffix)];
                if (postedFile != null)
                {
                    var uploadedFile = new UploadedFile
                                {
                                    ContentLength = postedFile.ContentLength,
                                    ContentType = postedFile.ContentType,
                                    FileName = postedFile.FileName
                                };

                    using (var stream = new MemoryStream())
                    {
                        postedFile.InputStream.CopyTo(stream);
                        uploadedFile.Data = stream.ToArray();
                    }

                    model = uploadedFile;
                }
            }
            else
            {
                model = base.BindModel(controllerContext, bindingContext);   
            }

            // return the binded model if it could be binded
            if (model != null) return model;

            // otherwise no values has been posted -> reset the field value, do validation and return the initial model
            initialModel.Value = null;
            ForceModelValidation(bindingContext);
            return initialModel;
        }

        /// <summary>
        /// Called when the model is updated.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);

            // set the file uploaded to the initial file (because it maybe wasn't posted but was posted before)
            var model = (IFieldViewModel)bindingContext.Model;
            if (model.Value == null && model.Type == typeof(UploadedFile))
            {
                model.Value = this.initialValue;
            }

            ForceModelValidation(bindingContext);
        }

        /// <summary>
        /// Forces the model validation.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        private static void ForceModelValidation(ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model as IValidatableObject;
            if (model == null) return;

            var modelName = bindingContext.ModelName;
            var modelState = bindingContext.ModelState;
            var errors = model.Validate(new ValidationContext(model, null, null));
            foreach (var error in errors)
            {
                foreach (var memberName in error.MemberNames)
                {
                    modelState.AddModelError(string.Join(".", modelName, memberName), error.ErrorMessage);
                }
            }
        }
    }
}
