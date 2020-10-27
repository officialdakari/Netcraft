using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using NCore;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandlist : Command
    {
        public Commandlist() : base("list", "Показывает список игроков", "list")
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetworkPlayer p = (NetworkPlayer)sender;
                    p.PacketQueue.AddQueue($"chat?Сейчас {NCore.clientList.Count} из {NCore.maxPlayers} игроков на сервере:");
                    var sc = new System.Collections.Specialized.StringCollection();
                    foreach (var a in Netcraft.GetOnlinePlayers())
                        sc.Add(a.Username);
                    p.PacketQueue.AddQueue($"chat?{string.Join(", ", Conversions.ToString(sc.Cast<string>().ToArray()))}");
                    p.PacketQueue.SendQueue();
                }
                else
                {
                    sender.SendMessage($"Сейчас {NCore.clientList.Count} из {NCore.maxPlayers} игроков на сервере:");
                    var sc = new System.Collections.Specialized.StringCollection();
                    foreach (var a in Netcraft.GetOnlinePlayers())
                        sc.Add(a.Username);
                    sender.SendMessage(string.Join(", ", Conversions.ToString(sc.Cast<string>().ToArray())));
                }

                return true;
            }

            return false;
        }
    }
}