namespace Unic.Flex.Core.ModelBinding
{
    using System.IO;
    using System.Web.Mvc;
    using Unic.Flex.Model.Types;

    /// <summary>
    /// Bind an unploaded file to a custom class.
    /// </summary>
    public class UploadedFileModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var postedFile = controllerContext.HttpContext.Request.Files[bindingContext.ModelName];
            if (postedFile == null) return null;
            if (postedFile.ContentLength == 0 && string.IsNullOrWhiteSpace(postedFile.FileName)) return null;

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

            return uploadedFile;
        }
    }
}
