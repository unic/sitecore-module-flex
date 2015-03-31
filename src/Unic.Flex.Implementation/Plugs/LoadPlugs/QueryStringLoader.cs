namespace Unic.Flex.Implementation.Plugs.LoadPlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using System.Linq;
    using System.Web;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Implementation.Fields.TextOnly;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Plugs;

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
        /// Initializes a new instance of the <see cref="QueryStringLoader" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        public QueryStringLoader(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }

        /// <summary>
        /// Executes the load plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public override void Execute(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            var queryString = HttpContext.Current.Request.QueryString;
            foreach (var section in form.Steps.SelectMany(step => step.Sections))
            {
                // ignore text only fields due to posible cross-site scripting
                foreach (var field in section.Fields.Where(f => f.GetType() != typeof(TextOnlyField)))
                {
                    if (string.IsNullOrWhiteSpace(field.Key)) continue;
                    
                    var value = queryString[field.Key];
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    field.Value = value;
                    this.userDataRepository.SetValue(form.Id, field.Id, value);
                }
            }
        }
    }
}
