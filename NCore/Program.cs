using System;
using System.Collections;
using System.Collections.Generic;
using global::System.Collections.Specialized;
using System.Diagnostics;
using global::System.Drawing;
using global::System.IO;
using System.Linq;
using global::System.Net;
using global::System.Net.Sockets;
using global::System.Text;
using global::System.Text.RegularExpressions;
using global::System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace NCore
{
    public class Config
    {
        public static string GetValue(string var, string cfg, string ifNull = null)
        {
            cfg = cfg.Replace(Constants.vbCrLf, Constants.vbLf);
            foreach (var a in cfg.Split(Constants.vbLf))
            {
                var b = a.Split("=");
                if ((b[0].ToLower() ?? "") == (var.ToLower() ?? ""))
                {
                    return string.Join("=", b.Skip(1).ToArray());
                }
            }

            return ifNull;
        }
    }

    static class NCore
    {
        internal static WorldServer World { get; set; }

        private static TcpListener Listning;
        internal readonly static List<NetworkPlayer> clientList = new List<NetworkPlayer>();
        private static NetworkPlayer pClient;
        // Dim npc As EntityPlayerNPC = New EntityPlayerNPC
        private readonly static object worldfile = Operators.AddObject(Application.StartupPath, "/world.txt");
        private readonly static object configfile = Operators.AddObject(Application.StartupPath, "/config.txt");
        private readonly static SaveLoad sv = new SaveLoad();
        internal static int maxPlayers = 20;
        private static string chatFormat = "%1 %2";
        private static int everyBodyAdmin = 0;
        private static int allowFlight = 0;
        private static string motd = "A NCore server";
        private static string name;
        private static int allowQuery = 0;
        private static int commandsConsoleOnly = 0;
        private static int enableAuth = -1;

        public static bool IsSingleplayerServer { get; set; } = false;

        private static int worldloadDelay = 40;

        public static void LoadConfig()
        {
            string cfg = File.ReadAllText(Conversions.ToString(configfile), Encoding.UTF8);
            string nccfg = File.ReadAllText("./ncore.cfg", Encoding.UTF8);
            maxPlayers = Conversions.ToInteger(Config.GetValue("max-players", cfg, "-1"));
            chatFormat = Config.GetValue("chat-format", cfg, "%1 %2");
            everyBodyAdmin = Conversions.ToInteger(Config.GetValue("everybody-admin", cfg, "0"));
            motd = Config.GetValue("server-motd", cfg, "null");
            name = Config.GetValue("server-name", cfg, "null");
            allowFlight = Conversions.ToInteger(Config.GetValue("allow-flight", cfg, "0"));
            allowQuery = Conversions.ToInteger(Config.GetValue("allow-query", nccfg, "0"));
            commandsConsoleOnly = Conversions.ToInteger(Config.GetValue("commands-console-only", nccfg, "0"));
            enableAuth = Conversions.ToInteger(Config.GetValue("enable-auth", cfg, "0"));

            if (isIllegalValue(everyBodyAdmin, 0, 1)) CrashReport(new Exception("Illegal property value"));
            if (isIllegalValue(allowFlight, 0, 1)) CrashReport(new Exception("Illegal property value"));
            if (isIllegalValue(allowQuery, 0, 1)) CrashReport(new Exception("Illegal property value"));
            if (isIllegalValue(commandsConsoleOnly, 0, 1)) CrashReport(new Exception("Illegal property value"));
            if (isIllegalValue(enableAuth, 0, 1)) CrashReport(new Exception("Illegal property value"));

        }

        public static void OnCrash(object sender, ThreadExceptionEventArgs e)
        {
            CrashReport(e.Exception);
        }

        public static void LoadCommands()
        {
            Netcraft.AddCommand(new Commandhelp());
            Netcraft.AddCommand(new Commandban());
            Netcraft.AddCommand(new Commandunban());
            Netcraft.AddCommand(new Commanditems());
            Netcraft.AddCommand(new Commandcraft());
            Netcraft.AddCommand(new Commandtoggleadmin());
            Netcraft.AddCommand(new Commandkick());
            Netcraft.AddCommand(new Commandkill());
            Netcraft.AddCommand(new Commandtogglespectator());
            Netcraft.AddCommand(new Commandgive());
            Netcraft.AddCommand(new Commandnmc());
            Netcraft.AddCommand(new Commandbroadcast());
            Netcraft.AddCommand(new Commandlist());
            Netcraft.AddCommand(new Commandstop());
            Netcraft.AddCommand(new Commandmessage());
            Netcraft.AddCommand(new Commandsudo());
            Netcraft.AddCommand(new Commandtppos());
            Netcraft.AddCommand(new Commandversion());
            Netcraft.AddCommand(new Commandaliases());
            Netcraft.AddCommand(new Commandplugins());
        }

        private static void eventhandler_a(string m)
        {
            Log(m);
            Send("chat?" + m);
        }

        private static Thread loopThread;
        internal const string NETCRAFT_VERSION = "1.3-ALPHA-U26102020";
        internal const string NCORE_VERSION = "0.4";

        private static void ThreadLoop()
        {
            while (true)
            {
                Thread.Sleep(100);
                logAppendDelay -= 1;
                if (logAppendDelay == 0)
                {
                    logAppendDelay = 40;
                    if (File.Exists("./log.txt"))
                    {
                        File.AppendAllText("./log.txt", toAppend, Encoding.UTF8);
                        toAppend = "";
                    }
                    else
                    {
                        File.WriteAllText("./log.txt", toAppend, Encoding.UTF8);
                        toAppend = "";
                    }

                    SaveAuth();
                    foreach (var b in World.Blocks)
                    {
                        if (b.Type == EnumBlockType.SAPLING)
                        {
                            World.Blocks.Remove(b);
                            Send("removeblock?" + b.Position.X.ToString() + "?" + b.Position.Y.ToString());
                            TreeGenerator.GrowthTree(b.Position, World);
                            Log($"Tree growth at [{b.Position.X.ToString() + ", " + b.Position.Y.ToString()}]");
                            break;
                        }
                    }
                }

                Console.Title = $"NCore {NCORE_VERSION} (Netcraft {NETCRAFT_VERSION}) | {clientList.Count}/{maxPlayers.ToString()} players | {(Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split(".")[0]}MB used of {Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L}MB, {(Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L - Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split(".")[0]}MB free | Total Processor Time: {Process.GetCurrentProcess().TotalProcessorTime.ToString()} | Uptime: {(DateTime.Now - Process.GetCurrentProcess().StartTime).ToString().Split(".")[0]}";
            }
        }

        private static List<Color> skyClr = new List<Color>();
        private static int worldtime, stp;
        private static Thread daytimeThread;

        public static void daytimeThreadLoop()
        {
            while (true)
            {
                Thread.Sleep(5000);
                if (worldtime >= skyClr.Count - 1)
                {
                    stp = -1;
                }

                if (worldtime <= 0)
                {
                    stp = 1;
                }

                worldtime += stp;
                BroadcastSkyChange(skyClr[worldtime]);
            }
        }

        public static void BroadcastSkyChange(Color clr)
        {
            try
            {
                foreach (var gg in clientList)
                    gg.SendSkyColorChange(clr);
            }
            catch (Exception ex)
            {
            }
        }

        public static void GametimeInitialize()
        {
            stp = 1;
            worldtime = 0;
            daytimeThread = new Thread(daytimeThreadLoop);
            daytimeThread.Name = "World Time";
            daytimeThread.Start();
            skyClr.Add(Color.Black); // ночь
            skyClr.Add(Color.Black); // ночь
            skyClr.Add(Color.Black); // ночь
            skyClr.Add(Color.Black); // ночь
            skyClr.Add(Color.DarkSlateBlue); // рассвет/закат
            skyClr.Add(Color.DarkBlue); // рассвет/закат
            skyClr.Add(Color.MediumBlue); // рассвет/закат
            skyClr.Add(Color.Blue); // день
            skyClr.Add(Color.Blue); // день
            skyClr.Add(Color.Blue); // день
            skyClr.Add(Color.Blue); // день
        }

        public static void ThreadAdd()
        {
            if (!threads.Contains(Thread.CurrentThread))
                threads.Add(Thread.CurrentThread);
        }

        public static void watchdogThreadLoop()
        {
            ThreadAdd();
            Thread.CurrentThread.Name = "Watchdog";
            while (true)
            {
                try
                {
                    foreach (var th in threads)
                    {
                        if (!th.IsAlive)
                        {
                            Log($"Watchdog detected thread death. Thread unexpectedly stopped working. Thread ID: {th.ManagedThreadId}", "ERROR");
                            threads.Remove(th);
                        }

                        if (th.ThreadState == System.Threading.ThreadState.Suspended)
                        {
                            Log($"Watchdog detected thread death. Thread unexpectedly suspended.", "ERROR");
                            threads.Remove(th);
                        }

                        if (th.ThreadState == System.Threading.ThreadState.Aborted)
                        {
                            Log($"Watchdog detected thread death. Thread unexpectedly aborted.", "ERROR");
                            threads.Remove(th);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                }
                catch (Exception ex)
                {
                    Log($"Error in watchdog thread:\r\n{ex.ToString()}", "ERROR");
                }
            }
        }

        private static List<Thread> threads = new List<Thread>();
        private static Thread threadWatchdog;
        private static int logAppendDelay = 40;

        public static void Main()
        {
            ThreadAdd();
            Log($"PID: {Process.GetCurrentProcess().Id} | Netcraft Version: {NETCRAFT_VERSION}");
            if (Thread.CurrentThread.ManagedThreadId != 1)
            {
                CrashReport(new Exception("The main thread ID was not 1"));
            }

            loopThread = new Thread(ThreadLoop);
            loopThread.Name = "Loop";
            loopThread.Start();
            threadWatchdog = new Thread(watchdogThreadLoop);
            threadWatchdog.Start();
            Thread.CurrentThread.Name = "Main";
            Console.Title = "NCore (Netcraft Version 1.2)";
            LoadBanlist();
            foreach (var i in File.ReadAllText("./auth.txt", Encoding.UTF8).Split(Constants.vbCrLf))
            {
                if (i.Length < 2)
                    continue;
                if (playerPasswords.ContainsKey(i.Split("=")[0]))
                    continue;
                playerPasswords.Add(i.Split("=")[0], i.Split("=").Last());
            }

            LoadConfig();
            LoadCommands();
            Netcraft.dobc += eventhandler_a;
            foreach (var s in Directory.GetFiles("./plugins"))
            {
                if (!(s.Split(".").Last() == "dll"))
                {
                    continue;
                }

                var p = PluginManager.Load(s);
                string result = p.OnLoad();
                if (result is object)
                {
                    Log($"[Plugin Loader] Ошибка при загрузке плагина {p.Name}. Результат загрузки:{Constants.vbCrLf}{result}", "ERROR");
                    PluginManager.Plugins.Remove(p);
                    continue;
                }

                Log($"[Plugin Loader] Плагин загружен успешно: {p.Name}");
            }

            if (File.Exists(Conversions.ToString(worldfile)))
            {
                World = sv.Load(File.ReadAllText(Conversions.ToString(worldfile), encoding: Encoding.UTF8));
            }
            else
            {
                World = WorldGenerator.Generate();
                File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World), Encoding.UTF8);
            }

            Start();
            while (true)
            {
                string m = Console.ReadLine();
                var args = m.Split(" ").Skip(1).ToArray();
                string cmd = m.Split(" ")[0];
                var toRun = default(Command);
                foreach (var local_a in Netcraft.GetCommands())
                {
                    if ((local_a.Name.ToLower() ?? "") == (cmd.ToLower() ?? ""))
                    {
                        toRun = local_a;
                        break;
                    }
                    if (local_a.Aliases.Contains(cmd.ToLower()))
                    {
                        toRun = local_a;
                        break;
                    }
                }

                if (IsNothing(toRun))
                {
                    Log("Неизвестная команда.");
                }
                else
                {
                    bool y;
                    try
                    {
                        y = toRun.OnCommand(Netcraft.ConsoleCommandSender, toRun, args, m);
                    }
                    catch (Exception ex)
                    {
                        Log("Произошла внутренняя ошибка при выполнении данной команды.");
                        LogError(ex);
                        return;
                    }

                    if (!y)
                    {
                        Log("Использование: " + toRun.Usage);
                    }
                }
            }
        }

        public static void UpdateList(string str, bool relay = false)
        {
            ThreadAdd();
            Console.WriteLine(str);
        }

        public delegate void xSend(string str, string except, bool readyClientsOnly);

        public static void Send(string str, string except = null, bool readyClientsOnly = false)
        {
            ThreadAdd();
            for (int x = 0, loopTo = clientList.Count - 1; x <= loopTo; x++)
            {
                try
                {
                    if (except != null)
                    {
                        if ((clientList[x].Username ?? "") == (except ?? ""))
                            continue;
                    }

                    if (readyClientsOnly)
                    {
                        if (clientList[x].IsLoaded)
                            clientList[x].Send(str);
                    }
                    else
                    {
                        clientList[x].Send(str);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        clientList.RemoveAt(x);
                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }
        }

        public static void SaveBanlist()
        {
            ThreadAdd();
            string a = "";
            foreach (var b in Netcraft.GetBannedPlayers())
                a += b + Constants.vbLf;
            File.WriteAllText("banned-players.txt", a, Encoding.UTF8);
        }

        public static void LoadBanlist()
        {
            ThreadAdd();
            foreach (var a in File.ReadAllText("banned-players.txt", Encoding.UTF8).Split(Constants.vbLf))
                Netcraft.field_a.Add(a);
        }

        public static void AcceptClient(IAsyncResult ar)
        {
            ThreadAdd();
            try
            {
                if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = "Network Join";
                pClient = new NetworkPlayer(Listning.EndAcceptTcpClient(ar));
                pClient.a += MessageReceived;
                pClient.b += (_) => NCore.ClientExited(pClient, false);
                clientList.Add(pClient);
                Netcraft.clientList = clientList;
                Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
                Log($"Входящее соединение с IP: {pClient.GetIp()}");
            }
            catch (Exception ex)
            {
            }
        }

        private static string toAppend = "";

        internal static void Log(string arg0, string arg1 = "INFO")
        {
            ThreadAdd();
            if (arg1 == "ERROR")
                Console.ForegroundColor = ConsoleColor.Red;
            if (arg1 == "SEVERE")
                Console.ForegroundColor = ConsoleColor.Red;
            if (arg1 == "WARNING")
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (arg1 == "INFO")
                Console.ForegroundColor = ConsoleColor.White;
            UpdateList($"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}]: {arg0}");
            toAppend += $"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}]: {arg0}" + Constants.vbCrLf;
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void LogPlugin(string arg0, Plugin arg2, string arg1 = "INFO")
        {
            ThreadAdd();
            if (arg1 == "ERROR")
                Console.ForegroundColor = ConsoleColor.Red;
            if (arg1 == "SEVERE")
                Console.ForegroundColor = ConsoleColor.Red;
            if (arg1 == "WARNING")
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (arg1 == "INFO")
                Console.ForegroundColor = ConsoleColor.White;
            UpdateList($"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}] ({arg2.Name}): {arg0}");
            toAppend += $"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}] ({arg2.Name}): {arg0}" + Constants.vbCrLf;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogError(Exception ex)
        {
            ThreadAdd();
            Log($"Exception in thread {Thread.CurrentThread.ManagedThreadId.ToString()}:" + Constants.vbCrLf + ex.ToString(), "ERROR");
        }

        private static Thread MainThread;
        private static bool ToExit = false;

        public static void StopServer()
        {
            Log("Stopping the server...");
            foreach (var plugin in PluginManager.Plugins)
            {
                plugin.OnUnload();
                Log($"Плагин выгружен: {plugin.Name}");
            }

            Kickall("Сервер выключается");
            Listning.Stop();
            SaveWorld();
            SaveBanlist();
            Process.GetCurrentProcess().Kill();
        }

        private static bool isIllegalValue(object v, params object[] vs)
        {
            if (!vs.Contains(v)) return true;
            return false;
        }

        public static void Kickall(string m)
        {
            foreach (var a in clientList)
                a.Kick(m);
        }

        public enum KickReason
        {
            LONG_PACKET,
            SERVER_ERROR,
            ILLEGAL_STATE,
            NONE
        }

        private static Hashtable playerPasswords = new Hashtable();
        private static StringCollection loggedIn = new StringCollection();

        public static void MessageReceived(string str, NetworkPlayer n)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "Network Packet";
            ThreadAdd();
            try
            {
                var a = str.Split("?");
                if (str == "ping")
                {
                    Log($"Pinged from {n.GetIp()}");
                    if (allowQuery == 1)
                    {
                        n.Send("name?" + name);
                        Thread.Sleep(50);
                        n.Send("motd?" + motd);
                        Thread.Sleep(50);
                        n.Send("players?" + clientList.Count.ToString() + "/" + maxPlayers.ToString());
                    }
                    else
                    {
                        n.Send("name?Access denied");
                        Thread.Sleep(50);
                        n.Send("motd?Sorry, but query is disabled for this server.");
                        Thread.Sleep(50);
                        n.Send("players?Denied");
                    }

                    n.Disconnect();
                    clientList.Remove(n);
                    return;
                }

                if (a[0] == "setname")
                {
                    if (!string.IsNullOrEmpty(n.Username))
                    {
                        return;
                    }

                    Log($"{a[1]} [/{n.GetIp()}] подключился к серверу.");
                    if (maxPlayers + 1 == clientList.Count)
                    {
                        n.Kick("Сервер заполнен!");
                        return;
                    }

                    foreach (var netplayer in clientList)
                    {
                        if ((netplayer.UUID ?? "") == (n.UUID ?? ""))
                            continue;
                        if ((netplayer.Username ?? "") == (a[1] ?? ""))
                        {
                            n.Kick("Игрок с таким именем уже играет на сервере.");
                            return;
                        }
                    }



                    n.Username = a[1];
                    n.senderName = n.Username;
                    if (!Regex.Match(n.Username, "^[a-zA-Z0-9_]*").Success)
                    {
                        n.Kick("Запрещённые символы в никнейме.");
                        return;
                    }

                    if (Netcraft.IsBanned(n.Username))
                    {
                        n.Kick("Вы были забанены на этом сервере!");
                        return;
                    }

                    if(enableAuth == 1)
                    {

                    } else if(enableAuth == 0)
                    {
                        loggedIn.Add(n.Username);
                        n.IsAuthorized = true;
                    } else
                    {
                        CrashReport(new Exception("Invalid config property value"));
                    }

                    Send("addplayer?" + a[1], n.Username);
                    n.PlayerInventory = new Inventory(n);
                    if (File.Exists(Conversions.ToString(Operators.AddObject(Operators.AddObject(Operators.AddObject(Application.StartupPath, "/playerdata/"), n.Username), ".txt"))))
                    {
                        PlayerInfoSaveLoad.Load(n, File.ReadAllText(Conversions.ToString(Operators.AddObject(Operators.AddObject(Operators.AddObject(Application.StartupPath, "/playerdata/"), n.Username), ".txt")), Encoding.UTF8));
                    }
                    else
                    {
                        n.PlayerInventory.AddItem(new ItemStack(Material.WOODEN_PICKAXE, 1));
                        n.PlayerInventory.AddItem(new ItemStack(Material.WOODEN_AXE, 1));
                        n.PlayerInventory.AddItem(new ItemStack(Material.WOODEN_SWORD, 1));
                        n.PlayerInventory.AddItem(new ItemStack(Material.WOODEN_SHOVEL, 1));
                    }

                    Log(n.Username + " присоединился к игре");
                    Chat(n.Username + " заходит на сервер");


                    // n.Send("blockchange?500?50?Red")
                }

                if (string.IsNullOrEmpty(n.Username))
                {
                    n.Kick("You can't send another packets");
                    return;
                }

                n.senderAdmin = n.IsAdmin;
                if (a[0] == "world")
                {
                    foreach (var b in World.Blocks)
                        n.SendBlockChange(b.Position, b.Type, packetQueue: true, isBackground: b.IsBackground);
                    n.PacketQueue.SendQueue();
                    foreach (var p in clientList)
                    {
                        if ((p.Username ?? "") == (n.Username ?? ""))
                            continue;
                        if (p.IsSpectator)
                            continue;
                        Thread.Sleep(100);
                        n.PacketQueue.AddQueue("addplayer?" + p.Username);
                        Thread.Sleep(100);
                        n.PacketQueue.AddQueue("moveplayer?" + p.Username + "?" + p.Position.X.ToString() + "?" + p.Position.Y.ToString());
                        try
                        {
                            if (IsNothing(p.SelectedItem))
                                continue;
                            Thread.Sleep(100);
                            n.Send((Conversions.ToDouble("itemset?" + p.Username + "?") + (double)p.SelectedItem.Type).ToString());
                        }
                        catch (Exception ex)
                        {
                            LogError(ex);
                        }
                    }
                    n.IsLoaded = true;
                    Thread.Sleep(100);
                    if(enableAuth == 1) n.Chat("Пожалуйста введите свой пароль в чат для авторизации. Это никто не увидит.");
                    if (everyBodyAdmin == 1)
                    {
                        n.IsAdmin = true;
                    }

                    var ev = new netcraft.server.api.events.PlayerJoinEventArgs(n);
                    NCSApi.REPlayerJoinEvent(ev);
                }

                if (!loggedIn.Contains(n.Username))
                {
                    if (a[0] == "chat")
                    {
                        if (!Regex.Match(a[1], "^[a-zA-Z0-9_]*").Success)
                        {
                            n.SendMessage("Пароль может содержать символы только [a-z, A-Z, 0-9]");
                        }

                        if (!playerPasswords.Keys.Cast<string>().ToArray().Contains(n.Username))
                        {
                            playerPasswords.Add(n.Username, a[1]);
                            Log($"{n.Username} registered from {n.GetIp()}!");
                        }

                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(playerPasswords[n.Username], a[1], false)))
                        {
                            n.SendMessage("Вы успешно авторизовались.");
                            loggedIn.Add(n.Username);
                            Log($"{n.Username} logged in!");
                            n.Send("teleport?" + n.Position.X.ToString() + "?" + n.Position.Y.ToString());
                            n.IsAuthorized = true;
                            netcraft.server.api.events.PlayerLoginEventArgs ev = new netcraft.server.api.events.PlayerLoginEventArgs(n);
                            NCSApi.REPlayerLoginEvent(ev);
                        }
                        else
                        {
                            Log($"{n.Username} used wrong password", "ERROR");
                            n.SendMessage("Вы ввели неверный пароль.");
                        }
                    }

                    return;
                }

                if (a[0] == "craft")
                {
                    try
                    {
                        var m = Enum.Parse(typeof(Material), a[1].ToUpper());
                        n.Craft((Material)m);
                    }
                    catch (Exception ex)
                    {
                        n.SendMessage("An internal error occured.");
                    }
                }

                if (a[0] == "entityplayermove")
                {
                    var mto = new Point(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
                    n.PlayerRectangle = new Rectangle(mto, new Size(47, 92));
                    var ev = new netcraft.server.api.events.PlayerMoveEventArgs(n.Position, mto, n);
                    if (ev.GetCancelled())
                    {
                        n.Send($"teleport?{n.Position.X.ToString()}?{n.Position.Y.ToString()}");
                        return;
                    }

                    var v = Normalize(mto - (Size)n.Position);
                    if (!n.DisableMovementCheck)
                    {
                        if (v.X > 10 | v.Y > 10 | v.X < -10 | v.Y < -10)
                        {
                            Log($"{n.Username} переместился слишком быстро! {v.X},{v.Y}", "WARNING");
                            n.Send($"teleport?{n.Position.X}?{n.Position.Y}");
                            return;
                        }

                        if (allowFlight == 0)
                        {
                            if (mto.Y - n.Position.Y > 0)
                            {
                                n.MovedInAir = 0;
                            }

                            if (!n.IsOnGround)
                            {
                                n.MovedInAir += 1;
                                if (n.MovedInAir == 50)
                                {
                                    Log($"{n.Username} переместился неправильно (Полёт)! {v.X},{v.Y}", "WARNING");
                                    n.Kick("Flying is not enabled on this server");
                                    return;
                                }
                            }
                        }

                        foreach (var b in World.Blocks)
                        {
                            var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                            var brec = new Rectangle(bpos, new Size(32, 32));
                            if (DistanceBetweenPoint(bpos, n.Position) > 10 * 32)
                                continue;
                            if (bpos.Y > n.Position.Y + 85)
                                continue;
                            if (brec.IntersectsWith(n.PlayerRectangle))
                            {
                                if (b.IsBackground)
                                    continue;
                                if (b.Type == EnumBlockType.WATER)
                                    continue;
                                if (b.Type == EnumBlockType.SAPLING)
                                    continue;
                                if (n.NoClip)
                                    return;
                                Log($"{n.Username} переместился неправильно (В блок)!", "WARNING");
                                if (v.Y > 1)
                                {
                                    n.Teleport(n.Position.X, n.Position.Y - 2);
                                }
                                else
                                {
                                    n.Send($"teleport?{n.Position.X}?{n.Position.Y}");
                                }
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    if (mto.Y > n.Position.Y)
                    {
                        bool grounded = false;
                        bool inWater = false;
                        foreach (var b in World.Blocks)
                        {
                            var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                            var brec = new Rectangle(bpos, new Size(32, 32));
                            if (DistanceBetweenPoint(bpos, n.Position) > 10 * 32)
                                continue;
                            if (brec.IntersectsWith(n.PlayerRectangle))
                            {
                                if (b.IsBackground)
                                    continue;
                                if (b.Type == EnumBlockType.WATER)
                                    inWater = true;
                                grounded = true;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (!grounded)
                        {
                            n.FallDistance += 1;
                            if (inWater)
                            {
                                n.FallDistance = 0;
                            }
                        }
                        else
                        {
                            if (inWater)
                            {
                                n.FallDistance = 0;
                            }

                            if (n.FallDistance > 3 * 32)
                            {
                                n.Damage((int)(n.FallDistance / 16d / 3d), "упал(а) с высокого места");
                            }

                            n.FallDistance = 0;
                        }
                    }

                    n.Position = mto;
                    if (n.IsSpectator)
                        return;
                    if (n.Position.Y > 619)
                    {
                        n.Damage(1, "выпал(а) из мира");
                    }

                    Send("updateplayerposition?" + n.Username + "?" + a[1] + "?" + a[2], n.Username);
                }

                try
                {

                    // If a(0) = "craft" Then
                    // Dim enumMaterial = [Enum].Parse(GetType(Material), a(1))
                    // n.Craft(enumMaterial)
                    // End If
                    if (a[0] == "chat")
                    {
                        string message = string.Join("?", a.Skip(1).ToArray());
                        var ev = new netcraft.server.api.events.PlayerChatEventArgs(n, message);
                        NCSApi.REPlayerChatEvent(ev);
                        if (ev.GetCancelled())
                            return;
                        if (message.Length == 0) return;
                        if (message[0] == '/')
                        {
                            if (commandsConsoleOnly == 1)
                            {
                                n.SendMessage("Команды в игре были отключены в настройках сервера.");
                                return;
                            }

                            var arr = message.Split(" ");
                            var lbl = message.Skip(1).ToArray();
                            string label = new string(lbl);
                            var args = arr.Skip(1).ToArray();
                            var cmd = default(Command);
                            Log($"{n.Username} запросил команду сервера: /{label}");
                            foreach (var g in Netcraft.GetCommands())
                            {
                                if (g.Name.ToLower() == label.Split(" ")[0].ToLower())
                                {
                                    cmd = g;
                                    break;
                                }
                                if(g.Aliases.Contains(label.Split(" ")[0].ToLower()))
                                {
                                    cmd = g;
                                    break;
                                }
                            }

                            if (IsNothing(cmd))
                            {
                                n.SendMessage("Команда не обнаружена. Введите /help для списка команд.");
                                return;
                            }

                            bool y;
                            try
                            {
                                y = cmd.OnCommand(n, cmd, args, label);
                            }
                            catch (Exception ex)
                            {
                                n.SendMessage("Произошла внутренняя ошибка при выполнении данной команды.");
                                Log($"Command error occured in \"{cmd.Name}\" performing by {n.Username}:", "WARNING");
                                Log(ex.ToString(), "WARNING");
                                return;
                            }

                            if (Conversions.ToBoolean(!y))
                            {
                                n.SendMessage("Использование: " + cmd.Usage);
                            }
                        }
                        else
                        {
                            Send("chat?" + chatFormat.Replace("%1", n.Username).Replace("%2", message));
                            Log(chatFormat.Replace("%1", n.Username).Replace("%2", message));
                        }
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    LogError(ex);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    Log(ex.ToString(), "ERROR");
                    CrashReport(ex);
                }

                try
                {
                    if (a[0] == "update_inventory")
                    {
                        // n.Send("clearinventory")
                        // Thread.Sleep(100)
                        // For Each i In n.PlayerInventory.Items
                        // Thread.Sleep(100)
                        // n.Send("additem?" + i.Type.ToString + " x " + i.Count.ToString)
                        // Next
                        n.UpdateInventory();
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.ToString() + Constants.vbCrLf + ex.Source, "SEVERE");
                    LogError(ex);
                }

                if (a[0] == "selectitem")
                {
                    foreach (var i in n.PlayerInventory.Items)
                    {
                        if ((i.Type.ToString() + " x " + i.Count.ToString() ?? "") == (a[1] ?? ""))
                        {
                            n.SelectedItem = i;
                            try
                            {
                                Send("itemset?" + n.Username + "?" + n.SelectedItem.Type.ToString(), n.Username);
                                n.Send("itemset?@?" + n.SelectedItem.Type.ToString());
                            }
                            catch (Exception ex)
                            {
                                Log(ex.ToString(), "ERROR");
                                LogError(ex);
                            }

                            break;
                        }
                    }
                }

                if (a[0] == "rightclick")
                {
                    try
                    {
                        var block = World.GetBlockAt(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
                        if (block == null)
                        {
                            n.Kick("Internal server error occured.");
                            return;
                        }

                        var ev = new netcraft.server.api.events.BlockRightClickEvent(n, block);
                        NCSApi.REBlockRightClickEvent(ev);
                    }
                    catch (Exception ex)
                    {
                        Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{ex.ToString()}");
                        return;
                    }
                }

                if (a[0] == "block_break")
                {
                    try
                    {
                        if (IsNothing(n.SelectedItem))
                            return;
                        foreach (var b in World.Blocks)
                        {
                            if (b.Position.X == Conversions.ToInteger(a[1]))
                            {
                                if (b.Position.Y == Conversions.ToInteger(a[2]))
                                {
                                    if (b.Unbreakable)
                                        return;
                                    var pos = Normalize(n.Position);

                                    var ev = new netcraft.server.api.events.BlockBreakEventArgs(n, b);
                                    NCSApi.REBlockBreakEvent(ev);
                                    if (DistanceBetween(pos.X, pos.Y, b.Position.X, b.Position.Y) > 6d)
                                    {
                                        ev.SetCancelled(true);
                                        return;
                                    }
                                    if (ev.GetCancelled())
                                    {
                                        return;
                                    }

                                    Send("removeblock?" + a[1] + "?" + a[2]);
                                    if (b.Type == EnumBlockType.STONE)
                                    {
                                        if (n.SelectedItem.Type == Material.WOODEN_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.STONE_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                    }

                                    if (b.Type == EnumBlockType.COAL_ORE)
                                    {
                                        if (n.SelectedItem.Type == Material.WOODEN_PICKAXE)
                                            n.Give(Material.COAL, 3);
                                        if (n.SelectedItem.Type == Material.STONE_PICKAXE)
                                            n.Give(Material.COAL, 3);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.COAL, 3);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.COAL, 3);
                                    }

                                    if (b.Type == EnumBlockType.IRON_ORE)
                                    {
                                        if (n.SelectedItem.Type == Material.STONE_PICKAXE)
                                            n.Give(Material.IRON, 3);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.IRON, 3);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.IRON, 3);
                                    }

                                    if (b.Type == EnumBlockType.IRON_BLOCK)
                                    {
                                        if (n.SelectedItem.Type == Material.STONE_PICKAXE)
                                            n.Give(Material.IRON_BLOCK);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.IRON_BLOCK);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.IRON_BLOCK);
                                    }

                                    if (b.Type == EnumBlockType.DIAMOND_BLOCK)
                                    {
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.DIAMOND_BLOCK);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.DIAMOND_BLOCK);
                                    }

                                    if (b.Type == EnumBlockType.DIAMOND_ORE)
                                    {
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.DIAMOND, 3);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.DIAMOND, 3);
                                    }

                                    if (b.Type == EnumBlockType.DIRT)
                                    {
                                        n.Give(Material.DIRT);
                                    }

                                    if (b.Type == EnumBlockType.COBBLESTONE)
                                    {
                                        if (n.SelectedItem.Type == Material.WOODEN_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.STONE_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.COBBLESTONE);
                                    }

                                    if (b.Type == EnumBlockType.GOLD_ORE)
                                    {
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.GOLD, 3);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.GOLD, 4);
                                    }

                                    if (b.Type == EnumBlockType.GOLD_BLOCK)
                                    {
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.GOLD_BLOCK);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.GOLD_BLOCK);
                                    }

                                    if (b.Type == EnumBlockType.WOOD)
                                    {
                                        n.Give(Material.WOOD);
                                    }

                                    if (b.Type == EnumBlockType.PLANKS)
                                    {
                                        n.Give(Material.PLANKS);
                                    }

                                    if (b.Type == EnumBlockType.SAND)
                                    {
                                        n.Give(Material.SAND);
                                    }

                                    if (b.Type == EnumBlockType.FURNACE)
                                    {
                                        n.Give(Material.FURNACE);
                                    }

                                    if (b.Type == EnumBlockType.LEAVES)
                                    {
                                        var rnd = new Random();
                                        int r = rnd.Next(1, 4);
                                        if (r == 2)
                                        {
                                            n.Give(Material.SAPLING);
                                        }
                                        else
                                        {
                                            n.Give(Material.LEAVES);
                                        }
                                    }

                                    World.Blocks.Remove(World.GetBlockAt(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2])));
                                    break;
                                }
                                else
                                {
                                    // UpdateList($"{n.Username} trying to break {a(1)} {a(2)}, but Y was {b.Position.Y}")
                                }
                            }
                            else
                            {
                                // UpdateList($"{n.Username} trying to break {a(1)} {a(2)}, but X was {b.Position.X}")
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{ex.ToString()}", "WARNING");
                        return;
                    }
                }

                try
                {
                    if (a[0] == "block_place")
                    {
                        var placeAt = new Point(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
                        EnumBlockType type;
                        var pos = Normalize(n.Position);
                        placeAt = Normalize(placeAt);

                        type = (EnumBlockType)Enum.Parse(typeof(EnumBlockType), n.SelectedItem.Type.ToString());
                        var b = new Block(placeAt, type, false, false);
                        var ev = new netcraft.server.api.events.BlockPlaceEventArgs(n, b);
                        NCSApi.REBlockPlaceEvent(ev);
                        if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d)
                        {
                            ev.SetCancelled(true);
                            return;
                        }
                        if (ev.GetCancelled())
                        {
                            return;
                        }

                        foreach (var g in clientList)
                            g.SendBlockChange(placeAt, type);
                        World.Blocks.Add(b);
                        n.SelectedItem.Count -= 1;
                        if (n.SelectedItem.Count <= 0)
                        {
                            n.PlayerInventory.Items.Remove(n.SelectedItem);
                            n.SelectedItem = null;
                        }

                        n.UpdateInventory();
                    }

                    if (a[0] == "block_place_bg")
                    {
                        var placeAt = new Point(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
                        EnumBlockType type;
                        var pos = Normalize(n.Position);
                        placeAt = Normalize(placeAt);
                        type = (EnumBlockType)Enum.Parse(typeof(EnumBlockType), n.SelectedItem.Type.ToString());
                        var b = new Block(placeAt, type, false, true);
                        var ev = new netcraft.server.api.events.BlockPlaceEventArgs(n, b);
                        NCSApi.REBlockPlaceEvent(ev);
                        if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d)
                        {
                            ev.SetCancelled(true);
                            return;
                        }
                        if (ev.GetCancelled())
                        {
                            return;
                        }

                        foreach (var g in clientList)
                            g.SendBlockChange(placeAt, type, isBackground: true);
                        World.Blocks.Add(b);
                        n.SelectedItem.Count -= 1;
                        if (n.SelectedItem.Count <= 0)
                        {
                            n.PlayerInventory.Items.Remove(n.SelectedItem);
                            n.SelectedItem = null;
                        }

                        n.UpdateInventory();
                    }
                }
                catch (NullReferenceException ex)
                {
                }
                catch (Exception ex)
                {
                    Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{ex.ToString()}", "WARNING");
                    return;
                }

                try
                {
                    if (a[0] == "pvp")
                    {
                        var nd = default(NetworkPlayer);
                        foreach (var i in clientList)
                        {
                            if ((i.Username ?? "") == (a[1] ?? ""))
                            {
                                nd = i;
                            }
                        }

                        if (IsNothing(nd))
                        {
                            n.Kick("Attempting to attack an invalid player");
                            return;
                        }

                        if ((nd.Username ?? "") == (n.Username ?? ""))
                        {
                            n.Kick("Attempting to attack self");
                            return;
                        }

                        if (DistanceBetweenPoint(Normalize(n.Position), Normalize(nd.Position)) > 5d)
                        {
                            n.DoWarning("Unreachable player!");
                            return;
                        }

                        if (!IsNothing(n.SelectedItem))
                        {
                            if (n.SelectedItem.Type == Material.DIAMOND_SWORD)
                            {
                                nd.Damage(35, n);
                            }
                            else if (n.SelectedItem.Type == Material.IRON_SWORD)
                            {
                                nd.Damage(25, n);
                            }
                            else if (n.SelectedItem.Type == Material.STONE_SWORD)
                            {
                                nd.Damage(20, n);
                            }
                            else if (n.SelectedItem.Type == Material.WOODEN_SWORD)
                            {
                                nd.Damage(14, n);
                            }
                            else
                            {
                                nd.Damage(9, n);
                            }
                        }
                        else
                        {
                            nd.Damage(5, n);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                }
                catch (Exception ex)
                {
                    Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{ex.ToString()}", "WARNING");
                    return;
                }

                try
                {
                    if (a[0] == "furnace")
                    {
                        if (IsNothing(n.SelectedItem))
                            return;
                        n.Furnace(World.GetBlockAt(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2])), Material.COAL, n.SelectedItem.Type);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{ex.ToString()}", "WARNING");
                    return;
                }
            }
            catch (Exception globalEx)
            {
                Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{globalEx.ToString()}", "WARNING");
                n.Kick("Internal server error occured.");
                return;
            }
        }

        public static void CrashReport(Exception ex)
        {
            ThreadAdd();
            string crashText = "Netcraft Crash Report" + Constants.vbCrLf + $"Server crashed at {DateTime.Now.ToString()}" + Constants.vbCrLf + $"{ex.GetType().ToString()}: {ex.Message}{Constants.vbCrLf}== STACK TRACE =={Constants.vbCrLf}{ex.InnerException.StackTrace}{Constants.vbCrLf}{Constants.vbCrLf}" + $"Exception.TargetSite: {ex.TargetSite}" + Constants.vbCrLf + $"Exception.Source: {ex.Source}";
            File.WriteAllText(Conversions.ToString(Operators.AddObject(Operators.AddObject(Operators.AddObject(Application.StartupPath, @"\crash-reports\"), DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-")), ".txt")), crashText);
            // Console.WriteLine($"OOPS!! THE SERVER IS CRASHED{vbCrLf}{vbCrLf}Crash report is saved.{vbCrLf}{vbCrLf}{crashText}")
            Console.Error.WriteLine("OOPS! The server is crashed" + Constants.vbCrLf + Constants.vbCrLf + crashText);
        }

        public static double DistanceBetween(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2d) + Math.Pow(y2 - y1, 2d));
        }

        public static double DistanceBetweenPoint(Point p1, Point p2)
        {
            int x1 = p1.X;
            int x2 = p2.X;
            int y1 = p1.Y;
            int y2 = p2.Y;
            return Math.Sqrt(Math.Pow(x2 - x1, 2d) + Math.Pow(y2 - y1, 2d));
        }

        public static Point Normalize(Point p)
        {
            var point = new Point()
            {
                X = p.X / 32,
                Y = p.Y / 32
            };
            return point;
        }

        public static bool IsNothing(object expession)
        {
            return expession is null;
        }

        public static void Chat(string arg0)
        {
            Send("chat?" + arg0);
        }

        public static void SaveAuth()
        {
            string txt = "";
            foreach (var t in playerPasswords.Keys.Cast<string>().ToArray())
                txt = Conversions.ToString(txt + Operators.AddObject(Operators.AddObject(t + "=", playerPasswords[t]), Constants.vbCrLf));
            File.WriteAllText("./auth.txt", txt, Encoding.UTF8);
        }

        public static void ClientExited(NetworkPlayer client, bool isError = false)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "Network Leave";
            ThreadAdd();
            if (client.IsLoaded)
            {
                var ev = new netcraft.server.api.events.PlayerLeaveEventArgs(client);
                NCSApi.REPlayerLeaveEvent(ev);
                File.WriteAllText($"./playerdata/{client.Username}.txt", PlayerInfoSaveLoad.Save(client), Encoding.UTF8);
                if (!isError)
                {
                    Chat(client.Username + " покинул игру");
                    Log(client.Username + " покинул игру");
                }
                else
                {
                    client.Kick("An internal server error occured");
                    Chat(client.Username + " left the game due to an error.");
                    Log(client.Username + " left the game due to an error.");
                }

                Send("removeplayer?" + client.Username);
                if (loggedIn.Contains(client.Username))
                {
                    loggedIn.Remove(client.Username);
                }
            }

            clientList.Remove(client);
            Netcraft.clientList = clientList;
        }

        private static bool Listening = false;

        private static void Start()
        {
            Listning = new TcpListener(IPAddress.Any, 6575);
            if (Listening == false)
            {
                Listning.Start();
                Log("Сервер NetCraft 1.1 запускается");
                Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
                Listening = true;
            }
            else
            {
                Listning.Stop();
            }
        }

        private static void OnClose()
        {
            File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World), Encoding.UTF8);
            foreach (var c in clientList)
                ClientExited(c);
        }
        public static void ForceCrash()
        {
            throw new NullReferenceException();
        }

        public static void SaveWorld()
        {
            File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World));
        }

        public static void SendCommandFeedback(string a, CommandSender b)
        {
            Log($"{b.GetName()}: {a}");
            Send($"chat?{b.GetName()}: {a}");
        }

        public static string Left(string a, int b)
        {
            return a.Substring(0, Math.Min(b, a.Length));
        }
    }

    public class Application
    {
        public static string StartupPath { get; } = ".";
    }
}