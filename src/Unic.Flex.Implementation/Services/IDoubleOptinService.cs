namespace Unic.Flex.Implementation.Services
{
    using Core.Context;
    using Model.Plugs;

    public interface IDoubleOptinService
    {
        void ExecuteSubSavePlugs(ISavePlug saveplug, IFlexContext form, string optInRecordId);

        bool ValidateConfirmationLink(string optInFormId, string optInRecordId, string email, string optInHashFromLink);
    }
}
