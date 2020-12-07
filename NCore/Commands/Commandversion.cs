
using System.Threading.Tasks;

namespace NCore
{
    public class Commandversion : Command
    {
        public Commandversion() : base("version", NCore.GetNCore().lang.get("commands.version.description"), "version")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                await sender.SendMessage($"This server is running NCore {NCore.NCORE_VERSION} for Netcraft {NCore.NETCRAFT_VERSION}. There is {PluginManager.Plugins.Count} plugins.");
                return true;
            }

            return false;
        }
    }
}