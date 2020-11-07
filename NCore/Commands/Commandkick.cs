using System.Linq;
using System.Threading.Tasks;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandkick : Command
    {
        public Commandkick() : base("kick", "Выгоняет игрока с сервера", "kick <игрок> [причина]")
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
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage("Игрок не найден!");
                    return true;
                }

                await p.Kick("Вы были выгнаны администратором.");
                await Netcraft.Broadcast($"{sender.GetName()} выгнал игрока {p.GetName()}. Причина не указана.");
                return true;
            }
            else if (args.Length > 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    sender.SendMessage("Игрок не найден!");
                    return true;
                }

                await p.Kick(string.Join(" ", args.Skip(1)));
                await Netcraft.Broadcast($"{sender.GetName()} выгнал игрока {p.GetName()}. Причина: {string.Join(" ", args.Skip(1))}");

                return true;
            }

            return false;
        }
    }
}