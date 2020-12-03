
using System.Threading.Tasks;

namespace NCore
{
    public class Commandplugins : Command
    {
        public Commandplugins() : base("plugins", "Shows list of plugins.", "plugins", new[] { "pl" })
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetcraftPlayer p = (NetcraftPlayer)sender;
                    foreach (var a in PluginManager.Plugins)
                        await p.PacketQueue.AddQueue($"chat?{a.Name} v{a.Version} => {a.Description}");
                    await p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in PluginManager.Plugins)
                    await sender.SendMessage($"{a.Name} v{a.Version} => {a.Description}");
                return true;
            }

            return false;
        }
    }
}