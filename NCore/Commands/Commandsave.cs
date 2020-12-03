using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandsave : Command
    {
        public Commandsave() : base("save", NCore.GetNCore().lang.get("commands.save.description"), "save", new string[] { "s", "save-all", "sg", "sgame" })
        {

        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав");
                return true;
            }
            if(args.Length == 0)
            {
                System.IO.File.WriteAllText("./world.txt", new SaveLoad().Save(NCore.GetNCore().World), Encoding.UTF8);
                await sender.SendMessage(lang.get("commands.save.success"));
                NCore.GetNCore().Log(sender.GetName() + " saved the game");
                return true;
            }
            return false;
        }
    }
}
