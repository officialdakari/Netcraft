
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandaliases : Command
    {
        public Commandaliases() : base("aliases", NCore.GetNCore().lang.get("commands.aliases.description"), "aliases", new[] { "a" })
        {
        }

        public async override Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetworkPlayer p = (NetworkPlayer)sender;
                    foreach (var a in Netcraft.GetCommands())
                    {
                        foreach (var b in a.Aliases)
                        {
                            if (b == a.Name) continue;
                            await p.PacketQueue.AddQueue($"chat?/{b} => /{a.Name}");
                        }
                    }

                    await p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in Netcraft.GetCommands())
                {
                    foreach (var b in a.Aliases)
                    {
                        if (b == a.Name) continue;
                        await sender.SendMessage($"{b} => {a.Name}");
                    }
                }

                return true;
            }

            return false;
        }
    }
}