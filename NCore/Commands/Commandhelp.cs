
using NCore.netcraft.server.api;

namespace NCore
{
    public class Commandhelp : Command
    {
        public Commandhelp() : base("help", "Показывает список команд", "help", new[] { "?" })
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
                        p.PacketQueue.AddQueue($"chat?/{a.Usage} => {a.Description}");
                    p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in Netcraft.GetCommands())
                    sender.SendMessage(a.Usage + " => " + a.Description);
                return true;
            }

            return false;
        }
    }
}