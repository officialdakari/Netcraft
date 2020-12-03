using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandban : Command
    {
        public Commandban() : base("ban", NCore.GetNCore().lang.get("commands.ban.description"), NCore.GetNCore().lang.get("commands.ban.usage"))
        {
        }

        public async override Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
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
                if (Netcraft.IsBanned(a))
                {
                    await sender.SendMessage(lang.get("commands.ban.failed.banned"));
                    return true;
                }
                await Netcraft.Broadcast(lang.get("commands.ban.success", sender.GetName(), a));
                await Netcraft.BanPlayer(a);
            }

            return false;
        }
    }
}