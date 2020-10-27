using NCore.netcraft.server.api;
namespace NCore
{
    public class Commandban : Command
    {
        public Commandban() : base("ban", "Банит игрока на сервере", "ban <игрок>")
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
                Netcraft.Broadcast($"{sender.GetName()} забанил игрока {a}.");
                Netcraft.BanPlayer(a);
            }

            return false;
        }
    }
}