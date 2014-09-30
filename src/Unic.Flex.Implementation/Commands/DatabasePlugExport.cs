namespace Unic.Flex.Implementation.Commands
{
    using Sitecore.Shell.Framework;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web.UI.Sheer;

    public class DatabasePlugExport : Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Diagnostics.Log.Error("***** Database Export called", this);

            var url = "http://flex/sitecore/shell/Applications/~/media/98EF24BE026F40CEBE27933AD4A05BF6.ashx?bc=White&db=master&h=128&la=en&mw=640&thn=1&vs=1&ts=ad792b3a-f17e-4332-942a-10ce2edbc45e";

            SheerResponse.Download(url);
        }
    }
}
