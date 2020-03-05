namespace Unic.Flex.Implementation.Services
{
    using System;
    using System.Linq;
    using Core.Context;
    using Core.Database;
    using Core.Logging;
    using Core.Mapping;
    using Fields.InputFields;
    using Glass.Mapper;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Web;
    using Model.Forms;
    using Model.Plugs;
    using Model.Types;
    using Plugs.SavePlugs;

    public class DoubleOptinService : IDoubleOptinService
    {
        private readonly IRequestContext requestContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;

        public DoubleOptinService(IRequestContext requestContext, IUnitOfWork unitOfWork, ILogger logger, IFormRepository formRepository)
        {
            this.requestContext = requestContext;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public void ExecuteSubSavePlugs(ISavePlug doubleOptinSavePlug, IFlexContext flexContext, string optInRecordId)
        {
            var options = new GetItemByIdOptions(doubleOptinSavePlug.ItemId)
            {
                Lazy = LazyLoading.Enabled
            };
            var item = this.requestContext.SitecoreService.GetItem<DoubleOptinSavePlug>(options);
            var saveplugs = item.SavePlugs;

            try
            {
                flexContext.Form = this.FillFormWithData(flexContext.Form, optInRecordId);

                foreach (var savePlug in saveplugs)
                {
                    savePlug.Execute(flexContext.Form);
                }
            }
            catch (Exception exception)
            {
                flexContext.ErrorMessage = flexContext.Form.ErrorMessage;
                this.logger.Error($"Error while executing Sub SavePlug for Double Optin SavePlug with Id: {doubleOptinSavePlug.ItemId}", this, exception);
            }
        }

        private IForm FillFormWithData(IForm form, string optInRecordId)
        {
            var fields = this.unitOfWork.SessionRepository.GetById(Convert.ToInt32(optInRecordId)).Fields;

            foreach (var field in form.Steps.SelectMany(step => step.Sections).SelectMany(section => section.Fields))
            {
                if (field is FileUploadField)
                {
                    field.Value = fields.FirstOrDefault(x => x.ItemId == field.ItemId)?.File;
                }
                else
                {
                    field.Value = fields.FirstOrDefault(x => x.ItemId == field.ItemId)?.Value;
                }
            }

            return form;
        }
    }
}
