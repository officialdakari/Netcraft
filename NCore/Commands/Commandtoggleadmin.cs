using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandtoggleadmin : Command
    {
        public Commandtoggleadmin() : base("toggleadmin", "Переключает статус администратора игроку", "toggleadmin <arg>")
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
                var local_a = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(local_a))
                {
                    sender.SendMessage("Игрок не найден");
                    return true;
                }

                local_a.IsAdmin = !local_a.IsAdmin;
                if (local_a.IsAdmin)
                {
                    local_a.Chat("Вы теперь администратор");
                    sender.SendMessage($"Выданы права администратора: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} выдал права администратора: {local_a.Username}");
                }
                else
                {
                    local_a.Chat("Вы больше не администратор");
                    sender.SendMessage($"Сняты права администратора: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} снял права администратора: {local_a.Username}");
                }

                return true;
            }

            return false;
        }
    }
}