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

                public static event PlayerPacketSendEventEventHandler PlayerPacketSendEvent;

                public delegate void PlayerPacketSendEventEventHandler(events.PlayerPacketSend e);

                internal static void REPlayerPacketSendEvent(events.PlayerPacketSend e)
                {
                    PlayerPacketSendEvent?.Invoke(e);
                }

                public static event PlayerPacketReceiveEventEventHandler PlayerPacketReceiveEvent;

                public delegate void PlayerPacketReceiveEventEventHandler(events.PlayerPacketReceive e);

                internal static void REPlayerPacketReceiveEvent(events.PlayerPacketReceive e)
                {
                    PlayerPacketReceiveEvent?.Invoke(e);
                }

                public static event SandPhysicsEventEventHandler SandPhysicsEvent;
                public delegate void SandPhysicsEventEventHandler(events.SandPhysicsEvent e);

                internal static void RESandPhysicsEvent(events.SandPhysicsEvent e)
                {
                    SandPhysicsEvent?.Invoke(e);
                }
            }

            public class Netcraft
            {
                internal static StringCollection field_a = new StringCollection();
                private static List<Command> field_b = new List<Command>();
                internal static List<NetcraftPlayer> clientList = new List<NetcraftPlayer>();

                public static CommandSender ConsoleCommandSender { get; private set; } = new CommandSender("Server", true);
                public static CommandSender RconCommandSender { get; private set; } = new CommandSender("RCON", true);

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

                public static NCore NCore()
                {
                    return global::NCore.NCore.GetNCore();
                }

                public async static Task<bool> DispatchCommand(CommandSender arg_a, Command arg_b, string arg_c)
                {
                    return await arg_b.OnCommand(arg_a, arg_b, arg_c.Split(' ').Skip(1).ToArray(), arg_c);
                }

                public async static Task<bool> DispatchCommand(CommandSender arg_a, string arg_b)
                {
                    string[] args = arg_b.Split(' ');
                    foreach(Command cmd in field_b)
                    {
                        if(cmd.Aliases.Contains(args[0].ToString()) || cmd.Name.ToLower() == args[0].ToLower())
                        {
                            return await cmd.OnCommand(arg_a, cmd, args.Skip(1).ToArray(), arg_b);
                        }
                    }
                    return false;
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
                            await client.Kick(NCore().lang.get("error.banned"));
                        }
                    }

                    field_a.Add(a.ToLower());
                    NCore().SaveBanlist();
                }

                public static async Task UnbanPlayer(string a)
                {
                    if (!field_a.Contains(a.ToLower()))
                    {
                        throw new Exception("Can't unban not banned player!");
                        return;
                    }

                    field_a.Remove(a.ToLower());
                    NCore().SaveBanlist();
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
                    dobc?.Invoke(m);
                }

                public static async Task<int> GetWorldTime()
                {
                    return NCore().worldtime;
                }

                public static async Task SetWorldTime(int time)
                {
                    if(time > NCore().skyClr.Count - 1)
                    {
                        throw new ArgumentOutOfRangeException();
                        return;
                    }
                    NCore().worldtime = time;
                    await NCore().BroadcastSkyChange(NCore().skyClr[time]);
                }

                internal static event dobcEventHandler dobc;

                internal delegate void dobcEventHandler(string m);

                public static WorldServer GetWorld()
                {
                    return NCore().World;
                }

                public static NetcraftPlayer GetPlayer(string arg0)
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

                public static List<NetcraftPlayer> GetOnlinePlayers()
                {
                    var s = new List<NetcraftPlayer>();
                    foreach (var p in clientList)
                        s.Add(p);
                    return s;
                }

                public static string[] GetPlayersData()
                {
                    var s = new List<string>();
                    foreach (var g in System.IO.Directory.GetFiles("./playerdata/"))
                        s.Add(g);
                    return s.ToArray();
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