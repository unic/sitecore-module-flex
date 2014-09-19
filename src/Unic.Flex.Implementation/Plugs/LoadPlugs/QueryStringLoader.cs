namespace Unic.Flex.Implementation.Plugs.LoadPlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using System.Linq;
    using System.Web;
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
        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringLoader"/> class.
        /// </summary>
        public QueryStringLoader()
        {
            this.userDataRepository = Container.Resolve<IUserDataRepository>();
        }

        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(Form form)
        {
            Assert.ArgumentNotNull(form, "form");

            //// todo: is here some kind of security check for querystring validations?

            var queryString = HttpContext.Current.Request.QueryString;
            foreach (var section in form.Steps.SelectMany(step => step.Sections))
            {
                foreach (var field in section.Fields)
                {
                    if (string.IsNullOrWhiteSpace(field.Key)) continue;
                    
                    var value = queryString[field.Key];
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    field.Value = value;
                    this.userDataRepository.SetValue(form.Id, field.Key, value);
                }
            }
        }
    }
}
