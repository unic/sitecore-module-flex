namespace Unic.Flex.Context
{
    using Glass.Mapper.Sc;
    using Unic.Flex.DomainModel.Forms;
    using Unic.Flex.Mapping;

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
