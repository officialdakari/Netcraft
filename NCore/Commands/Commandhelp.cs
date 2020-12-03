
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandhelp : Command
    {
        public Commandhelp() : base("help", NCore.GetNCore().lang.get("commands.help.description"), "help", new string[] { "?" })
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetcraftPlayer p = (NetcraftPlayer)sender;
                    NCore.Lang lang = p.lang;
                    string h = "";
                    foreach (var a in Netcraft.GetCommands())
                    {
                        string keyUsage = $"commands.{a.Name}.usage";
                        string usage = lang.get(keyUsage);
                        string keyDesc = $"commands.{a.Name}.description";
                        string desc = lang.get(keyDesc);
                        if (usage == keyUsage) usage = "/" + a.Usage;
                        await p.PacketQueue.AddQueue($"chat?{usage} => {desc}");
                    }
                    await p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in Netcraft.GetCommands())
                    await sender.SendMessage(a.Usage + " => " + a.Description);
                return true;
            }

            return false;
        }
    }
}