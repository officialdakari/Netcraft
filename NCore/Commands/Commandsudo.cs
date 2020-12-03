using NCore.netcraft.server.api;
using System.Linq;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandsudo : Command
    {
        public Commandsudo() : base("sudo", NCore.GetNCore().lang.get("commands.sudo.description"), NCore.GetNCore().lang.get("commands.sudo.usage"))
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

            if (args.Length > 1)
            {
                var p = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(p))
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }

                NCore.GetNCore().MessageReceived($"chat?{string.Join(" ", args.Skip(1).ToArray())}", p);
                await sender.SendMessage(lang.get("commands.sudo.success", p.Username));
                return true;
            }

            return false;
        }
    }
}