
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandaliases : Command
    {
        public Commandaliases() : base("aliases", "Показывает список сокращений для команд.", "aliases", new[] { "a" })
        {
        }

        public override bool OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetworkPlayer p = (NetworkPlayer)sender;
                    foreach (var a in Netcraft.GetCommands())
                    {
                        foreach (var b in a.Aliases)
                        {
                            if (b == a.Name) continue;
                            p.PacketQueue.AddQueue($"chat?/{b} => /{a.Name}");
                        }
                    }

                    p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in Netcraft.GetCommands())
                {
                    foreach (var b in a.Aliases)
                    {
                        if (b == a.Name) continue;
                        sender.SendMessage($"{b} => {a.Name}");
                    }
                }

                return true;
            }

            return false;
        }
    }
}