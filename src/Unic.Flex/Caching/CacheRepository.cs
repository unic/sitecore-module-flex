using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Caching
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Web.UI.WebControls;
    using System.Xml;
    using System.Xml.Serialization;
    using Sitecore;
    using Sitecore.Caching;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;

    public class CacheRepository : Sitecore.Caching.CustomCache, ICacheRepository
    {
        public CacheRepository() : base("sitecore-flex-forms", StringUtil.ParseSizeString("1MB"))
        {
        }

        public T Get<T>(string cacheKey) where T : class
        {
            Assert.ArgumentNotNullOrEmpty(cacheKey, "cacheKey");
            return this.GetObject(cacheKey) as T;
        }

        public void AddViewModel(string cacheKey, object data)
        {
            Assert.ArgumentNotNullOrEmpty(cacheKey, "cacheKey");
            Assert.ArgumentNotNull(data, "data");

            this.SetObject(cacheKey, data, 5000);
        }
    }
}
