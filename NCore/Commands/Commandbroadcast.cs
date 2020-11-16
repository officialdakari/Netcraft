using NCore.netcraft.server.api;
using System.Threading.Tasks;

namespace NCore
{
    public class Commandbroadcast : Command
    {
        public Commandbroadcast() : base("broadcast", NCore.GetNCore().lang.get("commands.broadcast.description"), NCore.GetNCore().lang.get("commands.broadcast.usage"))
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length > 0)
            {
                await Netcraft.Broadcast($"[{sender.GetName()}] " + string.Join(" ", args));
                return true;
            }

            return false;
        }
    }
}