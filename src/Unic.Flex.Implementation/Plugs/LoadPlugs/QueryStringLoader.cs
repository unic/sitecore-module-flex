namespace Unic.Flex.Implementation.Plugs.LoadPlugs
{
    using System.Linq;
    using System.Web;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Ninject;
    using Sitecore.Diagnostics;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.DomainModel.Plugs.LoadPlugs;
    using Unic.Flex.Model.DomainModel.Sections;

    /// <summary>
    /// Load plug to load values from querystring into the form fields
    /// </summary>
    [SitecoreType(TemplateId = "{4906DF7C-B200-4825-B1AC-488D7D928452}")]
    public class QueryStringLoader : LoadPlugBase
    {
        // todo: move all plug, fields, etc. implementation to custom assembly -> we have a circulate project references else

        /// <summary>
        /// Gets or sets the user data repository.
        /// </summary>
        /// <value>
        /// The user data repository.
        /// </value>
        [Inject]
        public IUserDataRepository userDataRepository { private get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringLoader"/> class.
        /// </summary>
        public QueryStringLoader()
        {
            Container.Kernel.Inject(this);
        }

        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            var queryString = HttpContext.Current.Request.QueryString;
            foreach (var section in form.Steps.SelectMany(step => step.Sections))
            {
                var standardSection = section as StandardSection ?? ((ReusableSection)section).Section;
                foreach (var field in standardSection.Fields)
                {
                    if (string.IsNullOrWhiteSpace(field.Key)) continue;
                    
                    var value = queryString[field.Key];
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    // todo: this currently only works for string types -> also allow this for i.e. checkboxes
                    field.Value = value;
                    this.userDataRepository.SetValue(form.Id, field.Key, value);
                }
            }
        }
    }
}
