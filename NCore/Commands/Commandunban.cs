using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandunban : Command
    {
        public Commandunban() : base("unban", "Разбанить игрока на сервере", "unban <игрок>")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 1)
            {
                string a = args[0];
                Netcraft.Broadcast($"{sender.GetName()} разбанил игрока {a}.");
                Netcraft.UnbanPlayer(a);
            }

            return false;
        }
    }
}