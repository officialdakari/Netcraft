using NCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetcraftCore.Commands
{
    public class Commandyes : Command
    {
        public Commandyes() : base("yes", "Question answer", "command.yes", "yes")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if(sender is NetcraftPlayer)
            {
                NetcraftPlayer p = (NetcraftPlayer)sender;
                string a = p.Yes();
                if(a != null)
                {
                    await sender.SendMessage(a);
                }
                return true;
            } else
            {
                await sender.SendMessage("You are not a player");
                return true;
            }
            return false;
        }
    }
}
