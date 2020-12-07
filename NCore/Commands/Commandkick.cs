using System.Linq;
using System.Threading.Tasks;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandkick : Command
    {
        public Commandkick() : base("kick", NCore.GetNCore().lang.get("commands.kick.description"), "kick <игрок> [причина]")
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

            if (args.Length == 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }

                p.Kick(lang.get("commands.kick.kicked"));
                await Netcraft.Broadcast(lang.get("commands.kick.success", sender.GetName(), p.Username, "None"));
                return true;
            }
            else if (args.Length > 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }

                p.Kick(p.lang.get("commands.kick.kicked.reason", string.Join(" ", args.Skip(1))));
                await Netcraft.Broadcast(lang.get("commands.kick.success", sender.GetName(), p.Username, string.Join(" ", args.Skip(1))));

                return true;
            }

            return false;
        }
    }
}