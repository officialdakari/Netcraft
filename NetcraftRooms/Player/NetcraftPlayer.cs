using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using global::System.Drawing;
using global::System.IO;
using global::System.Net;
using global::System.Net.Sockets;
using global::System.Security.Cryptography;
using global::System.Text;
using global::System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace NCore
{
    public class NetcraftPlayer
    {
        public NetcraftPlayer(TcpClient forClient)
        {
            field_01931 = Position;
            d = forClient;
            d.GetStream().BeginRead(new byte[] { 0 }, 0, 0, (_) => e(), null);
            var argarg0 = this;
            PacketQueue = new SendQueueType(ref argarg0);
            ip = ((IPEndPoint)d.Client.RemoteEndPoint).Address.ToString();
            PlayerRectangle = new Rectangle(Position, new Size(37, 72));
        }

        public string Username { get; internal set; } = null;
        public string Sprite { get; internal set; } = null;
        public string UUID { get; internal set; } = Guid.NewGuid().ToString();
        public Point Position { get; set; } = new Point(0, 0);
        public Inventory PlayerInventory { get; internal set; }
        public ItemStack SelectedItem { get; internal set; }
        public int Health { get; set; } = 100;
        public int Hunger { get; set; } = 20;
        public int Saturation { get; set; } = 5;
        public bool IsAdmin { get; set; } = false;
        public SendQueueType PacketQueue { get; internal set; }
        public bool IsSpectator { get; internal set; } = false;
        public bool DisableMovementCheck { get; set; } = false;
        public bool IsLoaded { get; internal set; } = false;
        public Rectangle PlayerRectangle { get; internal set; }
        public bool NoClip { get; private set; } = false;
        public int FallDistance { get; set; } = 0;
        public int MovedInAir { get; set; } = 0;
        public bool IsAuthorized { get; set; } = false;
        public bool UnlimitedReach { get; set; } = false;
        public DateTime MessagePacketTimeout { get; internal set; }
        public int MessageTimeoutWarnings { get; internal set; } = 0;
        public int AntiFlyWarnings { get; set; } = 0;
        internal NCore.Lang lang;
        public DateTime LastKeepAlivePacket { get; internal set; }
        public bool IsRconClient { get; internal set; } = false;
        private Action<bool> _question = null;

        public string GetIp()
        {
            return ip;
        }

        public string Yes()
        {
            if (_question == null)
            {
                return "There is no question to answer!";
            }
            _question(true);
            _question = null;
            return null;
        }

        public string No()
        {
            if (_question == null)
            {
                return "There is no question to answer!";
            }
            _question(false);
            _question = null;
            return null;
        }

        public void Ask(string message, Action<bool> action)
        {
            _question = action;
            Chat(message);
        }

        public override string ToString()
        {
            string t = "";
            t += $"username='{Username}';";
            t += $"uuid='{UUID}';";
            t += $"ip='{GetIp()}';";
            t += $"position={Position.X.ToString()},{Position.Y.ToString()};";
            t += $"selecteditem='{(SelectedItem == null ? "NULL" : SelectedItem.ToString())}';";
            t += $"world='NCore.WorldServer';";
            t += $"isSpectator='{IsSpectator.ToString()}';";
            t += $"nomovecheck='{DisableMovementCheck.ToString()}';";
            t += $"isLoaded='{IsLoaded.ToString()}';";
            t += $"playerRectangle='{PlayerRectangle.ToString()}';";
            t += $"noclip='{NoClip.ToString()}';";
            t += $"falldistance='{FallDistance.ToString()}';";
            t += $"antiflywarnings='{AntiFlyWarnings.ToString()}'";
            return t;
        }

        public TcpClient GetConnection()
        {
            return d;
        }

        public NetworkStream GetStream()
        {
            return d.GetStream();
        }

        private string ip;
        private Point field_01931;

        public async Task Spectator()
        {
            IsSpectator = true;
            field_01931 = Position;
            await NCore.GetNCore().Send("removeplayer?" + Username, Username);
            await SetNoClip(true);
        }

        public async Task Survival()
        {
            IsSpectator = false;
            Teleport(field_01931.X, field_01931.Y);
            await NCore.GetNCore().Send("addplayer?" + Username, Username);
            await SetNoClip(false);
        }

        
        public async Task SetNoClip(bool arg0)
        {
            if (arg0)
            {
                await Send("noclip");
                NoClip = true;
            }
            else
            {
                await Send("clip");
                NoClip = false;
            }
        }
        // getMessage
        public event aEventHandler a;

        public delegate void aEventHandler(string str, NetcraftPlayer n);
        // clientLogout
        public event bEventHandler b;

        public delegate void bEventHandler(NetcraftPlayer client);
        // SendMessage
        private StreamWriter c;
        // ListClient
        private TcpClient d;

        

        public async Task DoWarning(string n)
        {
            await Send("dowarn?" + n);
        }

        public async Task SendLog(string t)
        {
            await Send("evalresult?" + t.Replace("\n", "\r").Replace("\r\r", "\r"));
        }

        

        public async Task SendSkyColorChange(Color color)
        {
            await Send($"sky?{color.Name}");
        }

        public async Task Teleport(int x, int y)
        {
            Position = new Point(x, y);
            await Send("teleport?" + x.ToString() + "?" + y.ToString());
        }

        public async Task EditChatLine(int line, string message)
        {
            await Send("setchatline?" + line.ToString() + "?" + message);
        }

        public async Task Chat(string arg0)
        {
            await Send("chat?" + arg0.Replace("\n", "\r"));
        }


        #region stats
        public Dictionary<string, string> Stats { get; internal set; } = new Dictionary<string, string>();
        public void SetStat(string a, string b)
        {
            if (!Stats.ContainsKey(a))
            {
                Stats.Add(a, b);
                return;
            }
            Stats[a] = b;
        }

        public void SetStatInt(string a, int b)
        {
            if (!Stats.ContainsKey(a))
            {
                Stats.Add(a, b.ToString());
                return;
            }
            Stats[a] = b.ToString();
        }

        public string GetStat(string a) => Stats[a];
        public int GetStatInt(string a) => int.Parse(Stats[a]);

        public void IncrementStatInt(string a)
        {
            if (!Stats.ContainsKey(a))
            {
                Stats.Add(a, "0");
                return;
            }
            Stats[a] = (int.Parse(Stats[a]) + 1).ToString();
        }

        public void DecrementStatInt(string a)
        {
            if (!Stats.ContainsKey(a))
            {
                Stats.Add(a, "0");
                return;
            }
            if (int.Parse(Stats[a]) < 1) return;
            Stats[a] = (int.Parse(Stats[a]) - 1).ToString();
        }

        public async Task UpdateStats()
        {
            await Send("updatestats?" + JsonConvert.SerializeObject(Stats));
        }
        #endregion

        public async Task SendBlockChange(int x, int y, EnumBlockType m, bool nonsolid = false, bool packetQueue = false, bool isBackground = false)
        {
            var color = default(Color);
            string t = "";
            if (m == EnumBlockType.BEDROCK)
                t = "bedrock";
            if (m == EnumBlockType.STONE)
                t = "stone";
            if (m == EnumBlockType.DIRT)
                t = "dirt";
            if (m == EnumBlockType.PLANKS)
                t = "planks";
            if (m == EnumBlockType.WOOD)
                t = "wood";
            if (m == EnumBlockType.GRASS_BLOCK)
                t = "grass_block";
            if (m == EnumBlockType.SNOWY_GRASS_BLOCK)
                t = "snowygrass";
            if (m == EnumBlockType.NETCRAFT_BLOCK)
                t = "netcraft_block";
            if (m == EnumBlockType.SNOWY_NETCRAFT_BLOCK)
                t = "netcraft_block_snowy";
            if (m == EnumBlockType.COBBLESTONE)
                t = "cobble";
            if (m == EnumBlockType.LEAVES)
                t = "leaves";
            if (m == EnumBlockType.COAL_ORE)
                t = "coal_ore";
            if (m == EnumBlockType.IRON_ORE)
                t = "iron_ore";
            if (m == EnumBlockType.DIAMOND_ORE)
                t = "diamond_ore";
            if (m == EnumBlockType.OBSIDIAN)
                t = "obsidian";
            if (m == EnumBlockType.SAND)
                t = "sand";
            if (m == EnumBlockType.END_STONE)
                t = "endstone";
            if (m == EnumBlockType.GLASS)
                t = "glass";
            if (m == EnumBlockType.GRAVEL)
                t = "gravel";
            if (m == EnumBlockType.FIRE)
            {
                t = "fire";
                nonsolid = true;
            }
            if (m == EnumBlockType.GOLD_ORE)
                t = "gold_ore";
            if (m == EnumBlockType.FURNACE)
                t = "furnace";
            if (m == EnumBlockType.IRON_BLOCK)
                t = "iron_block";
            if (m == EnumBlockType.DIAMOND_BLOCK)
                t = "diamond_block";
            if (m == EnumBlockType.GOLD_BLOCK)
                t = "gold_block";
            if (m == EnumBlockType.TNT)
                t = "tnt";
            if (m == EnumBlockType.SEEDS)
            {
                t = "wheat0";
                nonsolid = true;
            }
            if (m == EnumBlockType.WHEAT)
            {
                t = "wheat7";
                nonsolid = true;
            }
            if (m == EnumBlockType.CHEST)
                t = "chest";
            if (m == EnumBlockType.SAPLING)
            {
                t = "sapling";
                nonsolid = true;
            }
            if (m == EnumBlockType.WATER)
            {
                t = "water";
                nonsolid = true;
            }
            if (m == EnumBlockType.LAVA)
            {
                t = "lava";
                nonsolid = true;
            }
            if (m == EnumBlockType.RED_FLOWER)
            {
                t = "poppy";
                nonsolid = true;
            }
            if (m == EnumBlockType.YELLOW_FLOWER)
            {
                t = "dandelion";
                nonsolid = true;
            }

            if (!packetQueue)
            {
                await Send($"blockchange?{x}?{y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
            else
            {
                await PacketQueue.AddQueue($"blockchange?{x}?{y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
        }

        public async Task SendBlockChange(Point x, EnumBlockType m, bool nonsolid = false, bool packetQueue = false, bool isBackground = false)
        {
            var color = default(Color);
            string t = "";
            if (m == EnumBlockType.BEDROCK)
                t = "bedrock";
            if (m == EnumBlockType.STONE)
                t = "stone";
            if (m == EnumBlockType.DIRT)
                t = "dirt";
            if (m == EnumBlockType.PLANKS)
                t = "planks";
            if (m == EnumBlockType.WOOD)
                t = "wood";
            if (m == EnumBlockType.GRASS_BLOCK)
                t = "grass_block";
            if (m == EnumBlockType.SNOWY_GRASS_BLOCK)
                t = "snowygrass";
            if (m == EnumBlockType.COBBLESTONE)
                t = "cobble";
            if (m == EnumBlockType.LEAVES)
                t = "leaves";
            if (m == EnumBlockType.NETCRAFT_BLOCK)
                t = "netcraft_block";
            if (m == EnumBlockType.SNOWY_NETCRAFT_BLOCK)
                t = "netcraft_block_snowy";
            if (m == EnumBlockType.COAL_ORE)
                t = "coal_ore";
            if (m == EnumBlockType.IRON_ORE)
                t = "iron_ore";
            if (m == EnumBlockType.END_STONE)
                t = "endstone";
            if (m == EnumBlockType.GRAVEL)
                t = "gravel";
            if (m == EnumBlockType.DIAMOND_ORE)
                t = "diamond_ore";
            if (m == EnumBlockType.OBSIDIAN)
                t = "obsidian";
            if (m == EnumBlockType.SAND)
                t = "sand";
            if (m == EnumBlockType.GLASS)
                t = "glass";
            if (m == EnumBlockType.SEEDS)
            {
                t = "wheat0";
                nonsolid = true;
            }
            if (m == EnumBlockType.WHEAT)
            {
                t = "wheat7";
                nonsolid = true;
            }
            if (m == EnumBlockType.FIRE)
            {
                t = "fire";
                nonsolid = true;
            }
            if (m == EnumBlockType.GOLD_ORE)
                t = "gold_ore";
            if (m == EnumBlockType.FURNACE)
                t = "furnace";
            if (m == EnumBlockType.IRON_BLOCK)
                t = "iron_block";
            if (m == EnumBlockType.DIAMOND_BLOCK)
                t = "diamond_block";
            if (m == EnumBlockType.GOLD_BLOCK)
                t = "gold_block";
            if (m == EnumBlockType.TNT)
                t = "tnt";
            if (m == EnumBlockType.CHEST)
                t = "chest";
            if (m == EnumBlockType.SAPLING)
            {
                t = "sapling";
                nonsolid = true;
            }
            if (m == EnumBlockType.WATER)
            {
                t = "water";
                nonsolid = true;
            }
            if (m == EnumBlockType.LAVA)
            {
                t = "lava";
                nonsolid = true;
            }
            if (m == EnumBlockType.RED_FLOWER)
            {
                t = "poppy";
                nonsolid = true;
            }
            if (m == EnumBlockType.YELLOW_FLOWER)
            {
                t = "dandelion";
                nonsolid = true;
            }

            if (!packetQueue)
            {
                await Send($"blockchange?{x.X}?{x.Y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
            else
            {
                await PacketQueue.AddQueue($"blockchange?{x.X}?{x.Y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
        }


        public async Task UpdateInventory()
        {
            await Send("clearinventory");
            await Task.Delay(100);
            foreach (var i in PlayerInventory.Items)
                await PacketQueue.AddQueue("additem?" + i.Type.ToString() + " x " + i.Count.ToString());
            await PacketQueue.SendQueue();
        }

        

        public void Disconnect()
        {
            d.Client.Close();
            d = null;
        }


        /// <summary>
        /// Отправить игроку сообщение в окне.
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="type">тип сообщения. 0 - информация, 1 - предупреждение, 2 - ошибка.</param>
        public async Task Message(string text, int type)
        {
            switch(type)
            {
                case 0:
                    await Send("msg?" + text);
                    break;
                case 1:
                    await Send("msgwarn?" + text);
                    break;
                case 2:
                    await Send("msgerror?" + text);
                    break;
            }
        }


        private async Task e()
        {
            PlayerRectangle = new Rectangle(Position, new Size(37, 72));
            try
            {
                string data = Encode.d(await new StreamReader(d.GetStream()).ReadLineAsync());
               
                    a?.Invoke(data, this);
            }
            catch (SocketException ex)
            {
                b?.Invoke(this);
                return;
            }
            catch (IOException ex)
            {
                b?.Invoke(this);
                return;
            }
            catch (ObjectDisposedException ex)
            {
                NCore.GetNCore().LogError(ex);
            }
            catch (ArgumentNullException ex)
            {
                NCore.GetNCore().LogError(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                NCore.GetNCore().LogError(ex);
            }
            catch (ArgumentException ex)
            {
                NCore.GetNCore().LogError(ex);
            }
            catch (OutOfMemoryException ex)
            {
                NCore.GetNCore().LogError(ex);
            }
            catch (NullReferenceException ex)
            {
                if(d == null)
                    b?.Invoke(this);

                NCore.GetNCore().LogError(ex);
            }
            catch (InvalidOperationException ex)
            {
                b?.Invoke(this);
                return;
            }

            try
            {
                d.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback((_) => e()), null);
            }
            catch (Exception ex)
            {
                b?.Invoke(this);
            }
        }

        public async Task Send(string Messsage)
        {
            try
            {
                string data = Messsage;
                data = Encode.e(data);
                c = new StreamWriter(d.GetStream());
                await c.WriteLineAsync(data);
                await c.FlushAsync();
            }
            catch (Exception ex)
            {
            }
        }
        public string Server = "";
        public async Task SendMessage(string m) => await Chat(m);

        public async Task Kick(string kickMessage = "You have been kicked out from server.", bool bcKick = false)
        {
           

            if(IsLoaded) NCore.GetNCore().Log($"{Username} disconnected with: '{kickMessage}'");
            await Send("msgkick?" + kickMessage);
            b?.Invoke(this);
            if (d != null && d.Client.Connected) Disconnect();
        }

        private TcpClient client;
        private StreamWriter sWriter;
        public bool connected { get; private set; } = false;
        public string Language { get; internal set; }

        public delegate void OnMessageReceivedEventHandler(string msg, NetcraftPlayer n);
        public event OnMessageReceivedEventHandler OnMessageReceived;
        internal string toNotice = null;
        internal int toNoticeType = 2;
        public delegate void _xUpdate(string str);

        private async void Read(IAsyncResult ar)
        {
            try
            {
                var x = Encode.d(await new StreamReader(client.GetStream()).ReadLineAsync());

                OnMessageReceived?.Invoke(x, this);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task handlePackets(string[] x)
        {
            foreach (var a in x)
            {
                string data = a.Replace("\r", "\r\n");
                bool cancel = false;
                OnMessageReceived?.Invoke(data, this);
                if (cancel) return;
            }
        }

        public async Task SendPacket(string str)
        {
            try
            {
                string data = Encode.e(str);
                bool cancel = false;
                if (cancel) return;
                sWriter = new StreamWriter(client.GetStream());
                await sWriter.WriteLineAsync(data);
                await sWriter.FlushAsync();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                
            }
        }
        /// <summary>
        /// Пытается подключиться к указанному серверу по указанному порту.
        /// </summary>
        /// <param name="ip">IP-адрес сервера</param>
        /// <param name="port">Порт сервера</param>

        public async Task Connect(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ip, port);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback(Read), null);
                connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                connected = false;
                
            }
        }

        public async Task DisconnectClient()
        {
            for (int x = 0; x < 48; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    await PacketQueue.AddQueue($"removeblock?{x}?{y}");
                }
            }
            Thread.Sleep(1000);
            await PacketQueue.SendQueue();
            foreach(Block b in NCore.GetNCore().__world.Blocks)
            {
                await SendBlockChange(b.Position, b.Type, false, true, b.IsBackground);
            }
            Thread.Sleep(1000);
            await PacketQueue.SendQueue();
            if (client == null) return;
            OnMessageReceived -= NCore.GetNCore().__forward;
            connected = false;
            client.Client.Close();
            client = null;
        }

        public Menu OpenedMenu;
    }

    public class Menu
    {
        public Dictionary<string, Action> Functions = new Dictionary<string, Action>();
        public NetcraftPlayer User;
        public async Task Open(NetcraftPlayer p)
        {
            await p.Send("openmenu?" + string.Join("?", Functions.Keys.Cast<string>()));
            User = p;
            p.OpenedMenu = this;
        }

        public async Task Close()
        {
            await User.Send("closemenu");
            User = null;
        }

        public async Task UpdateMenu(Dictionary<string, Action> functions)
        {
            Functions = functions;
            await User.Send("rewritemenu?" + string.Join("?", Functions.Keys.Cast<string>()));
        }
    }

    /* TODO ERROR: Skipped WarningDirectiveTrivia */
    internal class Encode
    {
        protected static byte[] a = new byte[] { 62, 59, 25, 19, 37 };
        protected static readonly byte[] b = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        protected const string c = "YmFuIHRlYmUgZXNsaSB1em5hbCBldG90IGtvZA==";

        public static string d(string stringToDecrypt)
        {
            try
            {
                var inputByteArray = new byte[stringToDecrypt.Length + 1];
                a = Encoding.UTF8.GetBytes(NCore.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
                var des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(a, b), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }

        public static string e(string stringToEncrypt)
        {
            try
            {
                a = Encoding.UTF8.GetBytes(NCore.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(a, b), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }
    }
}