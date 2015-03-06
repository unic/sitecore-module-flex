namespace Unic.Flex.Core.Mapping
{
    using Unic.Flex.Core.Context;
    using Unic.Flex.Model.Forms;

    public interface IViewMapper
    {
        void MapActiveStep(IFlexContext context);
    }
}
