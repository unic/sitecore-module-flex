namespace Unic.Flex.Implementation.Services
{
    using Core.Context;
    using Model.Plugs;

    public interface IDoubleOptinService
    {
        void ExecuteSubSavePlugs(ISavePlug doubleOptinSavePlug, IFlexContext form, string optInRecordId);
    }
}
