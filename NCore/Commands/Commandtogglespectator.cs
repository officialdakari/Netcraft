using NCore;

namespace NCore
{
    public class Commandtogglespectator : Command
    {
        public Commandtogglespectator() : base("togglespectator", "Переключает режим наблюдателя.", "togglespectator")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (!sender.IsPlayer)
            {
                return false;
            }

            if (args.Length == 0)
            {
                NetworkPlayer p = (NetworkPlayer)sender;
                if (p.IsSpectator)
                {
                    p.Survival();
                }
                else
                {
                    p.Spectator();
                }

                return true;
            }

            return false;
        }
    }
}