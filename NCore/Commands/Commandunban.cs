using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandunban : Command
    {
        public Commandunban() : base("unban", "Разбанить игрока на сервере", "unban <игрок>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 1)
            {
                string a = args[0];
                await Netcraft.Broadcast($"{sender.GetName()} разбанил игрока {a}.");
                await Netcraft.UnbanPlayer(a);
            }

            return false;
        }
    }
}