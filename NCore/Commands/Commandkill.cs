using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandkill : Command
    {
        public Commandkill() : base("kill", "Убить указанного игрока", "kill <игрок>")
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
                NetworkPlayer p;
                p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage("Игрок не найден!");
                    return true;
                }

                await NCore.GetNCore().SendCommandFeedback(args[0] + " убит", sender);
                return true;
            }

            return false;
        }
    }
}