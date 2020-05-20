using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Core.Commands
{
    using Sitecore.Shell.Framework.Commands;

    public class AsyncPlugExecutionCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            var fields = context.Items[0].Template.Fields;
        }
    }
}
