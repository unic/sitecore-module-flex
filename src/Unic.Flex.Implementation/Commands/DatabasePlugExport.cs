namespace Unic.Flex.Implementation.Commands
{
    using Sitecore.Shell.Framework.Commands;

    public class DatabasePlugExport : Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Diagnostics.Log.Error("***** Database Export called", this);
        }
    }
}
