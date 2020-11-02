using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandkill : Command
    {
        public Commandkill() : base("kill", "Убить указанного игрока", "kill <игрок>")
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
                NetworkPlayer p;
                p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    sender.SendMessage("Игрок не найден!");
                    return true;
                }

                NCore.GetNCore().SendCommandFeedback(args[0] + " убит", sender);
                return true;
            }

            return false;
        }
    }
}