
using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandhelp : Command
    {
        public Commandhelp() : base("help", "Показывает список команд", "help", new[] { "?" })
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (args.Length == 0)
            {
                if (sender.IsPlayer)
                {
                    NetworkPlayer p = (NetworkPlayer)sender;
                    foreach (var a in Netcraft.GetCommands())
                        await p.PacketQueue.AddQueue($"chat?/{a.Usage} => {a.Description}");
                    await p.PacketQueue.SendQueue();
                    return true;
                }

                foreach (var a in Netcraft.GetCommands())
                    await sender.SendMessage(a.Usage + " => " + a.Description);
                return true;
            }

            return false;
        }
    }
}