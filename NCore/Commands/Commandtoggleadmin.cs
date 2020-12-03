using NCore;
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandtoggleadmin : Command
    {
        public Commandtoggleadmin() : base("toggleadmin", NCore.GetNCore().lang.get("commands.toggleadmin.description"), NCore.GetNCore().lang.get("commands.toggleadmin.usage"))
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
                var local_a = Netcraft.GetPlayer(args[0]);
                if (NCore.IsNothing(local_a))
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }

                local_a.IsAdmin = !local_a.IsAdmin;
                if (local_a.IsAdmin)
                {
                    await local_a.Chat("You are now admin!");
                    await sender.SendMessage($"Admin permissions granted: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} granted admin permissions: {local_a.Username}");
                }
                else
                {
                    await local_a.Chat("You are no longer admin.");
                    await sender.SendMessage($"Admin permissions revoked: {local_a.Username}");
                    NCore.GetNCore().Log($"{sender.GetName()} revoked admin permissions: {local_a.Username}");
                }

                return true;
            }

            return false;
        }
    }
}