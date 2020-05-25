using CommandDefinitions = Unic.Flex.Core.Definitions.Items.AsyncPlugExecutionCommand;

namespace Unic.Flex.Core.Commands
{
    using DependencyInjection;
    using Plugs;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;

    public class AsyncPlugExecutionCommand
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Assert.ArgumentNotNull(command, "AsyncPlugExecutionCommand");

            var siteName = command.InnerItem.Fields[CommandDefinitions.Fields.SiteName.Id]?.Value;
            var logActivity = command.InnerItem.Fields[CommandDefinitions.Fields.LogActivity.Id]?.Value;
            var logTag = command.InnerItem.Fields[CommandDefinitions.Fields.LogTag.Id]?.Value;

            var plugExecutionService = DependencyResolver.Resolve<IAsyncPlugExecutionService>();

            plugExecutionService.LogActivity = logActivity != null && logActivity.Equals("1");
            plugExecutionService.LogTag = logTag;
            plugExecutionService.SiteName = siteName;
            plugExecutionService.ExecutePlugs();
        }
    }
}
