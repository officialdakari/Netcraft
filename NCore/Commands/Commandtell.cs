using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandtell : Command
    {
        public Commandtell() : base("tell", NCore.GetNCore().lang.get("commands.tell.description"), NCore.GetNCore().lang.get("commands.tell.usage"), new string[]{ "t", "msg", "m"})
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (args.Length >= 2)
            {
                string message = string.Join(' ', args.Skip(1).ToArray());
                NetcraftPlayer player = netcraft.server.api.Netcraft.GetPlayer(args[0]);
                if(player == null)
                {
                    await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                    return true;
                }
                await player.Chat($"{(sender.GetAdmin() == true ? "[ADMIN] " : "")}{sender.GetName()} => {player.Username}: {message}");
                await sender.SendMessage($"{sender.GetName()} => {player.Username}: {message}");
                return true;
            }
            return false;
        }
    }
}
