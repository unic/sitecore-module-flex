using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Logging
{
    public interface ILogger
    {
        void Info(string message, object owner);

        void Warn(string message, object owner);

        void Error(string message, object owner);
    }
}
