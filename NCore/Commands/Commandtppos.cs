using Microsoft.VisualBasic.CompilerServices;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandtppos : Command
    {
        public Commandtppos() : base("tppos", "Телепортирует Вас на заданные координаты [ТОЛЬКО ИГРОК]", "tppos <x> <y>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 2)
            {
                if (!sender.IsPlayer)
                {
                    await sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetworkPlayer p = (NetworkPlayer)sender;
                string x = args[0];
                string y = args[1];
                await NCore.GetNCore().SendCommandFeedback($"{p.Username} телепортирован на [{x}, {y}] ({NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y))))} блоков отсюда)", sender);
                await p.Teleport(Conversions.ToInteger(x) * 32, Conversions.ToInteger(y) * 32);
            }

            return false;
        }
    }
}