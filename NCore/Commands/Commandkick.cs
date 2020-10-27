using System.Linq;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandkick : Command
    {
        public Commandkick() : base("kick", "Выгоняет игрока с сервера", "kick <игрок> [причина]")
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
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    sender.SendMessage("Игрок не найден!");
                    return true;
                }

                p.Kick("Вы были выгнаны администратором.");
                Netcraft.Broadcast($"{sender.GetName()} выгнал игрока {p.GetName()}. Причина не указана.");
            }
            else if (args.Length > 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    sender.SendMessage("Игрок не найден!");
                    return true;
                }

                p.Kick("Вы были выгнаны администратором. Причина: " + string.Join(" ", args.Skip(1)));
                Netcraft.Broadcast($"{sender.GetName()} выгнал игрока {p.GetName()}. Причина: {string.Join(" ", args.Skip(1))}");
            }

            return false;
        }
    }
}