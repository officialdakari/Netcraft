using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Commands
{
    class Commandrmitem : Command
    {
        public Commandrmitem() : base("rmitem", "Удаляет предметы из инвентаря", "rmitem <игрок|@s|@a|@r> <предмет> [кол-во]")
        {
        }

        public override async Task<bool> OnCommand(CommandSender sender, Command cmd, string[] args, string label)
        {
            if (!sender.GetAdmin())
            {
                await sender.SendMessage("У Вас недостаточно прав!");
                return true;
            }

            if (args.Length == 2)
            {
                Material t = (Material)Enum.Parse(typeof(Material), args[1].ToUpper());
                NetworkPlayer p;
                if (args[0] == "@s")
                {
                    if (!sender.IsPlayer)
                    {
                        sender.SendMessage("Только для игрока!");
                        return true;
                    }

                    p = (NetworkPlayer)sender;
                    await p.RemoveItem(t);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} (1 шт.) у " + p.Username, sender);
                    return true;
                }
                else if (args[0] == "@a")
                {
                    foreach (var g in Netcraft.GetOnlinePlayers())
                        await g.RemoveItem(t);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} (1 шт.) у {Netcraft.GetOnlinePlayers().Count} игроков", sender);
                    return true;
                }
                else if (args[0] == "@r")
                {
                    Random rnd = new Random();
                    List<NetworkPlayer> g = Netcraft.GetOnlinePlayers();
                    p = g[rnd.Next(0, g.Count - 1)];
                    await p.RemoveItem(t);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} (1 шт.) у " + p.Username, sender);
                    return true;
                }
                else
                {
                    p = Netcraft.GetPlayer(args[0]);
                    if (NCore.IsNothing(p))
                    {
                        await sender.SendMessage("Игрок не найден!");
                        return true;
                    }

                    await p.RemoveItem(t);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} (1 шт.) у " + p.Username, sender);
                    return true;
                }
            }

            if (args.Length == 3)
            {
                Material t = (Material)Enum.Parse(typeof(Material), args[1].ToUpper());
                int count = Conversions.ToInteger(args[2]);
                NetworkPlayer p;
                if (args[0] == "@s")
                {
                    if (!sender.IsPlayer)
                    {
                        await sender.SendMessage("Только для игрока!");
                        return true;
                    }

                    p = (NetworkPlayer)sender;
                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} ({count} шт.) у " + p.Username, sender);
                    return true;
                }
                else if (args[0] == "@a")
                {
                    foreach (var g in Netcraft.GetOnlinePlayers())
                        await g.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} ({count} шт.) у {Netcraft.GetOnlinePlayers().Count} игроков", sender);
                    return true;
                }
                else if (args[0] == "@r")
                {
                    var rnd = new Random();
                    var g = Netcraft.GetOnlinePlayers();
                    p = g[rnd.Next(0, g.Count - 1)];
                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} ({count} шт.) у " + p.Username, sender);
                    return true;
                }
                else
                {
                    p = Netcraft.GetPlayer(args[0]);
                    if (NCore.IsNothing(p))
                    {
                        await sender.SendMessage("Игрок не найден!");
                        return true;
                    }

                    await p.RemoveItem(t, count);
                    await NCore.GetNCore().SendCommandFeedback($"Удалено {t.ToString().ToLower()} ({count} шт.) у " + p.Username, sender);
                    return true;
                }
            }

            return false;
        }
    }
}
