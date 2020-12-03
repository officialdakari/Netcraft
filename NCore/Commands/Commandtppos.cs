using Microsoft.VisualBasic.CompilerServices;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandtppos : Command
    {
        public Commandtppos() : base("tppos", NCore.GetNCore().lang.get("commands.tppos.description"), "tppos <x> <y>")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (!sender.GetAdmin())
            {
                await sender.SendMessage(lang.get("commands.generic.error.no-perms"));
                return true;
            }

            if (args.Length == 2)
            {
                if (!sender.IsPlayer)
                {
                    await sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetcraftPlayer p = (NetcraftPlayer)sender;
                string x = args[0];
                string y = args[1];
                await NCore.GetNCore().SendCommandFeedback(lang.get("commands.tppos.success", x, y, NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y)))).ToString()), sender);
                //await NCore.GetNCore().SendCommandFeedback($"{p.Username} телепортирован на [{x}, {y}] ({NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y))))} блоков отсюда)", sender);
                await p.Teleport(Conversions.ToInteger(x) * 32, Conversions.ToInteger(y) * 32);
            }

            return false;
        }
    }
}