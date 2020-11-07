
using System.Threading.Tasks;

namespace NCore
{
    public class Commandmessage : Command
    {
        public Commandmessage() : base("msg", "msg", "msg <i/w/e>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 1)
            {
                if (!sender.IsPlayer)
                {
                    await sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetworkPlayer p = (NetworkPlayer)sender;
                if (args[0] == "i") await p.Message("Это тестовое сообщение", 0);
                if (args[0] == "w") await p.Message("Это тестовое сообщение", 1);
                if (args[0] == "e") await p.Message("Это тестовое сообщение", 2);
                return true;
            }

            return false;
        }
    }
}