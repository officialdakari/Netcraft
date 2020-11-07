using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandtoggleadmin : Command
    {
        public Commandtoggleadmin() : base("toggleadmin", "Переключает статус администратора игроку", "toggleadmin <arg>")
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
                var local_a = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(local_a))
                {
                    await sender.SendMessage("Игрок не найден");
                    return true;
                }

                local_a.IsAdmin = !local_a.IsAdmin;
                if (local_a.IsAdmin)
                {
                    await local_a.Chat("Вы теперь администратор");
                    await sender.SendMessage($"Выданы права администратора: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} выдал права администратора: {local_a.Username}");
                }
                else
                {
                    await local_a.Chat("Вы больше не администратор");
                    await sender.SendMessage($"Сняты права администратора: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} снял права администратора: {local_a.Username}");
                }

                return true;
            }

            return false;
        }
    }
}