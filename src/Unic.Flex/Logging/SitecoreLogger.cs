using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Logging
{
    using Sitecore.Diagnostics;

    public class SitecoreLogger : ILogger
    {
        public void Info(string message, object owner)
        {
            Log.Info(this.FormatMessage(message), owner);
        }

        public void Error(string message, object owner)
        {
            Log.Error(this.FormatMessage(message), owner);
        }

        private string FormatMessage(string message)
        {
            return string.Format("FLEX :: {0}", message);
        }
    }
}
