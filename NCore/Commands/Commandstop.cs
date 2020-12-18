
using System.Threading.Tasks;

namespace NCore
{
    public class Commandstop : Command
    {
        public Commandstop() : base("end", NCore.GetNCore().lang.get("commands.end.description"), "netcraft.command.end", "end")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {

            if (args.Length == 0)
            {
                await sender.SendMessage("Are you sure wanted to stop server? Use '/end confirm' to confirm.");
                return true;
            }
            else if (args.Length == 1)
            {
                if (args[0] == "confirm")
                {
                    NCore.GetNCore().StopServer();
                    return true;
                }
            }

            return false;
        }
    }
}