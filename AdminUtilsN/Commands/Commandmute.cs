using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCore;
using NCore.netcraft.server.api;

namespace AdminUtilsN.Commands
{
    class Commandmute : NCore.Command
    {
        public Commandmute() : base("mute", "Мьют или размьют игрока", "mute <player> <seconds> <reason>", new[] { "m" })
        {
        }
        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length >= 3)
            {
                if (!sender.GetAdmin())
                {
                    sender.SendMessage("You don't have permissions to do that.");
                    return true;
                }
                NetcraftPlayer player = Netcraft.GetPlayer(args[0]);
                if (player == null)
                {
                    sender.SendMessage("Player not found!");
                    return true;
                }
                if(!Plugin.GetInstance().GetAutomod().isMuted(player.Username))
                {

                    Mute mute = new Mute(player, String.Join(" ", args.Skip(2).ToArray()), new DateTime(0, 0, 0, 0, 0, int.Parse(args[1])));

                    Plugin.GetInstance().GetAutomod().mutePlayer(player, mute);
                }
                else
                {
                    Plugin.GetInstance().GetAutomod().unmutePlayer(player);
                }

                return true;
            }
            return false;
        }
    }
}
