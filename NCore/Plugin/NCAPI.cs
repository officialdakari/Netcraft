using System;
using System.Collections.Generic;
using global::System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Threading.Tasks;

namespace NCore.netcraft
{
    namespace server
    {
        namespace api
        {
            // Public Class Logger
            // Shared Sub Info(arg0 As String)
            // Log(arg0, "INFO")
            // End Sub
            // Shared Sub Warning(arg0 As String)
            // Log(arg0, "WARNING")
            // End Sub
            // Shared Sub Severe(arg0 As String)
            // Log(arg0, "ERROR")
            // End Sub
            // End Class
            public class PluginLogger
            {
                private Plugin plugin;

                public PluginLogger(Plugin p)
                {
                    plugin = p;
                }

                public void Info(string arg0)
                {
                    NCore.GetNCore().LogPlugin(arg0, plugin);
                }

                public void Warning(string arg0)
                {
                    NCore.GetNCore().LogPlugin(arg0, plugin, "WARNING");
                }

                public void Severe(string arg0)
                {
                    NCore.GetNCore().LogPlugin(arg0, plugin, "ERROR");
                }
            }

            public class NCSApi
            {
                public static event BlockBreakEventEventHandler BlockBreakEvent;

                public delegate void BlockBreakEventEventHandler(events.BlockBreakEventArgs e);

                internal static void REBlockBreakEvent(events.BlockBreakEventArgs e)
                {
                    BlockBreakEvent?.Invoke(e);
                }

                public static event BlockPlaceEventEventHandler BlockPlaceEvent;

                public delegate void BlockPlaceEventEventHandler(events.BlockPlaceEventArgs e);

                internal static void REBlockPlaceEvent(events.BlockPlaceEventArgs e)
                {
                    BlockPlaceEvent?.Invoke(e);
                }

                public static event PlayerChatEventEventHandler PlayerChatEvent;

                public delegate void PlayerChatEventEventHandler(events.PlayerChatEventArgs e);

                internal static void REPlayerChatEvent(events.PlayerChatEventArgs e)
                {
                    PlayerChatEvent?.Invoke(e);
                }

                public static event PlayerMoveEventEventHandler PlayerMoveEvent;

                public delegate void PlayerMoveEventEventHandler(events.PlayerMoveEventArgs e);

                internal static void REPlayerMoveEvent(events.PlayerMoveEventArgs e)
                {
                    PlayerMoveEvent?.Invoke(e);
                }

                public static event PlayerJoinEventEventHandler PlayerJoinEvent;

                public delegate void PlayerJoinEventEventHandler(events.PlayerJoinEventArgs e);

                internal static void REPlayerJoinEvent(events.PlayerJoinEventArgs e)
                {
                    PlayerJoinEvent?.Invoke(e);
                }

                public static event PlayerLeaveEventEventHandler PlayerLeaveEvent;

                public delegate void PlayerLeaveEventEventHandler(events.PlayerLeaveEventArgs e);

                internal static void REPlayerLeaveEvent(events.PlayerLeaveEventArgs e)
                {
                    PlayerLeaveEvent?.Invoke(e);
                }

                public static event PlayerHealthEventEventHandler PlayerHealthEvent;

                public delegate void PlayerHealthEventEventHandler(events.PlayerHealthEventArgs e);

                internal static void REPlayerHealthEvent(events.PlayerHealthEventArgs e)
                {
                    PlayerHealthEvent?.Invoke(e);
                }

                public static event PlayerDeathEventEventHandler PlayerDeathEvent;

                public delegate void PlayerDeathEventEventHandler(events.PlayerDeathEventArgs e);

                internal static void REPlayerDeathEvent(events.PlayerDeathEventArgs e)
                {
                    PlayerDeathEvent?.Invoke(e);
                }

                public static event BlockRightClickEventEventHandler BlockRightClickEvent;

                public delegate void BlockRightClickEventEventHandler(events.BlockRightClickEvent e);

                internal static void REBlockRightClickEvent(events.BlockRightClickEvent e)
                {
                    BlockRightClickEvent?.Invoke(e);
                }

                public static event PlayerLoginEventEventHandler PlayerLoginEvent;

                public delegate void PlayerLoginEventEventHandler(events.PlayerLoginEventArgs e);

                internal static void REPlayerLoginEvent(events.PlayerLoginEventArgs e)
                {
                    PlayerLoginEvent?.Invoke(e);
                }
            }

            public class Netcraft
            {
                internal static StringCollection field_a = new StringCollection();
                private static List<Command> field_b = new List<Command>();
                internal static List<NetworkPlayer> clientList = new List<NetworkPlayer>();

                public static CommandSender ConsoleCommandSender { get; private set; } = new CommandSender("Server", true);

                public static void AddCommand(Command a)
                {
                    field_b.Add(a);
                }

                public static void RemoveCommand(string a)
                {
                    foreach (var local_a in field_b)
                    {
                        if ((local_a.Name.ToLower() ?? "") == (a.ToLower() ?? ""))
                        {
                            field_b.Remove(local_a);
                            break;
                        }
                    }
                }

                public static void PerformCommand(CommandSender arg_a, Command arg_b, string arg_c)
                {
                    arg_b.OnCommand(arg_a, arg_b, arg_c.Split(" ").Skip(1).ToArray(), arg_c);
                }

                public static List<Command> GetCommands()
                {
                    return field_b;
                }

                public static async Task BanPlayer(string a)
                {
                    if (field_a.Contains(a.ToLower()))
                    {
                        throw new Exception("Can't ban already banned player!");
                    }

                    foreach (var client in GetOnlinePlayers())
                    {
                        if ((client.Username.ToLower() ?? "") == (a.ToLower() ?? ""))
                        {
                            await client.Kick(NCore.GetNCore().lang.get("error.banned"));
                        }
                    }

                    field_a.Add(a.ToLower());
                    NCore.GetNCore().SaveBanlist();
                }

                public static async Task UnbanPlayer(string a)
                {
                    if (!field_a.Contains(a.ToLower()))
                    {
                        throw new Exception("Can't unban not banned player!");
                        return;
                    }

                    field_a.Remove(a.ToLower());
                    NCore.GetNCore().SaveBanlist();
                }

                public static List<string> GetBannedPlayers()
                {
                    var local_a = new List<string>();
                    foreach (string local_b in field_a)
                        local_a.Add(local_b.ToLower());
                    return local_a;
                }

                public static bool IsBanned(string arg_a)
                {
                    return field_a.Contains(arg_a.ToLower());
                }

                public static async Task Broadcast(string m)
                {
                    // accessMainForm.Log("Broadcast: " + m)
                    // accessMainForm.Send("chat?" + m)
                    // accessMainForm(m)
                    // My.Forms.Form1.Send("chat?" + m)
                    // My.Forms.Form1.Log(m)
                    dobc?.Invoke(m);
                }

                internal static event dobcEventHandler dobc;

                internal delegate void dobcEventHandler(string m);

                public static WorldServer GetWorld()
                {
                    return NCore.GetNCore().World;
                }

                public static NetworkPlayer GetPlayer(string arg0)
                {
                    foreach (var p in clientList)
                    {
                        if ((p.Username.ToLower() ?? "") == (arg0.ToLower() ?? ""))
                        {
                            return p;
                        }
                    }

                    return null;
                }

                public static List<NetworkPlayer> GetOnlinePlayers()
                {
                    var s = new List<NetworkPlayer>();
                    foreach (var p in clientList)
                        s.Add(p);
                    return s;
                }

                public static StringCollection GetPlayersData()
                {
                    var s = new StringCollection();
                    foreach (var g in System.IO.Directory.GetFiles("./playerdata/"))
                        s.Add(g);
                    return s;
                }
            }

            public class Config
            {
                internal string text;

                public static Config Read(string a)
                {
                    var cfg = new Config();
                    cfg.text = System.IO.File.ReadAllText(a, System.Text.Encoding.UTF8);
                    cfg.path = a;
                    return cfg;
                }

                internal string path;

                public void Save()
                {
                    System.IO.File.WriteAllText(path, text, System.Text.Encoding.UTF8);
                }

                public string Parse(string variable)
                {
                    return global::NCore.Config.GetValue(variable, text.Replace(Constants.vbCrLf, Constants.vbLf), "");
                }
            }
        }
    }
}