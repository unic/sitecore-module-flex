namespace Unic.Flex.Implementation.Services
{
    using System;
    using System.Linq;
    using Core.Context;
    using Core.Database;
    using Core.Logging;
    using Core.Mapping;
    using Glass.Mapper.Sc;
    using Model.Forms;
    using Model.Plugs;
    using Plugs.SavePlugs;

    public class DoubleOptinService : IDoubleOptinService
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;

        public DoubleOptinService(ISitecoreContext sitecoreContext, IUnitOfWork unitOfWork, ILogger logger, IFormRepository formRepository)
        {
            this.sitecoreContext = sitecoreContext;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public void ExecuteSubSavePlugs(ISavePlug doubleOptinSavePlug, IFlexContext flexContext, string optInRecordId)
        {
            var item = this.sitecoreContext.GetItem<DoubleOptinSavePlug>(doubleOptinSavePlug.ItemId);
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
                this.logger.Error("Error while executing save plug", this, exception);
            }
        }

        private IForm FillFormWithData(IForm form, string optInRecordId)
        {
            var fields = this.unitOfWork.SessionRepository.GetById(Convert.ToInt32(optInRecordId)).Fields;

            foreach (var field in form.Steps.SelectMany(step => step.Sections).SelectMany(section => section.Fields))
            {
               field.Value = fields.FirstOrDefault(x => x.ItemId == field.ItemId)?.Value;
            }

            return form;
        }
    }
}
