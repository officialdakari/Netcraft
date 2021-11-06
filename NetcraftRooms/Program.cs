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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NCore
{
    public class Config
    {
        public static string GetValue(string var, string cfg, string ifNull = null)
        {
            cfg = cfg.Replace(Constants.vbCrLf, Constants.vbLf);
            foreach (var a in cfg.Split('\n'))
            {
                var b = a.Split('=');
                if (a.Length == 0) continue;
                if (a[0] == '#') continue;
                if ((b[0].ToLower() ?? "") == (var.ToLower() ?? ""))
                {
                    return string.Join("=", b.Skip(1).ToArray());
                }
            }

            return ifNull;
        }
    }


    public partial class NCore
    {
        public class Lang
        {

            internal Hashtable formats;
            public static Lang FromText(string t)
            {
                Lang lang = new Lang();
                lang.formats = new Hashtable(new Dictionary<string, string>());
                t = t.Replace("\r\n", "\n");
                foreach (string i in t.Split('\n'))
                {
                    lang.formats.Add(i.Split('=')[0], Config.GetValue(i.Split('=')[0], t, i.Split('=')[0]));
                }
                return lang;
            }

            public static Lang FromFile(string p)
            {
                return FromText(System.IO.File.ReadAllText(p, Encoding.UTF8));
            }

            public string get(string i)
            {
                if (!formats.ContainsKey(i)) return i;
                return formats[i].ToString().Replace("&CRLF", "\r\n").Replace("&CR", "\r");
            }

            public string get(string i, params string[] args)
            {
                if (args == null) return get(i);
                if (!formats.ContainsKey(i)) return i;
                return String.Format(formats[i].ToString(), args).Replace("&CRLF", "\r\n").Replace("&CR", "\r");
            }

        }


        private TcpListener Listning;
        internal readonly List<NetcraftPlayer> players = new List<NetcraftPlayer>();
        private NetcraftPlayer pClient;
       
        public TimeSpan keepAliveTimeout = new TimeSpan(0, 0, 0, 5, 0);
        

        public string GetApplicationRoot()
        {
            //var exePath = Path.GetDirectoryName(System.Reflection
            //                  .Assembly.GetExecutingAssembly().CodeBase);
            //Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            //var appRoot = appPathMatcher.Match(exePath).Value;
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        public void OnCrash(object sender, ThreadExceptionEventArgs e)
        {
            CrashReport(e.Exception);
        }


        private void eventhandler_a(string m)
        {
            Log(m);
            Send("chat?" + m);
        }

        private Thread loopThread;
        internal const string NETCRAFT_VERSION = "0.1.7b";
        internal const string NCORE_VERSION = "0.8";
        int hungerDecreaseDelay = 200;
        public DateTime LastTick { get; private set; }
        DateTime lastSave;

        private async void ThreadLoop()
        {

            while (true)
            {
                Thread.Sleep(100);
                //if(lastSave == null)
                //{
                //    lastSave = DateTime.Now.AddSeconds(15);
                //}
                //if(lastSave < DateTime.Now)
                //{
                //    Log("World is autosaving");
                //    SaveWorld();
                //    Log("World was autosaved");
                //    lastSave = DateTime.Now.AddSeconds(15);
                //}
                logAppendDelay -= 1;
                
                LastTick = DateTime.Now;
                
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
                    
                }


                //problem detected ---- Console.Title = $"NCore {NCORE_VERSION} (Netcraft {NETCRAFT_VERSION}) | {players.Count}/{maxPlayers.ToString()} players | {(Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split('.')[0]}MB used of {Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L}MB, {(Process.GetCurrentProcess().MaxWorkingSet.ToInt64() / 1024L - Process.GetCurrentProcess().WorkingSet64 / 1024L / 1024L).ToString().Split('.')[0]}MB free | Total Processor Time: {Process.GetCurrentProcess().TotalProcessorTime.ToString()} | Uptime: {(DateTime.Now - Process.GetCurrentProcess().StartTime).ToString().Split(".")[0]}";
            }
        }

        internal List<Color> skyClr = new List<Color>();
        internal int worldtime;
        private int stp;
        private Thread daytimeThread;


        public async Task BroadcastSkyChange(Color clr)
        {
            try
            {
                foreach (var gg in players)
                    await gg.SendSkyColorChange(clr);
            }
            catch (Exception ex)
            {
            }
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
                Thread.Sleep(100);
                try
                {
                    if (LastTick.AddSeconds(10) < DateTime.Now)
                    {
                        Console.Title = $"NCore (not responding for {(DateTime.Now - LastTick).TotalSeconds.ToString()} seconds)";
                        //Log($"SERVER IS NOT RESPONDING!", "ERROR");
                        //Log($"Watchdog detected server lag!", "ERROR");
                        //Log($"The server has not responded " + (DateTime.Now - LastTick).TotalSeconds + " seconds!");
                        if ((DateTime.Now - LastTick).TotalSeconds > 30)
                        {
                            Log($"SERVER HAS NOT RESPONDED WITH IN 30 SECONDS! STOPPING", "ERROR");
                            Environment.Exit(-1);
                        }
                    }
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

        private List<Thread> threads = new List<Thread>();
        private Thread threadWatchdog;
        private int logAppendDelay = 40;

        public static void Main(string[] args)
        {
            try
            {
                NCore app = new NCore();
                nCore = app;
                app.LastTick = DateTime.Now.AddSeconds(10);
                app.Server();
            }
            catch (Exception e)
            {
                CrashReport(e);
                Console.ReadLine();
            }

            //Console.WriteLine("Что Вы хотите сделать?\r\n1 - запустить сервер");
            //char result = Console.ReadKey().KeyChar;
            //Console.WriteLine("\b");
            //if(result == '1')
            //{
            //    try
            //    {
            //        NCore instance = new NCore();
            //        nCore = instance;
            //        instance.Server();
            //    } catch(Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }

            //}
        }

        static NCore nCore;
        public static NCore GetNCore()
        {
            return nCore;
        }

        public WorldServer __world;

        public async Task Server()
        {
            try
            {
                ThreadAdd();
                Log($"PID: {Process.GetCurrentProcess().Id} | Netcraft Version: {NETCRAFT_VERSION}");
                Console.WriteLine(@"             _                  __ _   
  _ __   ___| |_ ___ _ __ __ _ / _| |_ 
 | '_ \ / _ \ __/ __| '__/ _` | |_| __|
 | | | |  __/ || (__| | | (_| |  _| |_ 
 |_| |_|\___|\__\___|_|  \__,_|_|  \__|
                                       ");
                if (Thread.CurrentThread.ManagedThreadId != 1)
                {
                    CrashReport(new Exception("The main thread ID was not 1"));
                }

                //lang = Lang.FromFile("./lang.txt");

                Log("Pushconnect starting");

                loopThread = new Thread(ThreadLoop);
                loopThread.Name = "Loop";
                loopThread.Start();
                threadWatchdog = new Thread(watchdogThreadLoop);
                threadWatchdog.Start();
                __world = JsonConvert.DeserializeObject<WorldServer>(File.ReadAllText("./world.json"));

                Thread.CurrentThread.Name = "Main";
                
                LoadBanlist();
                playerPasswords = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("./auth.txt"));

                Start();
                LoadRooms();
                Log("Server started");
                while (true)
                {
                    string m = Console.ReadLine();
                    var args = m.Split(' ').Skip(1).ToArray();
                    string cmd = m.Split(' ')[0];
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        string lastLog = "";
        public void writeline(string str)
        {
            ThreadAdd();
            Console.WriteLine(str);
            lastLog = str;
        }

        public delegate void xSend(string str, string except, bool readyClientsOnly);

        public async Task Send(string str, string except = null, bool readyClientsOnly = false)
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
                            await players[x].Send(str);
                    }
                    else
                    {
                        await players[x].Send(str);
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

        public async Task BroadcastChatTranslation(string translationKey, string[] format, string except = null, bool readyClientsOnly = false)
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
                            await players[x].Chat(players[x].lang.get(translationKey, format));
                    }
                    else
                    {
                        await players[x].Chat(players[x].lang.get(translationKey, format));
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

        

        public void LoadBanlist()
        {
            
        }

        public async void AcceptClient(IAsyncResult ar)
        {
            ThreadAdd();
            try
            {
                if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = "Network Join";
                pClient = new NetcraftPlayer(Listning.EndAcceptTcpClient(ar));
                pClient.a += MessageReceived;
                pClient.b += (_) => NCore.GetNCore().handleDisconnection(pClient, false);
                players.Add(pClient);
                Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
                Log(pClient.GetIp() + " connecting");
            }
            catch (Exception ex)
            {
            }
        }

        private string toAppend = "";

        internal void Log(string arg0, string arg1 = "INFO")
        {
            ThreadAdd();
            arg0 = arg0.Replace("&0", "").Replace("&1", "").Replace("&2", "").Replace("&3", "").Replace("&4", "").Replace("&5", "").Replace("&r", "");
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

            writeline($"[{DateTime.Now} {arg1}]: {arg0}");
            toAppend += $"[{DateTime.Now} {arg1}] [Thread {Thread.CurrentThread.ManagedThreadId}]: {arg0}" + Constants.vbCrLf;
            Console.ForegroundColor = ConsoleColor.White;
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
            Log("Pushconnect stopping");
            

            Kickall("Pushconnect stopped");
            Listning.Stop();
            Process.GetCurrentProcess().Kill();
        }

        private bool isIllegalValue(object v, params object[] vs)
        {
            if (!vs.Contains(v)) return true;
            return false;
        }

        public async Task Kickall(string m)
        {
            for (int i = 0; i < players.Count; i++)
            {
                try
                {
                    players[i].Kick(m);
                }
                catch (NullReferenceException)
                {

                }
            }
        }
        const string RCON_PASSWORD = "12344321";
        private Dictionary<string, string> playerPasswords = new Dictionary<string, string>();
        private StringCollection loggedIn = new StringCollection();
        const bool ENABLE_STATISTICS_EXPERIMENTAL = true;

        internal async void __forward(string msg, NetcraftPlayer n)
        {
                if (msg.StartsWith("msgkick?"))
                {
                    string kick = msg.Substring(8);
                    await n.DisconnectClient();
                    await n.Send($"msgwarn?You were kicked from {n.Server}: {kick}");
                    n.Server = "";
                    n.OnMessageReceived -= __forward;
                    return;
                }
                await n.Send(msg);
        }
        Dictionary<string, object[]> banned = new Dictionary<string, object[]>();
        public async void MessageReceived(string str, NetcraftPlayer n)
        {
            bool forward = true;
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "Network Packet";
            ThreadAdd();
            if (str == "")
            {
                if (n.IsLoaded) return;
                n.Disconnect();
                Log(n.GetIp() + " disconnected for bad packet", "WARNING");
                players.Remove(n);
                return;

            }
            if (str.Length > 5000)
            {
                n.Disconnect();
                Log(n.GetIp() + " disconnected for too long packet", "WARNING");
                players.Remove(n);
                return;
            }
            try
            {
                var a = str.Split('?');
                if (str == "ping")
                {
                    Log($"Pinged from {n.GetIp()}");
                   
                        await n.Send("name?Pushconnect");
                        await Task.Delay(10);
                        await n.Send("motd?Netcraft MP Main Pushconnect");
                        await Task.Delay(10);
                        await n.Send("players?Unknown");
                  

                    n.Disconnect();
                    players.Remove(n);
                    return;
                }
                Lang lang = n.lang;
                if (str.Length < 1)
                {
                    n.Kick("Too short packet.");
                    return;
                }
                
                if (a[0] == "setname")
                {
                    if (!string.IsNullOrEmpty(n.Username))
                    {
                        return;
                    }
                    

                    foreach (var netplayer in players)
                    {
                        if (netplayer.UUID == n.UUID) continue;
                        if (netplayer.Username == a[1] && netplayer.IsAuthorized)
                        {
                            await n.Kick("This player is online");
                            return;
                        }
                    }



                    n.Username = a[1];
                    n.Language = a[2];
                    n.Sprite = a[3];
                    n.IsLoaded = true;
                    if (!Regex.Match(n.Username, "^[a-zA-Z0-9_]*").Success)
                    {
                        await n.Kick("Invalid nickname");
                        return;
                    }

                    forward = false;
                    Log(a[1] + " connected");

                    // n.Send("blockchange?500?50?Red")
                }

                if (string.IsNullOrEmpty(n.Username))
                {
                    return;
                }
                if (!loggedIn.Contains(n.Username) && (enableAuth == 1))
                {
                    if (a[0] == "chat")
                    {
                        if (!Regex.Match(a[1], "^[a-zA-Z0-9_]*").Success)
                        {
                            await n.SendMessage("Invalid password.");
                        }

                        if (!playerPasswords.Keys.Cast<string>().ToArray().Contains(n.Username))
                        {
                            playerPasswords.Add(n.Username, a[1]);
                            Log($"{n.Username} registered from {n.GetIp()}!");
                        }

                        if (playerPasswords[n.Username] == a[1])
                        {
                            await n.SendMessage("Успешный вход. Введите :help для помощи.");
                            await n.DisconnectClient();
                            await n.Teleport(0, 0);
                            loggedIn.Add(n.Username);
                            Log($"{n.Username} logged in!");
                            n.IsAuthorized = true;
                            foreach (var netplayer in players)
                            {
                                if (netplayer.UUID == n.UUID) continue;
                                if (netplayer.Username == a[1])
                                {
                                    if (netplayer.IsAuthorized == false)
                                    {
                                        await netplayer.Kick("Авторизация с другого устройства");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log($"{n.Username} used wrong password", "ERROR");
                            await n.SendMessage("Неверный пароль");
                        }
                    }

                    return;
                }
                if(a[0] == "world")
                {
                    await n.PacketQueue.AddQueue("chat?Введите пароль в чат (для регистрации / входа в аккаунт)");
                    
                }
                try
                {
                    if (!n.IsAuthorized) return;
                    if(a[0] == "menusel")
                    {
                        n.OpenedMenu.Functions[a[1]]();
                    }
                    if (a[0] == "chat")
                    {
                        string message = string.Join("?", a.Skip(1).ToArray());

                        if (message[0] == ':')
                        {
                            string m = message.Substring(1);
                            string[] _arr = m.Split(' ');
                            string[] args = _arr.Skip(1).ToArray();
                            string cmd = _arr[0];
                            if (cmd == "help")
                            {
                                Menu menu = new Menu();
                                menu.Functions.Add("Создать комнату", () =>
                                {
                                    if (Directory.Exists("./rooms/" + n.Username))
                                    {
                                        n.Message("У Вас уже есть комната.", 3);
                                        return;
                                    }
                                    Room r = new Room();
                                    r.owner = n.Username;
                                    r.Create();
                                    _rooms.Add(r);
                                });
                                menu.Functions.Add("Открыть комнату", () =>
                                {
                                    if (!Directory.Exists("./rooms/" + n.Username))
                                    {
                                        n.Message("Сначала создайте комнату.", 3);
                                        return;
                                    }
                                    Room r = _rooms.Where((_r) => _r.owner == n.Username).ToArray()[0];
                                    r.Start();
                                });
                                menu.Functions.Add("Закрыть комнату", () =>
                                {
                                    if (!Directory.Exists("./rooms/" + n.Username))
                                    {
                                        n.Message("Сначала создайте комнату.", 3);
                                        return;
                                    }
                                    Room r = _rooms.Where((_r) => _r.owner == n.Username).ToArray()[0];
                                    r.Stop();
                                });
                                menu.Functions.Add("Войти в комнату", () =>
                                {
                                    Dictionary<string, Action> _a = new Dictionary<string, Action>();
                                    foreach(Room _r in _rooms)
                                    {
                                        _a.Add(_r.owner, () =>
                                        {
                                            if(_r._isStarted)
                                            {
                                                string nickname = _r.owner;
                                                
                                                if (!Directory.Exists("./rooms/" + nickname))
                                                {
                                                    n.Message("Такой комнаты не существует", 3);
                                                    return;
                                                }
                                                Room r = _rooms.Where((___r) => ___r.owner == nickname).ToArray()[0];
                                                if (!r._isStarted)
                                                {
                                                    n.Message("Комната закрыта", 3);
                                                    return;
                                                }
                                                if(banned.ContainsKey(n.Username))
                                                {
                                                    object[] data = banned[n.Username];
                                                    string server = data[0].ToString();
                                                    DateTime until = (DateTime)data[1];
                                                    if(DateTime.Now < until)
                                                    {
                                                        n.Message("Вы были исключены администратором, повторное подключение невозможно.", 3);
                                                        return;
                                                    } else
                                                    {
                                                    }
                                                }
                                                for(int x = 0; x < 70; x++)
                                                {
                                                    for(int y = 0; y < 20; y++)
                                                    {
                                                        n.PacketQueue.AddQueue($"removeblock?{x.ToString()}?{y.ToString()}");
                                                    }
                                                }
                                                n.PacketQueue.SendQueue();
                                                n.Connect("127.0.0.1", r.port);
                                                n.Server = nickname;
                                                n.OnMessageReceived += __forward;
                                                n.SendPacket($"setname?{n.Username}?{n.Language}?{n.Sprite}");
                                                Thread.Sleep(1000);
                                                n.SendPacket($"world");
                                            }
                                        });
                                    }
                                    _a.Add("Закрыть", () =>
                                    {
                                        n.OpenedMenu.Close();
                                    });
                                    menu.UpdateMenu(_a);
                                });
                                menu.Functions.Add("Игроки", () =>
                                {
                                    Dictionary<string, Action> _a = new Dictionary<string, Action>();
                                    NetcraftPlayer[] _players = players.Where((player) => player.Server == n.Username).ToArray();
                                    if (!Directory.Exists("./rooms/" + n.Username))
                                    {
                                        n.Message("Сначала создайте комнату.", 3);
                                        return;
                                    }
                                    Room r = _rooms.Where((_r) => _r.owner == n.Username).ToArray()[0];
                                    if(r._isStarted)
                                    {
                                        foreach(NetcraftPlayer p in _players)
                                        {
                                            _a.Add(p.Username, () =>
                                            {
                                                Dictionary<string, Action> b = new Dictionary<string, Action>();
                                                b.Add("Исключить", () =>
                                                {
                                                    p.Chat("&2[NetMP]&r Вы исключены из игры.");
                                                    _players.All((__p) =>
                                                    {
                                                        __p.Chat($"&2[NetMP] &1Игрок {p.Username} был исключен администратором");
                                                        return true;
                                                    });
                                                    p.DisconnectClient();
                                                    n.OpenedMenu.Close();
                                                });
                                                b.Add("Заблокировать", () =>
                                                {
                                                    p.Chat("&2[NetMP]&r Администратор комнаты заблокировал Вас.");
                                                    _players.All((__p) =>
                                                    {
                                                        __p.Chat($"&2[NetMP] &1Игрок {p.Username} был исключен администратором");
                                                        return true;
                                                    });
                                                    banned.Add(n.Username, new object[] {n.Username, DateTime.Now.AddMinutes(30) });
                                                    p.DisconnectClient();
                                                    n.OpenedMenu.Close();
                                                });
                                                b.Add("Выход от имени этого игрока", () =>
                                                {
                                                    p.SendPacket("chat?пока");
                                                    
                                                    p.DisconnectClient();
                                                    n.OpenedMenu.Close();
                                                });
                                                b.Add("Закрыть", () =>
                                                {
                                                    n.OpenedMenu.Close();
                                                });
                                                menu.UpdateMenu(b);
                                            });
                                        }
                                    }
                                    _a.Add("Закрыть", () =>
                                    {
                                        n.OpenedMenu.Close();
                                    });
                                    menu.UpdateMenu(_a);
                                });
                                menu.Functions.Add("Закрыть", () =>
                                {
                                    n.OpenedMenu.Close();
                                });
                                    menu.Functions.Add("Спавн бедрока (чтобы не падать)", () =>
                                {
                                    n.SendBlockChange(0, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                    n.SendBlockChange(1, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                    n.SendBlockChange(2, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                    n.PacketQueue.SendQueue();
                                });
                                await menu.Open(n);
                                //await n.Chat($"Мультиплеер Netcraft - Список команд\n:help > Показать этот список\n:c <сообщение> > чат по всем серверам\n:exit > Выход\n:\n:createroom > Создать сервер\n:startroom > Запустить сервер\n:joinroom <name> > Войти в сервер\n:stoproom > Остановить свой сервер");
                            } else if(cmd == "c")
                            {
                                string msg = string.Join(' ', args);
                                foreach(NetcraftPlayer p in players)
                                {
                                    await p.Chat($"[NetMP] {n.Username}: {msg}");
                                }
                            }
                            else if (cmd == "joinroom")
                            {
                                
                            }else if (cmd=="exit")
                            {
                                await n.SendPacket("chat~Этот игрок покинул сервер.");
                                await n.DisconnectClient();
                            } else if(cmd == "createroom")
                            {
                            } else if(cmd == "startroom")
                            {
                            } else if(cmd == "stoproom")
                            {
                            } else if(cmd == "spb")
                            {

                                await n.SendBlockChange(0, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                await n.SendBlockChange(1, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                await n.SendBlockChange(2, 5, EnumBlockType.BEDROCK, packetQueue: true);
                                await n.PacketQueue.SendQueue();
                            }
                            forward = false;
                        }
                        else
                        {
                            // Redirect packet
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
                if (forward)
                   await n.SendPacket(string.Join('?', a));
            }
            catch (Exception globalEx)
            {
                Log($"Error while processing {n.Username}'s packet:{Constants.vbCrLf}{globalEx.ToString()}", "WARNING");
                await n.Kick("Internal server error occured.");
                return;
            }
        }

        public static void CrashReport(Exception ex)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 120; i++) Console.WriteLine(new string(' ', 1000));
            Console.Clear();
            string crashText = "Netcraft Crash Report" + Constants.vbCrLf + $"Server crashed at {DateTime.Now.ToString()}" + Constants.vbCrLf + $"{ex.ToString()}";
            File.WriteAllText("./crash-reports/" + DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-") + ".txt", crashText);

            Console.WriteLine(crashText + "\n\nPress any key to exit...");
            Console.ReadKey();
            Console.ResetColor();
            Console.Clear();
            for (int i = 0; i < 120; i++) Console.WriteLine(new string(' ', 1000));
            Console.Clear();
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
        List<Room> _rooms = new List<Room>();
        public void LoadRooms()
        {
            foreach(string d in Directory.GetDirectories("./rooms"))
            {
                if (d.Contains("_EXAMPLE_")) continue;
                Room r = new Room();
                r._path = d;
                r.owner = d.Split(Path.DirectorySeparatorChar).Last();
                r.port = int.Parse(File.ReadAllText(d + "/port"));
                _rooms.Add(r);
            }
        }
        public class Room
        {
            public Process _server;
            public bool _isStarted = false;
            public string _path;
            public string owner;
            public int port;
            public void Start()
            {
                if (_isStarted) return;
                _server = new Process();
                _server.StartInfo.FileName = "cmd.exe";
                _server.StartInfo.UseShellExecute = true;
                _server.StartInfo.Arguments = "/c NetcraftCore.exe";
                _server.StartInfo.WorkingDirectory = _path;

                _server.Start();
                _isStarted = true;
            }
            public void Stop()
            {
                if (_server == null) return;
                _server.Kill();
                _server = null;

                _isStarted = false;
            }
            public void Create()
            {
                DirectoryCopy("./rooms/_EXAMPLE_", "./rooms/" + owner);
                _path = "./rooms/" + owner;
                int i = new Random().Next(9000, 20000);
                File.WriteAllText("./rooms/" + owner + "/server.properties", File.ReadAllText("./rooms/" + owner + "/server.properties").Replace("[PORT]", i.ToString()));
                File.WriteAllText(_path + "/port", i.ToString());
                port = i;
            }
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
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

        public async Task Chat(string arg0)
        {
            await Send("chat?" + arg0);
        }

        public void SaveAuth()
        {
            string txt = "";
            
            File.WriteAllText("./auth.txt", JsonConvert.SerializeObject(playerPasswords));
        }
        string handlingDisconnection = null;
        public async void handleDisconnection(NetcraftPlayer client, bool isError = false)
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "Network Leave";
            ThreadAdd();
            if (handlingDisconnection == client.UUID)
            {
                Log($"handleDisconnection({client.ToString().TrimEnd(';')}, {isError.ToString().ToLower()}) called twice", "WARNING");
                return;
            }
            if (handlingDisconnection == null) handlingDisconnection = client.UUID;
            if (client.IsLoaded)
            {
                if (!isError)
                {
                    Log(client.Username + " disconnected");
                }
                else
                {
                    client.Kick("An internal server error occured");
                    Log(client.Username + " left the game due to an error.");
                }
                await client.SendPacket("chat~Этот игрок покинул сервер. \"Netcraft MP\"");
                await client.DisconnectClient();


                if (loggedIn.Contains(client.Username))
                {
                    loggedIn.Remove(client.Username);
                }
            }

            players.Remove(client);
        }

        private bool Listening = false;
        private int enableAuth = 1;

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
        public void ForceCrash()
        {
            throw new NullReferenceException();
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