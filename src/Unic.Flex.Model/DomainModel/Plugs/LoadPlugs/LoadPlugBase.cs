using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.DomainModel.Plugs.LoadPlugs
{
    using Unic.Flex.Model.DomainModel.Forms;

    public abstract class LoadPlugBase : PlugBase, ILoadPlug
    {
        public abstract void Execute(Form form);
    }
}
