using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandkill : Command
    {
        public Commandkill() : base("kill", NCore.GetNCore().lang.get("commands.kill.description"), "netcraft.command.kill", NCore.GetNCore().lang.get("commands.kill.usage"))
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;

            if (args.Length == 1)
            {
                NetcraftPlayer p;
                p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }
                await p.UpdateHealth(0, "was slain by admin");
                await NCore.GetNCore().SendCommandFeedback(args[0] + " killed", sender);
                return true;
            }

            return false;
        }
    }
}