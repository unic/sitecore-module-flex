namespace Unic.Flex.Plugs
{
    using System.Linq;
    using Sitecore.Diagnostics;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Sections;

    public class PlugsService : IPlugsService
    {
        private readonly IUserDataRepository userDataRepository;
        
        public PlugsService(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }
        
        public void ExecuteLoadPlugs(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            // check if we need to execute the load plugs -> only the first time
            if (this.userDataRepository.IsFormStored(form.Id)) return;

            // todo: add exception handling
            foreach (var plug in form.LoadPlugs)
            {
                plug.Execute(form);
            }
        }
    }
}
