namespace Unic.Flex.Implementation.Services
{
    using System;
    using System.Linq;
    using Core.Database;
    using Core.Logging;
    using Core.Mapping;
    using Core.Utilities;
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
        public void ExecuteSubSavePlugs(ISavePlug saveplug, IForm form, string optInFormId, string optInRecordId, string email, string optInHash)
        {
            if (!this.ValidateConfirmationLink(optInFormId, optInRecordId, email, optInHash)) return;

            var item = this.sitecoreContext.GetItem<DoubleOptinSavePlug>(saveplug.ItemId);
            var saveplugs = item.SavePlugs;

            foreach (var savePlug in saveplugs)
            {
               form = this.FillFormWithData(form, optInRecordId);
               savePlug.Execute(form);
            }
        }

        private bool ValidateConfirmationLink(string optInFormId, string optInRecordId, string email, string optInHashFromLink)
        {
            var optInHash = this.CreateOptInHash(optInRecordId, email, optInFormId);
            if (optInHash != optInHashFromLink)
            {
                this.logger.Warn($"OptinService.ValidateConfirmationLink: OptInHash did not match. OptInHash={optInHashFromLink} OptInRecordId={optInRecordId} Email:{email}", this);
                return false;
            }

            if (!this.unitOfWork.FormRepository.Any(x => x.ItemId.ToString() == optInFormId))
            {
                this.logger.Warn($"OptinService.ValidateConfirmationLink: No form with this ID was found. OptInFormID={optInFormId}", this);
                return false;
            }

            if (!this.unitOfWork.SessionRepository.Any(x => x.Id.ToString() == optInRecordId))
            {
                this.logger.Warn($"OptinService.ValidateConfirmationLink: No session with this ID was found. OptInRecordId={optInRecordId}", this);
                return false;
            }

            return true;
        }

        private IForm FillFormWithData(IForm form, string optInRecordId)
        {
            var fields = this.unitOfWork.SessionRepository.GetById(Convert.ToInt32(optInRecordId)).Fields;

            foreach (var formStep in form.Steps)
            {
               foreach (var formStepSection in formStep.Sections)
                {
                    foreach (var field in formStepSection.Fields)
                    {
                        field.Value = fields.FirstOrDefault(x => x.ItemId == field.ItemId)?.Value;
                    }
               }
            }

            return form;
        }

        private string CreateOptInHash(string optInRecordId, string email, string formId)
        {
            var salt = Sitecore.Configuration.Settings.GetSetting("Flex.DoubleOptin.Salt");
            var stringToHash = $"{optInRecordId}|{email}|{formId}";
            return SecurityUtil.GenerateHash(stringToHash, salt);
        }
    }
}
