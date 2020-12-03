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
        public Commandeval() : base("eval", "Evaluates code dyamically", "eval <code>", new string[] { "evaluate", "run" }) 
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {

            return true;
        }
    }
}
