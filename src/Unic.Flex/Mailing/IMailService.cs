using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mailing
{
    public interface IMailService
    {
        string ReplaceEmailAddressesFromConfig(string addresses);
    }
}
