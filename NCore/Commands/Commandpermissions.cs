using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    public class Commandpermissions : Command
    {
        public Commandpermissions() : base("permissions", "Manage permissions", "netcraft.command.permissions", "permissions <...>", new string[] { "perms", "perm", "permission" })
        {
        }

        public async override Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            NCore.Lang lang = sender.IsPlayer ? ((NetcraftPlayer)sender).lang : NCore.GetNCore().lang;
            if (args.Length == 0) return false;
            if(args[0] == "help")
            {
                await sender.SendMessage("perms help\r\nperms get <player>\r\nperms add <player> <permission>\r\nperms rem <player> <permission>");

                return true;
            }
            if(args[0] == "get")
            {
                if(args.Length == 2)
                {
                    string str = string.Join("\r\n", NCore.GetNCore().permissions.GetPermissions(args[1]));
                    if (str == null) str = "Not found.";
                    await sender.SendMessage($"{args[0]}'s permissions:\r\n" + str);
                }
                return true;
            }
            if (args[0] == "add")
            {
                if (args.Length == 3)
                {
                    if (!NCore.GetNCore().permissions.perms.ContainsKey(args[0]))
                    {
                        await sender.SendMessage("Not found.");
                        return true;
                    }
                    if (NCore.GetNCore().permissions.AddPermission(args[1], args[2]))
                    {
                        await sender.SendMessage($"Permission '{args[2]}' added: '{args[1]}'");
                    }
                    else
                    {
                        await sender.SendMessage("Failed");
                    }

                }
                return true;
            }
            if (args[0] == "rem")
            {
                if (args.Length == 3)
                {
                    if (!NCore.GetNCore().permissions.perms.ContainsKey(args[0]))
                    {
                        await sender.SendMessage("Not found.");
                        return true;
                    }
                    if (NCore.GetNCore().permissions.RemovePermission(args[1], args[2]))
                    {
                        await sender.SendMessage($"Permission '{args[2]}' removed: '{args[1]}'");
                    }
                    else
                    {
                        await sender.SendMessage("Failed");
                    }

                }
                return true;
            }
            return false;
        }
    }
}
