using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandlist : Command
    {
        public Commandlist() : base("list", NCore.GetNCore().lang.get("commands.list.description"), "list")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetcraftPlayer p = (NetcraftPlayer)sender;
                    await p.PacketQueue.AddQueue($"chat?" + lang.get("commands.list.info", Netcraft.GetOnlinePlayers().Count.ToString(), NCore.GetNCore().maxPlayers.ToString())); //Сейчас {NCore.GetNCore().players.Count} из {NCore.GetNCore().maxPlayers} игроков на сервере:");
                    var sc = new System.Collections.Specialized.StringCollection();
                    foreach (var a in Netcraft.GetOnlinePlayers())
                        sc.Add(a.Username);
                    await p.PacketQueue.AddQueue($"chat?{string.Join(", ", sc.Cast<string>().ToArray())}");
                    await p.PacketQueue.SendQueue();
                }
                else
                {
                    await sender.SendMessage(NCore.GetNCore().lang.get("commands.list.info", Netcraft.GetOnlinePlayers().Count.ToString(), NCore.GetNCore().maxPlayers.ToString()));
                    var sc = new System.Collections.Specialized.StringCollection();
                    foreach (var a in Netcraft.GetOnlinePlayers())
                        sc.Add(a.Username);
                    await sender.SendMessage(string.Join(", ", sc.Cast<string>().ToArray()));
                }

                return true;
            }

            return false;
        }
    }
}