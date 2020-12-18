using NCore.netcraft.server.api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    public class Commandgamerule : Command
    {
        public Commandgamerule() : base("gamerule", NCore.GetNCore().lang.get("commands.gamerule.description"), "netcraft.command.gamerule", NCore.GetNCore().lang.get("commands.gamerule.usage"), new string[] { "gamerule", "rule", "opt"})
        {
        }

        public async override Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if(args.Length == 1)
            {
                if(args[0] == "daylightCycle")
                {
                    await sender.SendMessage(lang.get("commands.gamerule.query", args[0], NCore.GetNCore().World.Gamerules.daylightCycle.ToString()));
                }
                else if (args[0] == "blockPhysics")
                {
                    await sender.SendMessage(lang.get("commands.gamerule.query", args[0], NCore.GetNCore().World.Gamerules.daylightCycle.ToString()));
                }
                else
                {
                    await sender.SendMessage(lang.get("commands.gamerule.error.not-found"));
                }
                return true;
            } else if (args.Length == 2)
            {
                if (args[0] == "daylightCycle")
                {
                    Netcraft.GetWorld().Gamerules.daylightCycle = bool.Parse(args[1]);
                    await sender.SendMessage(lang.get("commands.gamerule.success", args[0], NCore.GetNCore().World.Gamerules.daylightCycle.ToString()));
                }
                else if (args[0] == "blockPhysics")
                {
                    Netcraft.GetWorld().Gamerules.blockPhysics = bool.Parse(args[1]);
                    await sender.SendMessage(lang.get("commands.gamerule.success", args[0], NCore.GetNCore().World.Gamerules.blockPhysics.ToString()));
                }
                else
                {
                    await sender.SendMessage(lang.get("commands.gamerule.error.not-found"));
                }
                return true;
            }
            return false;
        }
    }
}
