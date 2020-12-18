using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace NCore.Commands
{
    class Commandeval : Command
    {
        public Commandeval() : base("eval", "Evaluates code dynamically", "netcraft.command.eval", "eval <code>", new string[] { "evaluate", "run" }) 
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (args.Length < 1) return false;
            ScriptOptions a = ScriptOptions.Default;
            a = a.AddReferences(new string[] { "System", "System.Core", "System.Linq", "System.IO", "System.Text", "System.Threading", "System.Threading.Tasks",
                NCore.GetNCore().GetApplicationRoot()});
            a = a.AddImports(new string[] { "System", "System.Linq", "System.IO", "System.Text", "System.Threading", "System.Threading.Tasks",
                "NCore", "NCore.netcraft.server.api"});
            await sender.SendMessage((await CSharpScript.EvaluateAsync(string.Join(' ', args), a)).ToString());
            return true;
        }
    }
}
