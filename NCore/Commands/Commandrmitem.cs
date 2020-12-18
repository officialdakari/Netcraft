using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandrmitem : Command
    {
        public Commandrmitem() : base("rmitem", NCore.GetNCore().lang.get("commands.rmitem.description"), "netcraft.command.rmitem", NCore.GetNCore().lang.get("commands.rmitem.usage"), new string[] {"removeitem", "clearitem", "clear", "clearinv" })
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;

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
                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.rmitem.success.player", t.ToString().ToLower(), count.ToString(), p.Username), sender);
                    return true;
                }
                else if (args[0] == "@a")
                {
                    foreach (var g in Netcraft.GetOnlinePlayers())
                        await g.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.rmitem.success.multiple", t.ToString().ToLower(), count.ToString(), Netcraft.GetOnlinePlayers().Count.ToString()), sender);
                    return true;
                }
                else if (args[0] == "@r")
                {
                    var rnd = new Random();
                    var g = Netcraft.GetOnlinePlayers();
                    p = g[rnd.Next(0, g.Count - 1)];
                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.rmitem.success.player", t.ToString().ToLower(), count.ToString(), p.Username), sender);
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

                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback(lang.get("commands.rmitem.success.player", t.ToString().ToLower(), count.ToString(), p.Username), sender);
                    return true;
                }
            }

            return false;
        }
    }
}
