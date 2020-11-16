using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandtell : Command
    {
        public Commandtell() : base("tell", "Отправить приватное сообщение игроку", "tell <player> <message>", new string[]{ "t", "msg", "m"})
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if(args.Length >= 2)
            {
                string message = string.Join(' ', args.Skip(1).ToArray());
                NetworkPlayer player = netcraft.server.api.Netcraft.GetPlayer(args[0]);
                if(player == null)
                {
                    await sender.SendMessage("Игрок не найден!");
                    return true;
                }
                await player.Chat($"{(sender.GetAdmin() == true ? "[ADMIN] " : "")}{sender.GetName()} шепчет Вам: {message}");
                await sender.SendMessage($"Вы прошептали {player.Username}: {message}");
                return true;
            }
            return false;
        }
    }
}
