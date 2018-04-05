namespace Unic.Flex.Implementation.Services
{
    using Model.Forms;
    using Model.Plugs;

    public interface IDoubleOptinService
    {
        void ExecuteSubSavePlugs(ISavePlug saveplug, IForm form, string optInFormId, string optInRecordId, string emailId, string optInHash);
    }
}
