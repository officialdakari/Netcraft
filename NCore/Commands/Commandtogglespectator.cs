using NCore;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandtogglespectator : Command
    {
        public Commandtogglespectator() : base("togglespectator", "Переключает режим наблюдателя.", "togglespectator")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
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
                    await p.Survival();
                }
                else
                {
                    await p.Spectator();
                }

                return true;
            }

            return false;
        }
    }
}