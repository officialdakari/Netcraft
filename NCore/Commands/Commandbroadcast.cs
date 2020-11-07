using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandbroadcast : Command
    {
        public Commandbroadcast() : base("broadcast", "Отправляет сообщение в чат", "broadcast <сообщение>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length > 0)
            {
                await Netcraft.Broadcast("[Broadcast] " + string.Join(" ", args));
                return true;
            }

            return false;
        }
    }
}