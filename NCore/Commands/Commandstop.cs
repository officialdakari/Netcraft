
namespace NCore
{
    public class Commandstop : Command
    {
        public Commandstop() : base("end", "Выключает сервер", "end")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                return false;
            }

            if (args.Length == 0)
            {
                sender.SendMessage("Вы уверены что хотите выключить сервер? Введите /end confirm чтобы выключить сервер.");
                return true;
            }
            else if (args.Length == 1)
            {
                if (args[0] == "confirm")
                {
                    NCore.StopServer();
                    return true;
                }
            }

            return false;
        }
    }
}