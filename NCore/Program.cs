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

    class NCore
    {
        internal WorldServer World { get; set; }

        private TcpListener Listning;
        internal readonly List<NetworkPlayer> players = new List<NetworkPlayer>();
        private NetworkPlayer pClient;
        // Dim npc As EntityPlayerNPC = New EntityPlayerNPC
        private readonly object worldfile = "./world.txt";
        private readonly object configfile = "./server.properties";
        private readonly SaveLoad sv = new SaveLoad();
        internal int maxPlayers = 20;
        private string chatFormat = "%1 %2";
        private int everyBodyAdmin = 0;
        private int allowFlight = 0;
        private string motd = "A NCore server";
        private string name;
        private int allowQuery = 0;
        private int commandsConsoleOnly = 0;
        private int enableAuth = -1;

        public bool IsSingleplayerServer { get; set; } = false;

        private int worldloadDelay = 40;

        public void LoadConfig()
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

        public void OnCrash(object sender, ThreadExceptionEventArgs e)
        {
            CrashReport(e.Exception);
        }

        public void LoadCommands()
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
            Netcraft.AddCommand(new Commands.Commandsave());
        }

        private void eventhandler_a(string m)
        {
            Log(m);
            Send("chat?" + m);
        }

        private Thread loopThread;
        internal const string NETCRAFT_VERSION = "1.3-ALPHA";
        internal const string NCORE_VERSION = "0.4";

        private void ThreadLoop()
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
                            TreeGenerator.GrowthTree(b.Position, World, b.IsBackground);
                            Log($"Tree growth at [{b.Position.X.ToString() + ", " + b.Position.Y.ToString()}]");
                            break;
                        }
                    }
                }
                foreach(Block b in World.Blocks)
                {
                    if(b.Type == EnumBlockType.CHEST)
                    {
                        if(World.GetChestAt(b.Position) == null)
                        {
                            BlockChest chest = new BlockChest(b.Position, b.IsBackground);
                            World.Chests.Add(chest);
                        }
                    } else { continue; }
                }
                foreach(BlockChest b in World.Chests)
                {
                    List<ItemStack> itemsToRemove = new List<ItemStack>();
                    foreach(ItemStack i in b.items)
                    {
                        if(i.Count < 1)
                        {
                            itemsToRemove.Add(i);
                        }
                    }
                    foreach(ItemStack i in itemsToRemove)
                    {
                        b.items.Remove(i);
                    }
                    if(itemsToRemove.Count > 0) Log($"Removed {itemsToRemove.Count} zero-count items in chest at [{b.Position.X.ToString()},{b.Position.Y.ToString()}]", "WARNING");
                }
                foreach (NetworkPlayer n in players)
                {

                    foreach (var b in World.Blocks)
                    {
                        var bpos = new Point(b.Position.X * 32, b.Position.Y * 32);
                        var brec = new Rectangle(bpos, new Size(32, 32));
                        if (DistanceBetweenPoint(bpos, n.Position) > 10 * 32)
                            continue;
                        if (bpos.Y > n.Position.Y + 85)
                            continue;
                        if (b.IsBackground)
                            continue;
                        if (b.Type == EnumBlockType.WATER)
                            continue;
                        if (b.Type == EnumBlockType.SAPLING)
                            continue;
                        if (brec.IntersectsWith(n.PlayerRectangle))
                        {
                            if (n.NoClip)
                                break;
                            if (b.Type == EnumBlockType.LAVA)
                            {
                                n.Damage(10, "решил(а) поплавать в лаве");
                                break;
                            }

                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                Console.Title = $"NCore {NCORE_VERSION} (Netcraft {NETCRAFT_VERSION}) | {players.Count}/{maxPlayers.ToString()} players | {(Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split(".")[0]}MB used of {Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L}MB, {(Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L - Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split(".")[0]}MB free | Total Processor Time: {Process.GetCurrentProcess().TotalProcessorTime.ToString()} | Uptime: {(DateTime.Now - Process.GetCurrentProcess().StartTime).ToString().Split(".")[0]}";
            }
        }

        private List<Color> skyClr = new List<Color>();
        private int worldtime, stp;
        private Thread daytimeThread;

        public void daytimeThreadLoop()
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

        public void BroadcastSkyChange(Color clr)
        {
            try
            {
                foreach (var gg in players)
                    gg.SendSkyColorChange(clr);
            }
            catch (Exception ex)
            {
            }
        }

        public void GametimeInitialize()
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

        public void ThreadAdd()
        {
            if (!threads.Contains(Thread.CurrentThread))
                threads.Add(Thread.CurrentThread);
        }

        

        public void watchdogThreadLoop()
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
                            if(th.ManagedThreadId == 1)
                            {
                                CrashReport(new netcraft.server.api.exceptions.ThreadDeathException());
                                return;
                            }
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

        private List<Thread> threads = new List<Thread>();
        private Thread threadWatchdog;
        private int logAppendDelay = 40;

        public static void Main(string[] args)
        {
            Console.WriteLine("Что Вы хотите сделать?\r\n1 - запустить сервер");
            char result = Console.ReadKey().KeyChar;
            Console.WriteLine("\b");
            if(result == '1')
            {
                NCore instance = new NCore();
                nCore = instance;
                instance.Server();
                
            }
        }

        static NCore nCore;
        public static NCore GetNCore()
        {
            return nCore;
        }

        

        public void Server()
        {
            ThreadAdd();
            Log($"PID: {Process.GetCurrentProcess().Id} | Netcraft Version: {NETCRAFT_VERSION}");
            Console.WriteLine("███╗░░██╗███████╗████████╗░█████╗░██████╗░░█████╗░███████╗████████╗\n████╗░██║██╔════╝╚══██╔══╝██╔══██╗██╔══██╗██╔══██╗██╔════╝╚══██╔══╝\n██╔██╗██║█████╗░░░░░██║░░░██║░░╚═╝██████╔╝███████║█████╗░░░░░██║░░░\n██║╚████║██╔══╝░░░░░██║░░░██║░░██╗██╔══██╗██╔══██║██╔══╝░░░░░██║░░░\n██║░╚███║███████╗░░░██║░░░╚█████╔╝██║░░██║██║░░██║██║░░░░░░░░██║░░░\n╚═╝░░╚══╝╚══════╝░░░╚═╝░░░░╚════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░░░░░░░╚═╝░░░");
            if (Thread.CurrentThread.ManagedThreadId != 1)
            {
                CrashReport(new Exception("The main thread ID was not 1"));
            }

            Log("Сервер запускается. Пожалуйста подождите...");

            Log("Так выглядит информация.");
            Log("Так выглядит предупреждение", "WARNING");
            Log("Так выглядит важная информация", "SEVERE");
            Log("Так выглядит ошибка", "ERROR");

            Log("Step 1: Создание потока loop.");
            loopThread = new Thread(ThreadLoop);
            loopThread.Name = "Loop";
            loopThread.Start();
            Log("Step 1: loop поток создан и запущен.");
            Log("Step 2: Создание потока watchdog.");
            threadWatchdog = new Thread(watchdogThreadLoop);
            threadWatchdog.Start();
            Log("Step 2: watchdog поток создан и запущен.");
            Thread.CurrentThread.Name = "Main";

            Log("Step 3: Загрузка списка банов и паролей игроков.");
            LoadBanlist();
            foreach (var i in File.ReadAllText("./auth.txt", Encoding.UTF8).Split(Constants.vbCrLf))
            {
                if (i.Length < 2)
                    continue;
                if (playerPasswords.ContainsKey(i.Split("=")[0]))
                    continue;
                playerPasswords.Add(i.Split("=")[0], i.Split("=").Last());
            }
            Log("Step 3: Готово.");

            Log("Step 4: Загрузка конфигурации и встроенных команд.");
            LoadConfig();
            LoadCommands();
            Log("Step 4: Готово.");

            Netcraft.dobc += eventhandler_a;

            Log("Step 5: Загрузка плагинов.");
            int pluginLoadErrors = 0;
            int all = 0;
            int pluginLoadSuccess = 0;
            foreach (var s in Directory.GetFiles("./plugins"))
            {
                if (s.Split(".").Last() != "dll") continue;

                all++;

                var p = PluginManager.Load(s);
                string result = p.OnLoad();
                if (result != null)
                {
                    pluginLoadErrors++;
                    Log($"[Plugin Loader] Ошибка при загрузке плагина {p.Name}. Результат загрузки:{Constants.vbCrLf}{result}", "ERROR");
                    PluginManager.Plugins.Remove(p);
                    continue;
                }

                Log($"[Plugin Loader] Плагин загружен успешно: {p.Name}");
                pluginLoadSuccess++;
            }
            Log($"Step 5: Плагины загружены. Всего плагинов: {all.ToString()}; Успешно: {pluginLoadSuccess.ToString()}; Не загружено (ошибка): {pluginLoadErrors.ToString()}");

            Log("Step 6: Загрузка мира...");
            if (File.Exists(Conversions.ToString(worldfile)))
            {
                World = sv.Load(File.ReadAllText(Conversions.ToString(worldfile), encoding: Encoding.UTF8));
                List<Block> blocksToRemove = new List<Block>();
                foreach(Block b in World.Blocks)
                {
                    if(b.Position.X > 64)
                    {
                        blocksToRemove.Add(b);
                    }
                    if(b.Position.Y > 17)
                    {
                        blocksToRemove.Add(b);
                    }
                }
                foreach(Block b in blocksToRemove)
                {
                    World.Blocks.Remove(b);
                }
                if (blocksToRemove.Count > 0) Log($"Удалено {blocksToRemove.Count} блоков на запрещённых местах.", "WARNING");
                Log("Step 6: Мир загружен.");
            }
            else
            {
                Log("Step 6: Мир не найден, генерация нового мира.");
                World = WorldGenerator.Generate();
                File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World), Encoding.UTF8);
                Log("Step 6: Мир создан и загружен.");
            }

            Log("Step 7: Запуск TCP сервера.");
            Start();
            Log("Step 7: TCP сервер запущен.");

            Log("Сервер успешно запущен.");
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
                        Log("Произошла внутренняя ошибка при выполнении данной команды.\r\n" + ex.ToString());
                        y = true;
                        break;
                    }

                    if (!y)
                    {
                        Log("Использование: " + toRun.Usage);
                    }
                }
            }
        }

        public void UpdateList(string str, bool relay = false)
        {
            ThreadAdd();
            Console.WriteLine(str);
        }

        public delegate void xSend(string str, string except, bool readyClientsOnly);

        public void Send(string str, string except = null, bool readyClientsOnly = false)
        {
            ThreadAdd();
            for (int x = 0, loopTo = players.Count - 1; x <= loopTo; x++)
            {
                try
                {
                    if (except != null)
                    {
                        if ((players[x].Username ?? "") == (except ?? ""))
                            continue;
                    }

                    if (readyClientsOnly)
                    {
                        if (players[x].IsLoaded)
                            players[x].Send(str);
                    }
                    else
                    {
                        players[x].Send(str);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        players.RemoveAt(x);
                    }
                    catch (Exception ex1)
                    {
                    }
                }
            }
        }

        public void SaveBanlist()
        {
            ThreadAdd();
            string a = "";
            foreach (var b in Netcraft.GetBannedPlayers())
                a += b + Constants.vbLf;
            File.WriteAllText("banned-players.txt", a, Encoding.UTF8);
        }

        public void LoadBanlist()
        {
            ThreadAdd();
            foreach (var a in File.ReadAllText("banned-players.txt", Encoding.UTF8).Split(Constants.vbLf))
                Netcraft.field_a.Add(a);
        }

        public void AcceptClient(IAsyncResult ar)
        {
            ThreadAdd();
            try
            {
                if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = "Network Join";
                pClient = new NetworkPlayer(Listning.EndAcceptTcpClient(ar));
                pClient.a += MessageReceived;
                pClient.b += (_) => NCore.GetNCore().ClientExited(pClient, false);
                players.Add(pClient);
                Netcraft.clientList = players;
                Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
                Log($"Входящее соединение с IP: {pClient.GetIp()}");
            }
            catch (Exception ex)
            {
            }
        }

        private string toAppend = "";

        internal void Log(string arg0, string arg1 = "INFO")
        {
            ThreadAdd();
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.Write($"{DateTime.Now.ToString()} [");
            if (arg1 == "ERROR")
                Console.ForegroundColor = ConsoleColor.DarkRed;
            if (arg1 == "SEVERE")
                Console.ForegroundColor = ConsoleColor.Magenta;
            if (arg1 == "WARNING")
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (arg1 == "INFO")
                Console.ForegroundColor = ConsoleColor.White;
            //Console.Write(arg1);
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.Write($"] {arg0}\n");
            //UpdateList($"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}]: {arg0}");
            UpdateList($"[{DateTime.Now} {arg1}]: {arg0}");
            toAppend += $"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}]: {arg0}" + Constants.vbCrLf;
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal void LogPlugin(string arg0, Plugin arg2, string arg1 = "INFO")
        {
            ThreadAdd();
            Log($"{arg2.Name}: {arg0}", arg1);
        }

        public void LogError(Exception ex)
        {
            ThreadAdd();
            Log($"Exception in thread {Thread.CurrentThread.ManagedThreadId.ToString()}:" + Constants.vbCrLf + ex.ToString(), "ERROR");
        }

        private Thread MainThread;
        private bool ToExit = false;

        public void StopServer()
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

        private bool isIllegalValue(object v, params object[] vs)
        {
            if (!vs.Contains(v)) return true;
            return false;
        }

        public void Kickall(string m)
        {
            foreach (var a in players)
                a.Kick(m);
        }

        public enum KickReason
        {
            LONG_PACKET,
            SERVER_ERROR,
            ILLEGAL_STATE,
            NONE
        }

        private Hashtable playerPasswords = new Hashtable();
        private StringCollection loggedIn = new StringCollection();

        public void MessageReceived(string str, NetworkPlayer n)
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
                        n.Send("players?" + players.Count.ToString() + "/" + maxPlayers.ToString());
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
                    players.Remove(n);
                    return;
                }

                if (a[0] == "setname")
                {
                    if (!string.IsNullOrEmpty(n.Username))
                    {
                        return;
                    }

                    Log($"{a[1]} [/{n.GetIp()}] подключился к серверу.");
                    if (maxPlayers + 1 == players.Count)
                    {
                        n.Kick("Сервер заполнен!");
                        return;
                    }

                    foreach (var netplayer in players)
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
                    foreach (var p in players)
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
                            n.Send("itemset?" + p.Username + "?" + p.SelectedItem.Type.ToString());
                        }
                        catch (Exception ex)
                        {
                            LogError(ex);
                        }
                    }
                    n.IsLoaded = true;
                    Thread.Sleep(100);
                    n.Send("completeload");
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
                                if(n.MovedInAir != 50) n.MovedInAir--;
                            }

                            if (!n.IsOnGround)
                            {
                                n.MovedInAir++;
                                if (n.MovedInAir == 50)
                                {
                                    n.AntiFlyWarnings++;
                                    Log($"{n.Username} переместился неправильно (Полёт)! [{v.X}, {v.Y}] [{n.AntiFlyWarnings.ToString()} warnings]", "WARNING");
                                    if(n.AntiFlyWarnings == 10)
                                    {
                                        n.Kick("Flying is not enabled on this server");
                                    }
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
                                if (b.Type == EnumBlockType.LAVA)
                                    continue;
                                if (b.Type == EnumBlockType.SAPLING)
                                    continue;
                                if (n.NoClip)
                                    break;
                                Log($"{n.Username} переместился неправильно (В блок)!", "WARNING");
                                if (v.Y > 1)
                                {
                                    n.Teleport(n.Position.X, n.Position.Y - 2);
                                }
                                else
                                {
                                    n.Send($"teleport?{n.Position.X}?{n.Position.Y}");
                                }
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    bool noPosUpdate = false;
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
                                //break;
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
                                n.Damage(n.FallDistance / 16 / 3, "упал(а) с высокого места");
                                noPosUpdate = true;
                            }

                            n.FallDistance = 0;
                        }
                    }


                    if (!noPosUpdate) n.Position = mto;


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

                if (a[0] == "tochest")
                {
                    ItemStack item = null;

                    foreach(ItemStack i in n.PlayerInventory.Items)
                    {
                        if (a[1] == $"{i.Type.ToString()} x {i.Count.ToString()}")
                        {
                            item = i;
                            break;
                        }
                    }

                    if (item == null)
                    {
                        Log($"{n.SelectedItem} tried to put null item into chest.", "WARNING");
                        return;
                    }
                    n.OpenChest.AddItem(ref n, item);
                }

                if (a[0] == "fromchest")
                {
                    int item = -1;

                    foreach (ItemStack i in n.OpenChest.items)
                    {
                        if (a[1] == $"{i.Type.ToString()} x {i.Count.ToString()}")
                        {
                            item = n.OpenChest.items.IndexOf(i);
                            break;
                        }
                    }

                    if (item == -1)
                    {
                        Log($"{n.SelectedItem} tried to get null item from chest.", "WARNING");
                        return;
                    }
                    n.OpenChest.RemoveItem(ref n, item);
                }

                if (a[0] == "closechest")
                {
                    n.OpenChest = null;
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
                        if(block.Type == EnumBlockType.TNT)
                        {
                            Explode(4, block.Position);
                        }
                        if(block.Type == EnumBlockType.CHEST)
                        {
                            n.Chest(World.GetChestAt(block.Position));
                        }
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
                                    if (b.Type == EnumBlockType.LAVA)
                                    {
                                        if (n.SelectedItem.Type == Material.BUCKET)
                                        {
                                            n.RemoveItem(Material.BUCKET);
                                            n.Give(Material.LAVA_BUCKET);

                                        }
                                        else { return; }
                                    }
                                    if (b.Type == EnumBlockType.WATER)
                                    {
                                        if (n.SelectedItem.Type == Material.BUCKET)
                                        {
                                            n.RemoveItem(Material.BUCKET);
                                            n.Give(Material.WATER_BUCKET);

                                        }
                                        else { return; }
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
                                            n.Give(Material.IRON_ORE);
                                        if (n.SelectedItem.Type == Material.IRON_PICKAXE)
                                            n.Give(Material.IRON_ORE);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.IRON_ORE);
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
                                            n.Give(Material.GOLD_ORE);
                                        if (n.SelectedItem.Type == Material.DIAMOND_PICKAXE)
                                            n.Give(Material.GOLD_ORE);
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

                                    if(b.Type == EnumBlockType.TNT)
                                    {
                                        n.Give(Material.TNT);
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
                        if(n.SelectedItem.Type == Material.LAVA_BUCKET)
                        {
                            if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d) return;
                                foreach (var g in players)
                                g.SendBlockChange(placeAt, EnumBlockType.LAVA);
                            World.Blocks.Add(new Block(placeAt, EnumBlockType.LAVA, false, false));
                            n.SelectedItem.Count -= 1;
                            if (n.SelectedItem.Count <= 0)
                            {
                                n.PlayerInventory.Items.Remove(n.SelectedItem);
                                n.SelectedItem = null;
                            }

                            n.UpdateInventory();
                            return;
                        }
                        if (n.SelectedItem.Type == Material.WATER_BUCKET)
                        {
                            if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d) return;
                            foreach (var g in players)
                                g.SendBlockChange(placeAt, EnumBlockType.WATER);
                            World.Blocks.Add(new Block(placeAt, EnumBlockType.WATER, false, false));
                            n.SelectedItem.Count -= 1;
                            if (n.SelectedItem.Count <= 0)
                            {
                                n.PlayerInventory.Items.Remove(n.SelectedItem);
                                n.SelectedItem = null;
                            }

                            n.UpdateInventory();
                            return;
                        }
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
                        if(type == EnumBlockType.SAPLING)
                        {
                            Block c = World.GetBlockAt(placeAt.X, placeAt.Y + 1);
                            if (c == null) return;
                            if (c.Type != EnumBlockType.GRASS_BLOCK) return;
                        }
                        foreach (var g in players)
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
                        if (n.SelectedItem.Type == Material.LAVA_BUCKET)
                        {
                            if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d) return;
                            foreach (var g in players)
                                g.SendBlockChange(placeAt, EnumBlockType.LAVA, true, false, true);
                            World.Blocks.Add(new Block(placeAt, EnumBlockType.LAVA, false, true));
                            n.SelectedItem.Count -= 1;
                            if (n.SelectedItem.Count <= 0)
                            {
                                n.PlayerInventory.Items.Remove(n.SelectedItem);
                                n.SelectedItem = null;
                            }

                            n.UpdateInventory();
                            return;
                        }
                        if (n.SelectedItem.Type == Material.WATER_BUCKET)
                        {
                            if (DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6d) return;
                            foreach (var g in players)
                                g.SendBlockChange(placeAt, EnumBlockType.WATER, true, false, true);
                            World.Blocks.Add(new Block(placeAt, EnumBlockType.WATER, false, true));
                            n.SelectedItem.Count -= 1;
                            if (n.SelectedItem.Count <= 0)
                            {
                                n.PlayerInventory.Items.Remove(n.SelectedItem);
                                n.SelectedItem = null;
                            }

                            n.UpdateInventory();
                            return;
                        }
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

                        foreach (var g in players)
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
                        foreach (var i in players)
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

        public void CrashReport(Exception ex)
        {
            string crashText = "Netcraft Crash Report" + Constants.vbCrLf + $"Server crashed at {DateTime.Now.ToString()}" + Constants.vbCrLf + $"{ex.GetType().ToString()}: {ex.Message}{Constants.vbCrLf}== STACK TRACE =={Constants.vbCrLf}{ex.InnerException.StackTrace}{Constants.vbCrLf}{Constants.vbCrLf}" + $"Exception.TargetSite: {ex.TargetSite}" + Constants.vbCrLf + $"Exception.Source: {ex.Source}";
            File.WriteAllText("./crash-reports/" + DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-") +  ".txt", crashText);
            Environment.Exit(-1);
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

        public void Explode(double radius, Point point)
        {
            List<Block> blocksToRemove = new List<Block>();
            foreach(Block b in World.Blocks)
            {
                if (b.Type == EnumBlockType.IRON_BLOCK)
                    continue;
                if (b.Type == EnumBlockType.DIAMOND_BLOCK)
                    continue;
                if (b.Type == EnumBlockType.OBSIDIAN)
                    continue;
                if (b.Type == EnumBlockType.BEDROCK)
                    continue;
                if (DistanceBetweenPoint(b.Position, point) <= radius) blocksToRemove.Add(b);
            }
            
            foreach(Block b in blocksToRemove)
            {
                foreach(NetworkPlayer p in players)
                {
                    p.PacketQueue.AddQueue($"removeblock?{b.Position.X.ToString()}?{b.Position.Y.ToString()}");
                    World.Blocks.Remove(b);
                }
            }
            foreach (NetworkPlayer p in players)
            {
                p.PacketQueue.SendQueue();
            }
            foreach (NetworkPlayer p in players)
            {
                if(DistanceBetweenPoint(Normalize(p.Position), point) <= radius)
                {
                    p.Damage(10, "взорвался(-лась)");
                }
            }
            Log($"Explosion at [{point.X},{point.Y}] with power {radius.ToString()}");

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
            return expession == null;
        }

        public void Chat(string arg0)
        {
            Send("chat?" + arg0);
        }

        public void SaveAuth()
        {
            string txt = "";
            foreach (var t in playerPasswords.Keys.Cast<string>().ToArray())
                txt = Conversions.ToString(txt + Operators.AddObject(Operators.AddObject(t + "=", playerPasswords[t]), Constants.vbCrLf));
            File.WriteAllText("./auth.txt", txt, Encoding.UTF8);
        }

        public void ClientExited(NetworkPlayer client, bool isError = false)
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

            players.Remove(client);
            Netcraft.clientList = players;
        }

        private bool Listening = false;

        private void Start()
        {
            Listning = new TcpListener(IPAddress.Any, 6575);
            if (Listening == false)
            {
                Listning.Start();
                Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
                Listening = true;
            }
            else
            {
                Listning.Stop();
            }
        }

        private void OnClose()
        {
            File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World), Encoding.UTF8);
            foreach (var c in players)
                ClientExited(c);
        }
        public void ForceCrash()
        {
            throw new NullReferenceException();
        }

        public void SaveWorld()
        {
            File.WriteAllText(Conversions.ToString(worldfile), sv.Save(World));
        }

        public void SendCommandFeedback(string a, CommandSender b)
        {
            Log($"[{b.GetName()}: {a}]");
            Send($"chat?[{b.GetName()}: {a}]");
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