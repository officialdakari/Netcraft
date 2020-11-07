using NCore.netcraft.server.api;
using System.Linq;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandsudo : Command
    {
        public Commandsudo() : base("sudo", "Отправляет сообщение или команду как другой игрок", "sudo <игрок> <сообщение | /команда [аргументы...]>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length > 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage("Игрок не найден!");
                    return true;
                }

                NCore.GetNCore().MessageReceived($"chat?{string.Join(" ", args.Skip(1).ToArray())}", p);
                await sender.SendMessage("Успешно выполнено sudo.");
                return true;
            }

            return false;
        }
    }
}