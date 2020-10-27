using NCore.netcraft.server.api;
namespace NCore
{
    public class Commandbroadcast : Command
    {
        public Commandbroadcast() : base("broadcast", "Отправляет сообщение в чат", "broadcast <сообщение>")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length > 0)
            {
                Netcraft.Broadcast("[Broadcast] " + string.Join(" ", args));
                return true;
            }

            return false;
        }
    }
}