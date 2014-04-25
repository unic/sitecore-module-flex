using ISitecoreContext = Glass.Mapper.Sc.ISitecoreContext;

namespace Unic.Flex.Service.Context
{
    using System;
    using Unic.Flex.DomainModel.Forms;
    using Unic.Flex.Service.Glass;

    public class ContextService : IContextService
    {
        private readonly IFormRepository formRepository;

        public ContextService(IFormRepository formRepository)
        {
            this.formRepository = formRepository;
        }
        
        public IForm LoadForm(string dataSource, ISitecoreContext sitecoreContext)
        {
            return this.formRepository.LoadForm(dataSource, sitecoreContext);
        }
    }
}
