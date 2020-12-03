using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandgive : Command
    {
        public Commandgive() : base("give", NCore.GetNCore().lang.get("commands.give.description"), NCore.GetNCore().lang.get("commands.give.usage"))
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
                args[2] = "1";
            }

            if (args.Length == 3)
            {
                Material t = (Material)Enum.Parse(typeof(Material), args[1].ToUpper());
                int count = Conversions.ToInteger(args[2]);
                NetcraftPlayer p;
                if (args[0] == "@s")
                {
                    if (!sender.IsPlayer)
                    {
                        await sender.SendMessage("Только для игрока!");
                        return true;
                    }

                    p = (NetcraftPlayer)sender;
                    await p.Give(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.give.success.player", t.ToString().ToLower(), args[2], p.Username), sender);
                    return true;
                }
                else if (args[0] == "@a")
                {
                    foreach (var g in Netcraft.GetOnlinePlayers())
                        await g.Give(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.give.success.all", t.ToString().ToLower(), args[2], Netcraft.GetOnlinePlayers().Count.ToString()), sender);
                    return true;
                }
                else if (args[0] == "@r")
                {
                    var rnd = new Random();
                    var g = Netcraft.GetOnlinePlayers();
                    p = g[rnd.Next(0, g.Count - 1)];
                    await p.Give(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.give.success.player", t.ToString().ToLower(), args[2], p.Username), sender);
                    return true;
                }
                else
                {
                    p = Netcraft.GetPlayer(args[0]);
                    if (NCore.IsNothing(p))
                    {
                        await sender.SendMessage(lang.get("commands.generic.player.not-found"));
                        return true;
                    }

                    await p.Give(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.give.success.player", t.ToString().ToLower(), args[2], p.Username), sender);
                    return true;
                }
            }

            return false;
        }
    }
}