
using System.Threading.Tasks;

namespace NCore
{
    public class Commandnmc : Command
    {
        public Commandnmc() : base("nmc", "Выключает проверку движения [ТОЛЬКО ИГРОК]", "nmc")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.IsPlayer)
            {
                await sender.SendMessage("Только для игрока.");
                return true;
            }

            if (sender.GetAdmin())
            {
                NetworkPlayer p = (NetworkPlayer)sender;
                p.DisableMovementCheck = !p.DisableMovementCheck;
                if (p.DisableMovementCheck)
                {
                    await sender.SendMessage("Отключена проверка передвижения.");
                }

                return true;
            }

            return false;
        }
    }
}