using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NCore.netcraft.server.api;

namespace NCore.Commands
{
    public class Commandtime : Command
    {
        public Commandtime() : base("time", NCore.GetNCore().lang.get("commands.time.description"), "netcraft.command.time", NCore.GetNCore().lang.get("commands.time.usage"))
        {
        }

        public async override Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if(args.Length == 1)
            {
                int d = int.Parse(args[0]);
                if(d > NCore.GetNCore().skyClr.Count - 1)
                {
                    await sender.SendMessage(lang.get("commands.time.invalid"));
                    return true;
                }
                Netcraft.SetWorldTime(d);
                await sender.SendMessage(lang.get("commands.time.success", d.ToString()));
                return true;
            }
            return false;
        }
    }
}
