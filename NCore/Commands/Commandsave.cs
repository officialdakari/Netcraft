using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandsave : Command
    {
        public Commandsave() : base("save", "Сохраняет игру", "save", new string[] { "s", "save-all", "sg", "sgame" })
        {

        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if(!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав");
                return true;
            }
            if(args.Length == 0)
            {
                System.IO.File.WriteAllText("./world.txt", new SaveLoad().Save(NCore.GetNCore().World), Encoding.UTF8);
                await sender.SendMessage("Игра успешно сохранена");
                NCore.GetNCore().Log(sender.GetName() + " сохранил игру");
                return true;
            }
            return false;
        }
    }
}
