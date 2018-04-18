namespace Unic.Flex.Implementation.Services
{
    using System;
    using System.Linq;
    using System.Web;
    using Core.Database;
    using Core.Logging;
    using Core.Utilities;
    using Glass.Mapper.Sc;
    using Model;

    public class DoubleOptinLinkService : IDoubleOptinLinkService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly ISitecoreContext sitecoreContext;

        public DoubleOptinLinkService(IUnitOfWork unitOfWork, ILogger logger, ISitecoreContext sitecoreContext)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.sitecoreContext = sitecoreContext;
        }
        public string CreateConfirmationLink(string formId, string toEmail, string optInRecordId)
        {
            var optInHash = this.CreateOptInHash(optInRecordId, toEmail, formId);
            var url = this.sitecoreContext.GetCurrentItem<ItemBase>().Url;
            var link = $"{url}?{Definitions.Constants.ScActionQueryKey}={Definitions.Constants.OptionQueryKey}&{Definitions.Constants.OptInFormIdKey}={Uri.EscapeDataString(formId)}&{Definitions.Constants.OptInRecordIdKey}={optInRecordId}&{Definitions.Constants.OptInHashKey}={Uri.EscapeDataString(optInHash)}";

            return link;
        }

        public bool ValidateConfirmationLink(string optInFormId, string optInRecordId, string email, string optInHashFromLink)
        {
            var optInHash = this.CreateOptInHash(optInRecordId, email, optInFormId);
            if (optInHash != optInHashFromLink)
            {
                this.logger.Warn($"DoubleOptinLinkService.ValidateConfirmationLink: OptInHash did not match. OptInHash={optInHashFromLink} OptInRecordId={optInRecordId} Email:{email}", this);
                return false;
            }

            if (!this.unitOfWork.FormRepository.Any(x => x.ItemId.ToString() == optInFormId))
            {
                this.logger.Warn($"DoubleOptinLinkService.ValidateConfirmationLink: No form with this ID was found. OptInFormID={optInFormId}", this);
                return false;
            }

            if (!this.unitOfWork.SessionRepository.Any(x => x.Id.ToString() == optInRecordId))
            {
                this.logger.Warn($"DoubleOptinLinkService.ValidateConfirmationLink: No session with this ID was found. OptInRecordId={optInRecordId}", this);
                return false;
            }

            return true;
        }

        private string CreateOptInHash(string optInRecordId, string email, string formId)
        {
            var stringToHash = $"{optInRecordId}|{email}|{formId}";
            return SecurityUtil.GenerateHash(stringToHash);
        }
    }
}
