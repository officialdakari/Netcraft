using NCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetcraftCore.Commands
{
    public class Commandno : Command
    {
        public Commandno() : base("no", "Question answer", "command.no", "no")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (sender is NetcraftPlayer)
            {
                NetcraftPlayer p = (NetcraftPlayer)sender;
                string a = p.No();
                if (a != null)
                {
                    await sender.SendMessage(a);
                }
                return true;
            }
            else
            {
                await sender.SendMessage("You are not a player");
                return true;
            }
            return false;
        }
    }
}
