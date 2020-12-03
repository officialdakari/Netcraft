using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandunban : Command
    {
        public Commandunban() : base("unban", NCore.GetNCore().lang.get("commands.unban.description"), NCore.GetNCore().lang.get("commands.unban.usage"))
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (!sender.GetAdmin())
            {
                await sender.SendMessage(lang.get("commands.generic.error.no-perms"));
                return true;
            }

            if (args.Length == 1)
            {
                string a = args[0];
                if(!Netcraft.IsBanned(a))
                {
                    await sender.SendMessage(lang.get("commands.unban.failed.not-banned"));
                    return true;
                }
                await Netcraft.Broadcast(lang.get("commands.unban.success", sender.GetName(), a));
                await Netcraft.UnbanPlayer(a);
            }

            return false;
        }
    }
}