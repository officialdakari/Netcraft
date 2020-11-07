
using System.Threading.Tasks;

namespace NCore
{
    public class Commandstop : Command
    {
        public Commandstop() : base("end", "Выключает сервер", "end")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                return false;
            }

            if (args.Length == 0)
            {
                await sender.SendMessage("Вы уверены что хотите выключить сервер? Введите /end confirm чтобы выключить сервер.");
                return true;
            }
            else if (args.Length == 1)
            {
                if (args[0] == "confirm")
                {
                    NCore.GetNCore().StopServer();
                    return true;
                }
            }

            return false;
        }
    }
}