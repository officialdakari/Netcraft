using Microsoft.VisualBasic.CompilerServices;

namespace NCore
{
    public class Commandtppos : Command
    {
        public Commandtppos() : base("tppos", "Телепортирует Вас на заданные координаты [ТОЛЬКО ИГРОК]", "tppos <x> <y>")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 2)
            {
                if (!sender.IsPlayer)
                {
                    sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetworkPlayer p = (NetworkPlayer)sender;
                string x = args[0];
                string y = args[1];
                NCore.GetNCore().SendCommandFeedback($"{p.Username} телепортирован на [{x},{y}] ({NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y))))} блоков отсюда)", sender);
                p.Teleport(Conversions.ToInteger(x), Conversions.ToInteger(y));
            }

            return false;
        }
    }
}