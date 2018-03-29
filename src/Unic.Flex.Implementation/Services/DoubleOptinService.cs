namespace Unic.Flex.Implementation.Services
{
    using Glass.Mapper.Sc;
    using Model;
    using Model.Forms;
    using Model.Plugs;
    using Plugs.SavePlugs;

    public class DoubleOptinService : IDoubleOptinService
    {
        private readonly ISitecoreContext sitecoreContext;

        public DoubleOptinService(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }
        public void ExecuteSubSavePlugs(ISavePlug saveplug, IForm form)
        {
            var item = this.sitecoreContext.GetItem<DoubleOptinSavePlug>(saveplug.ItemId);
            var saveplugs = item.SavePlugs;

            foreach (var savePlug in saveplugs)
            {
               form = this.FillFormWithData(form);
               savePlug.Execute(form);
            }
        }

        private IForm FillFormWithData(IForm form)
        {
            foreach (var formStep in form.Steps)
            {
                foreach (var formStepSection in formStep.Sections)
                {
                    foreach (var field in formStepSection.Fields)
                    {
                        field.Value = this.sitecoreContext.GetItem<object>(field.ItemId);
                    }
                }
            }

            return form;

        }
    }
}
