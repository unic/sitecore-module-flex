namespace Unic.Flex.Implementation.Plugs.LoadPlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Diagnostics;
    using System;
    using System.Linq;
    using System.Web;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Implementation.Fields.TextOnly;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Plugs;

    [SitecoreType(TemplateId = "{4906DF7C-B200-4825-B1AC-488D7D928452}")]
    public class QueryStringLoader : LoadPlugBase
    {
        private readonly IUserDataRepository userDataRepository;

        [SitecoreField("Is Domain Protected")]
        public bool IsDomainProtected { get; set; }

        [SitecoreField("Allow from Domains")]
        public string AllowFromDomains { get; set; }

        public QueryStringLoader(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }

        public override void Execute(IForm form)
        {
            Assert.ArgumentNotNull(form, "form");

            if (this.CanQueryStringBeProcessed())
            {
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

        private bool CanQueryStringBeProcessed() =>
            !IsDomainProtected || (IsDomainProtected && this.IsDomainAllowed());

        private bool IsDomainAllowed() =>
            AllowFromDomains.Contains(HttpContext.Current.Request.Url.Host);
    }
}
