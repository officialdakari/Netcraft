using Microsoft.VisualBasic.CompilerServices;
using System.Threading.Tasks;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandtppos : Command
    {
        public Commandtppos() : base("tppos", NCore.GetNCore().lang.get("commands.tppos.description"), "netcraft.command.tppos", "tppos <x> <y> [player]")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;

            if (args.Length >= 2)
            {
                if (!sender.IsPlayer)
                {
                    await sender.SendMessage("Только для игрока.");
                    return true;
                }

                NetcraftPlayer p = (NetcraftPlayer)sender;
                if(args.Length == 3)
                {
                    p = Netcraft.GetPlayer(args[2]);
                }
                if(p == null)
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }
                string x = args[0];
                string y = args[1];
                await NCore.GetNCore().SendCommandFeedback(lang.get("commands.tppos.success", x, y, NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y)))).ToString(), p.Username), sender);
                //await NCore.GetNCore().SendCommandFeedback($"{p.Username} телепортирован на [{x}, {y}] ({NCore.DistanceBetweenPoint(NCore.Normalize(p.Position), NCore.Normalize(new System.Drawing.Point(Conversions.ToInteger(x), Conversions.ToInteger(y))))} блоков отсюда)", sender);
                await p.Teleport(Conversions.ToInteger(x) * 32, Conversions.ToInteger(y) * 32);
                return true;
            }

            return false;
        }
    }
}