using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mailing
{
    using Unic.Flex.Model.DomainModel.Fields;

    public interface IMailService
    {
        string ReplaceEmailAddressesFromConfig(string addresses);

        string ReplaceTokens(string content, IEnumerable<IField> fields);
    }
}
