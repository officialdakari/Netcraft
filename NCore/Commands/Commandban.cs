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
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 1)
            {
                string a = args[0];
                await Netcraft.Broadcast(NCore.GetNCore().lang.get("commands.ban.success", sender.GetName(), a));
                await Netcraft.BanPlayer(a);
            }

            return false;
        }
    }
}