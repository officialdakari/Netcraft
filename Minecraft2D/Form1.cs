/*  
 *  PROJECT: NETCRAFT
 *  AUTHOR: DarkCoder15#5102
 *  GITHUB: https://github.com/GladCypress3030/Netcraft
 *  VERSION: 0.1.7A
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using global::System.Drawing.Imaging;
using global::System.IO;
using System.Linq;
using global::System.Net.Sockets;
using System.Runtime.CompilerServices;
using global::System.Runtime.InteropServices;
using global::System.Security.Cryptography;
using global::System.Text;
using global::System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace Minecraft2D
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
            InventoryButton.Name = "Button1";
            _ListBox1.Name = "ListBox1";
            ChatButton.Name = "Button2";
            _ProgressBar1.Name = "ProgressBar1";
            _localPlayer.Name = "localPlayer";
            _Warning.Name = "Warning";
            _ButtonLeft.Name = "ButtonLeft";
            _ButtonJump.Name = "ButtonJump";
            _ButtonRight.Name = "ButtonRight";
            MenuButton.Name = "Button3";
            _ButtonAttack.Name = "ButtonAttack";
            _ListBox2.Name = "ListBox2";
        }

        private TcpClient client;
        private StreamWriter sWriter;
        public bool connected { get; private set; } = false;

        public delegate void OnMessageReceivedEventHandler(string msg);
        internal string toNotice = null;
        internal int toNoticeType = 2;
        public delegate void _xUpdate(string str);

        private async void Read(IAsyncResult ar)
        {
            try
            {
                var x = Encode.d(await new StreamReader(client.GetStream()).ReadLineAsync()).Split(Conversions.ToChar(Constants.vbLf));
                await handlePackets(x);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                if (connected)
                {
                    connected = false;
                    toNotice = $"{lang.get("text.error.connection_lost")}:\r\n\r\n{ex.ToString()}";
                }
            }
        }

        public async Task handlePackets(string[] x)
        {
            foreach (var a in x)
            {
                string data = a.Replace("\r", "\r\n");
                bool cancel = false;
                PacketReceive?.Invoke(ref data, ref cancel);
                if (cancel) return;
                await Packet(data);
            }
        }

        public async Task Send(string str)
        {
            try
            {
                string data = Encode.e(str);
                bool cancel = false;
                PacketSent?.Invoke(ref data, ref cancel);
                if (cancel) return;
                sWriter = new StreamWriter(client.GetStream());
                await sWriter.WriteLineAsync(data);
                await sWriter.FlushAsync();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                if (connected)
                {
                    toNotice = $"{lang.get("text.error.packet_send_error")}:\r\n\r\n{ex.ToString()}";
                    //FancyMessage.Show($"{lang.get("text.error.packet_send_error")}:\r\n\r\n" + ex.ToString(), "Can't send packet", FancyMessage.Icon.Error);
                    Close();
                }
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
                FancyMessage.Show($"{ex.GetType().ToString()}: {ex.Message}", lang.get("text.error.unable_connect"), FancyMessage.Icon.Error);
                Close();
                MainMenu.GetInstance().Show();
            }
        }

        public async Task Disconnect()
        {
            connected = false;
            client.Client.Close();
            client = null;
        }

        private readonly List<PictureBox> blocks = new List<PictureBox>();
        private readonly List<PictureBox> playerEntities = new List<PictureBox>();
        private readonly List<EntityPlayer> players = new List<EntityPlayer>();
        private string pName;

        public bool IsSingleplayer { get; set; } = false;

        public Process ServerProcess;

        //public Process ServerProcess
        //{
        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    get
        //    {
        //        return _ServerProcess;
        //    }

        //    [MethodImpl(MethodImplOptions.Synchronized)]
        //    set
        //    {
        //        if (_ServerProcess != null)
        //        {
        //            _ServerProcess.OutputDataReceived -= onServerProcessDataReceived;
        //            _ServerProcess.ErrorDataReceived -= onServerProcessErrorDataReceived;
        //        }

        //        _ServerProcess = value;
        //        if (_ServerProcess != null)
        //        {
        //            _ServerProcess.OutputDataReceived += onServerProcessDataReceived;
        //            _ServerProcess.ErrorDataReceived += onServerProcessErrorDataReceived;
        //        }
        //    }
        //}

        internal string ip = "127.0.0.1";
        internal int port = 6575;

        internal void onServerProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (Logs.Instance != null) Logs.Instance.Log(e.Data);
        }

        internal void onServerProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("[ERROR DATA RECEIVED]: " + e.Data);
        }

        public delegate void PacketEventHandler(ref string packet, ref bool cancelled);
        public event PacketEventHandler PacketSent;
        public event PacketEventHandler PacketReceive;

        public Image playerSkin { get; set; } = My.Resources.Resources.sprite;
        public Image playerSkinFlip { get; set; } = My.Resources.Resources.sprite;
        public List<Rectangle> blockRectangles = new List<Rectangle>();

        public enum Craftable
        {
            BREAD,
            PLANKS,
            WOODEN_PICKAXE,
            WOODEN_SWORD,
            WOODEN_AXE, 
            WOODEN_SHOVEL,
            STONE_PICKAXE,
            STONE_SWORD, 
            STONE_AXE,
            STONE_SHOVEL,
            IRON_PICKAXE,
            IRON_SWORD,
            IRON_AXE,
            IRON_SHOVEL, 
            DIAMOND_PICKAXE,
            DIAMOND_SWORD,
            DIAMOND_AXE, 
            DIAMOND_SHOVEL, 
            STICK,
            FURNACE,
            IRON,
            GOLD,
            DIAMOND,
            IRON_BLOCK,
            GOLD_BLOCK,
            DIAMOND_BLOCK,
            BUCKET,
            CHEST,
            HEALING_POTION
        }

        public Control GetControl(string name)
        {
            foreach(Control c in Controls)
            {
                if(c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }
        internal static Form1 instance;
        public static Form1 GetInstance()
        {
            return instance;
        }

        private void DoLang()
        {
            ChatButton.Text = lang.get("game.button.chat");
            InventoryButton.Text = lang.get("game.button.inventory");
            MenuButton.Text = lang.get("game.button.pause");
        }

        private delegate void eScroll();
        private void EnableScroll()
        {
            if(InvokeRequired)
            {
                Invoke(new eScroll(this.EnableScroll));
            } else
            {
                this.AutoScroll = false;
                this.HorizontalScroll.Enabled = true;
                this.HorizontalScroll.Visible = true;
                this.VerticalScroll.Enabled = true;
                this.VerticalScroll.Visible = true;
                this.AutoScroll = true;
            }
        }

        public void Darker()
        {
            Bitmap bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            using (Graphics G = Graphics.FromImage(bmp))
            {
                G.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                G.CopyFromScreen(this.PointToScreen(new Point(0, 0)), new Point(0, 0), this.ClientRectangle.Size);
                double percent = 0.50;
                Color darken = Color.FromArgb((int)(255 * percent), Color.Black);
                using (Brush brsh = new SolidBrush(darken))
                {
                    G.FillRectangle(brsh, this.ClientRectangle);
                }
            }
            makeItDark.Image = bmp;
            makeItDark.SizeMode = PictureBoxSizeMode.Normal;
            makeItDark.Show();
            makeItDark.Size = ClientSize + new Size(100, 100);
        }    

        public void Lighten()
        {
            makeItDark.Hide();
        }

        public Image GetFlipped(Image bmp)
        {
            Bitmap b = (Bitmap)bmp.Clone();
            b.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return b;
        }

        Lang lang;
        Dictionary<string, string> itemNames = new Dictionary<string, string>();
        NCResources ncr;
        string logFileName = $"log-{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-")}.txt";
        private async void Form1_Load(object sender, EventArgs e)
        {
            instance = this;
            log($"Netcraft Beta v{MainMenu.GetInstance().Ver}");
            log("by DarkCoder15 & TheNonameee");
            log("");
            log("Initializing...");
            DateTime loadStartTime = DateTime.Now;

            log($"Loading resources");
            ncr = NCResources.Load("./ncr");
            log($"Resources loaded");
            NConsole.instance = new NConsole();
            if(Debugger.IsAttached || (Interaction.Command() == "dbg"))
            {
                NConsole.instance.Show();
            }
            invClose1.BringToFront();
            chatPanel1.KeyDown += Form1_KeyDown;
            chatPanel1.KeyUp += Form1_KeyDown;
            makeItDark.KeyDown += Form1_KeyDown;
            makeItDark.KeyUp += Form1_KeyDown;
            foreach (Control ac in chatPanel1.Controls)
            {
                ac.KeyDown += Form1_KeyDown;
                ac.KeyUp += Form1_KeyUp;
            }
            Bitmap bmp;
            log("Attempting to get local player sprite texture from " + MainMenu.GetInstance().textBox3.Text);
            try
            {
                System.Net.WebRequest request =
    System.Net.WebRequest.Create(
    MainMenu.GetInstance().textBox3.Text);
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream =
                    response.GetResponseStream();
                bmp = new Bitmap(responseStream);
                log("Got player sprite from " + MainMenu.GetInstance().textBox3.Text);
            }
            catch (Exception ex)
            {
                bmp = My.Resources.Resources.sprite;
                log("Can't load image from " + MainMenu.GetInstance().textBox3.Text);
            }
            log("Setting variables...");
            playerSkin = bmp;
            playerSkinFlip = (Bitmap)bmp.Clone();
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            log("Done setting variables");
            log($"Translating UI to {Utils.LANGUAGE} language...");
            DoLang();
            log("Done translating");
            
            playerSkinFlip.RotateFlip(RotateFlipType.Rotate180FlipY);

            if (!Directory.Exists(@".\mods"))
            {
                log("Mods folder not found, creating one for you...");
                Directory.CreateDirectory(@".\mods");
            }
            if (!Directory.Exists(@".\logs"))
            {
                log("Logs folder not found, creating one for you...");
                Directory.CreateDirectory(@".\logs");
            }

            log("Setting control layers...");
            localPlayer.SendToBack();
            R1.BringToFront();
            ListBox1.BringToFront();
            log("Done setting control layers");
            this.Text = "NetCraft " + MainMenu.GetInstance().Ver;
            if (IsSingleplayer)
            {
                log("Note: singleplayer server loaded. If you close opened console, server will shut down");
                Text = $"NetCraft {My.MyProject.Forms.MainMenu.Ver} (Singleplayer)";
            }

            log($"Connecting to {ip}:{port}");
            await Connect(ip, port);
            if(!connected)
            {
                log("Connect failed");
                MainMenu.GetInstance().Show();
                leave();
                Close();
                return;
            }
            pName = Utils.InputBox("text.playername");
            if (pName == null)
            {
                log($"Player name is null, closing...");
                Close();
                return;
            }
            log($"Player name: {pName}");
            await SendPacket("setname", pName, Utils.LANGUAGE, MainMenu.GetInstance().textBox3.Text);
            log($"Sending 'SETUP' packet with arguments ['{pName}', '{Utils.LANGUAGE}', '{MainMenu.GetInstance().textBox3.Text}']");
            Username = pName;
            NConsole.instance.winFormsConsole1.PromptString = pName + "@" + ip + " > ";
            NConsole.instance.Text = pName + " - Netcraft Developer Shell";
            NConsole.instance.winFormsConsole1.reload();
            Thread.Sleep(900);
            log($"Sending 'REQUEST_WORLD' packet");
            SendSinglePacket("world");
            WriteChat("Client message: Вы вошли на сервер");
            initializeMove();
            log($"Loading craftable items...");
            foreach (var i in Enum.GetNames(typeof(Craftable))) {
                log($"Added craftable: '{i}'");
                ListBox2.Items.Add(i.ToLower());
            }
            //cTicker = new Thread(tickLoop);
            //cTicker.Start();
            log($"Starting tick timer");
            timer1.Start();
            log($"Started tick timer");
            foreach (var pluginPath in Directory.GetFiles(@".\mods"))
                PluginManager.Load(pluginPath);
            try
            {
                log($"Loading Discord RPC...");
                MainMenu.GetInstance().presence.Details = String.Format(lang.get("rpc.playername"), pName) + " | Netcraft " + MainMenu.GetInstance().Ver;
                if (!IsSingleplayer)
                {
                    MainMenu.GetInstance().presence.State = lang.get("rpc.playing.network_game");
                }
                else
                {
                    MainMenu.GetInstance().presence.State = lang.get("rpc.playing.singleplayer");
                }
                MainMenu.GetInstance().dRPC.SetPresence(MainMenu.GetInstance().presence);
                log($"Loaded Discord RPC");
            }
            catch (Exception ex)
            {
                log($"Failed Discord RPC loading: {ex.Message}");
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
            }
            //audioPlay();
            foreach(Control c in Controls)
            {
                c.MouseMove += Form1_MouseMove;
            }
            log($"Netcraft loaded in {(DateTime.Now - loadStartTime).TotalMilliseconds} ms!");
        }

        internal void audioPlay()
        {
            My.MyProject.Computer.Audio.Play("./resources/game_music_loop.wav", AudioPlayMode.BackgroundLoop);
        }

        internal void audioStop()
        {
            My.MyProject.Computer.Audio.Stop();
        }

        private async void initializeMove()
        {

            mThread = new Thread(moveLoop);
            mThread.Start();
        }

        public async void tickLoop()
        {
            //while (true)
            //{
            //    Thread.Sleep(5);
            //    Tick();
            //}
        }

        public Thread cTicker;
        private Thread mThread;

        private void OnJoinRequested()
        {
            Debug.WriteLine(Constants.vbCrLf + "Someone tried to join game" + Constants.vbCrLf);
        }

        public delegate void xChat(string arg0);
        public delegate void xSetChatLine(int i, string arg0);

        public void WriteChat(string arg0)
        {
            if (InvokeRequired)
            {
                Invoke(new xChat(WriteChat), arg0);
                return;
            }
            try
            {
                chat += arg0 + "\n";
                richTextBox1.AppendText(arg0 + "\r\n");
            } catch(Exception)
            {

            }
            //My.MyProject.Forms.Chat.RichTextBox1.AppendText(arg0 + Constants.vbCrLf);
            //My.MyProject.Forms.Chat.RichTextBox1.Select(My.MyProject.Forms.Chat.RichTextBox1.TextLength, 0);
            //My.MyProject.Forms.Chat.RichTextBox1.ScrollToCaret();
        }
        string chat = "";
        public void SetChatLine(int i, string line)
        {
            if (InvokeRequired)
            {
                Invoke(new xSetChatLine(SetChatLine), i, line);
                return;
            }
            try
            {
                //richTextBox1.Lines[i] = line;
                string[] lines = chat.Split('\n');
                lines[i] = line;
                richTextBox1.Text = string.Join("\r\n", lines);
                ColorAA();
            }
            catch (Exception)
            {

            }
        }

        public delegate void xHealth(int arg0);

        public void SetHealth(int arg0)
        {
            if (InvokeRequired)
            {
                Invoke(new xHealth(SetHealth), (object)arg0);
            }
            else
            {
                ProgressBar1.Value = arg0;
            }
        }

        private void ColorAA()
        {

            var HT = new Dictionary<string, Color>();
            HT.Add("&0", Color.DodgerBlue);
            HT.Add("&1", Color.Red);
            HT.Add("&2", Color.Orange);
            HT.Add("&3", Color.Yellow);
            HT.Add("&4", Color.Magenta);
            HT.Add("&5", Color.Lime);
            HT.Add("&r", richTextBox1.ForeColor);
            string wrd = string.Join("|", HT.Keys.Cast<string>().ToArray());
            MatchCollection allIp = Regex.Matches(richTextBox1.Text, wrd);
            foreach (Match ip in allIp)
            {
                int ind = richTextBox1.Find(ip.Value);
                richTextBox1.Select(ind, richTextBox1.TextLength - ind);
                richTextBox1.SelectionColor = HT[ip.Value];
                richTextBox1.SelectedText = richTextBox1.SelectedText.Remove(0, ip.Value.Length);
                richTextBox1.DeselectAll();
            }
            richTextBox1.DeselectAll();
            HighlightTagging();
        }

        public void HighlightTagging()
        {
            string Words_dont_come_easy = "\\@" + Username;
            MatchCollection allIp = Regex.Matches(richTextBox1.Text, Words_dont_come_easy);
            foreach (Match ip in allIp)
            {
                richTextBox1.SelectionStart = ip.Index;
                richTextBox1.SelectionLength = ip.Length;
                richTextBox1.SelectionBackColor = Color.Gold;
                richTextBox1.SelectionColor = Color.Blue;
            }
        }


        public void SetHunger(int arg0)
        {
            if (InvokeRequired)
            {
                Invoke(new xHealth(SetHealth), (object)arg0);
            }
            else
            {
                progressBar1.Value = arg0;
            }
        }

        public delegate void xTp(int x, int y);

        public void TeleportLocalPlayer(int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new xTp(TeleportLocalPlayer), x, y);
            }
            else
            {
                localPlayer.Location = new Point(x, y);
            }
        }

        ChestWindow chest;

        public async Task Packet(string p)
        {
            InvokePacket(p);
        }
        public delegate Task invokePacket(string p);
        public async Task InvokePacket(string p)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new invokePacket(InvokePacket), p);
            } else
            {

                var a = p.Split('?');
                if(a[0] == "evalresult")
                {
                    string m = string.Join("?", a.Skip(1).ToArray());
                    log(m.Replace("\r", "\r\n") + "\r\n");
                    //NConsole.instance.richTextBox1.AppendText(m.Replace("\r", "\r\n") + "\r\n");
                    //NConsole.instance.richTextBox1.Select(textBox2.TextLength, 0);
                    //NConsole.instance.richTextBox1.ScrollToCaret();
                }
                if (a[0] == "blockchange")
                {
                    var b = new PictureBox();
                    bool g = false;
                    if (a.Length == 5)
                    {
                        b.Tag = a[4];
                    }

                    b.Name = a[1] + "B" + a[2];
                    b.BackColor = Color.Transparent;
                    b.Size = new Size(32, 32);
                    b.Location = new Point((Conversions.ToInteger(a[1]) * 32) - HorizontalScroll.Value, Conversions.ToInteger(a[2]) * 32 - VerticalScroll.Value);
                    if (a[3] == "iron_ore")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("iron_ore");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "grass_block")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("grass_side");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "dandelion")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("dandelion");
                    }
                    else if (a[3] == "poppy")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("poppy");
                    }
                    else if (a[3] == "dirt")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("dirt");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "stone")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("stone");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "gravel")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("gravel");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "bedrock")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("bedrock");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "wood")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("wood");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "leaves")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("leaves");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "cobble")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("COBBLESTONE");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "planks")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("planks");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "endstone")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("endstone");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "netcraft_block")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("NETCRAFT_BLOCK");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "netcraft_block_snowy")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("SNOWY_NETCRAFT_BLOCK");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "diamond_ore")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("diamond_ore");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "snowygrass")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("grass_block_snow");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "gold_ore")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("gold_ore");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "obsidian")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("obsidian");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "water")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("WATER");
                        b.BackColor = Color.Blue;
                    }
                    else if (a[3] == "fire")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("FIRE");
                    }
                    else if (a[3] == "sand")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("sand");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "glass")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("glass");
                    }
                    else if (a[3] == "furnace")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("FURNACE");
                        b.Tag += b.Tag.ToString() + "?furnace";
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "iron_block")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("iron_block");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "diamond_block")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("diamond_block");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "gold_block")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("gold_block");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "coal_ore")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("coal_ore");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "wheat0")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("wheat");
                    }
                    else if (a[3] == "wheat7")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("WHEAT_GROW");
                    }
                    else if (a[3] == "sapling")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("sapling");
                    }
                    else if (a[3] == "tnt") 
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("TNT");
                        b.BackColor = BackColor;
                    }
                    else if (a[3] == "lava")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("lava");
                        b.BackColor = Color.Orange;
                    }
                    else if (a[3] == "chest")
                    {
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        b.BackgroundImage = ncr.GetBitmap("chest");
                    }
                    else
                    {
                        b.BackColor = Color.FromName(a[3]);
                    }

                    b.Visible = true;
                    b.Text = "";
                    blocks.Add(b);
                    await CreateBlock(b);
                }

                if (a[0] == "hunger")
                {
                    SetHunger(int.Parse(a[1]));
                }

                if (a[0] == "removeblock")
                {
                    await BreakBlock(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
                }

                if (a[0] == "addplayer")
                {
                    await CreatePlayer(a[1], 0 - HorizontalScroll.Value, 0, a[2]);
                    Console.WriteLine($"Player added: {a[1]}");
                }

                if (a[0] == "removeplayer")
                {
                    await DelPlayer(a[1]);
                    Console.WriteLine($"Player removed: {a[1]}");
                }

                if (a[0] == "updateplayerposition")
                {
                    await MovePlayer(a[1], Conversions.ToInteger(a[2]) - HorizontalScroll.Value, Conversions.ToInteger(a[3]) - VerticalScroll.Value);
                }

                if (a[0] == "completeload")
                {
                    EnableScroll();
                }

                if (a[0] == "teleport")
                {
                    TeleportLocalPlayer(Conversions.ToInteger(a[1]) - HorizontalScroll.Value, Conversions.ToInteger(a[2]) - VerticalScroll.Value);
                    await UpdatePlayerPosition();
                    Console.WriteLine(String.Format("Local player teleported: {0}, {1}", a[1], a[2]));
                    log(String.Format("Local player teleported: {0}, {1}", a[1], a[2]));
                }

                if (a[0] == "clearinventory")
                {
                    Invoke(new MethodInvoker(ListBox1.Items.Clear));
                }

                if (a[0] == "additem")
                {
                    await AddItem(a[1]);
                }

                if (a[0] == "setchatline")
                {
                    int i = int.Parse(a[1]);
                    if (i + 1 > richTextBox1.Lines.Length) return;
                    //richTextBox1.Lines[i] = string.Join("?", a.Skip(2).ToArray());
                    SetChatLine(i, string.Join("?", a.Skip(2).ToArray()));
                    log($"Server chat: Line {i.ToString()} edited to '{string.Join("?", a.Skip(2).ToArray())}'");
                }

                if (a[0] == "msgerror")
                {
                    FancyMessage.Show(string.Join("?", a.Skip(1).ToArray()), "Netcraft", FancyMessage.Icon.Error);
                    Console.WriteLine($"Error message from server: {string.Join("?", a.Skip(1).ToArray())}");
                }

                if (a[0] == "msgwarn")
                {
                    FancyMessage.Show(string.Join("?", a.Skip(1).ToArray()), "Netcraft", FancyMessage.Icon.Warning);
                    Console.WriteLine($"Warning message from server: {string.Join("?", a.Skip(1).ToArray())}");
                }

                if (a[0] == "msg")
                {
                    FancyMessage.Show(string.Join("?", a.Skip(1).ToArray()), "Netcraft", FancyMessage.Icon.Info);
                    Console.WriteLine($"Info message from server: {string.Join("?", a.Skip(1).ToArray())}");
                }

                if (a[0] == "msgkick")
                {
                    FancyMessage.Show(string.Join("?", a.Skip(1).ToArray()), lang.get("text.kicked"), FancyMessage.Icon.Info);
                    Console.WriteLine($"Kicked from server: {string.Join("?", a.Skip(1).ToArray())}");
                    log($"Kicked from server: {string.Join("?", a.Skip(1).ToArray())}");
                    Close();
                }

                if (a[0] == "chat")
                {
                    WriteChat(string.Join("?", a.Skip(1).ToArray()));
                    Console.WriteLine($"Chat: {string.Join("?", a.Skip(1).ToArray())}");
                    log($"Server chat: {string.Join("?", a.Skip(1).ToArray())}");
                }


                if (a[0] == "health")
                {
                    SetHealth(Conversions.ToInteger(a[1]));
                    Console.WriteLine("Health update: " + a[1]);
                }

                if (a[0] == "noclip")
                {
                    NoClip = true;
                }

                if (a[0] == "clip")
                {
                    NoClip = false;
                }

                if (a[0] == "dowarn")
                {
                    await DoWarning(string.Join("?", a.Skip(1).ToArray()));
                }

                if (a[0] == "sky")
                {
                    BackColor = Color.FromName(a[1]);
                    Update();
                }

                if(a[0] == "addentity")
                {
                    await CreateEntity(a[1], a[2]);
                }

                if (a[0] == "moveentity")
                {
                    await MoveEntity(a[1], new Point(int.Parse(a[2]), int.Parse(a[3])));
                }

                if (a[0] == "delentity")
                {
                    await DeleteEntity(a[1]);
                }

                if (a[0] == "chestopen")
                {
                    ChestWindow cw = new ChestWindow();
                    //cw.listBox1.Items.AddRange(ListBox1.Items);
                    cw.listBox2.Items.AddRange(a.Skip(1).ToArray());
                    cw.ShowDialog();
                    chest = cw;
                    cw.Close();
                    cw.Dispose();
                    chest = null;
                }

                if(a[0] == "updatestats")
                {
                    log($"Received 'UPDATE_STATS': {string.Join("?", a.Skip(1))}");
                    Stats = JsonConvert.DeserializeObject<Dictionary<string, string>>(string.Join("?", a.Skip(1)));
                }

                if (a[0] == "itemset")
                {
                    try
                    {
                        // REM - Деревянные инструменты
                        if (a[2] == "WOODEN_PICKAXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("WOODEN_PICKAXE"), ncr.GetBitmapFlipped("WOODEN_PICKAXE"), a[2]);
                        }

                        if (a[2] == "WOODEN_AXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("WOODEN_AXE"), ncr.GetBitmapFlipped("WOODEN_AXE"), a[2]);
                        }

                        if (a[2] == "WOODEN_SWORD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("WOODEN_SWORD"), ncr.GetBitmapFlipped("WOODEN_SWORD"), a[2]);
                        }

                        if (a[2] == "WOODEN_SHOVEL")
                        {
                            await SetItem(a[1], ncr.GetBitmap("WOODEN_SHOVEL"), ncr.GetBitmapFlipped("WOODEN_SHOVEL"), a[2]);
                        }


                        // REM - Каменные инструменты
                        if (a[2] == "STONE_PICKAXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("STONE_PICKAXE"), ncr.GetBitmapFlipped("STONE_PICKAXE"), a[2]);
                        }

                        if (a[2] == "STONE_AXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("STONE_AXE"), ncr.GetBitmapFlipped("STONE_AXE"), a[2]);
                        }

                        if (a[2] == "STONE_SWORD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("STONE_SWORD"), ncr.GetBitmapFlipped("STONE_SWORD"), a[2]);
                        }

                        if (a[2] == "STONE_SHOVEL")
                        {
                            await SetItem(a[1], ncr.GetBitmap("STONE_SHOVEL"), ncr.GetBitmapFlipped("STONE_SHOVEL"), a[2]);
                        }

                        // REM - Железные инструменты
                        if (a[2] == "IRON_PICKAXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("IRON_PICKAXE"), ncr.GetBitmapFlipped("IRON_PICKAXE"), a[2]);
                        }

                        if (a[2] == "IRON_AXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("IRON_AXE"), ncr.GetBitmapFlipped("IRON_AXE"), a[2]);
                        }

                        if (a[2] == "IRON_SWORD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("IRON_SWORD"), ncr.GetBitmapFlipped("IRON_SWORD"), a[2]);
                        }

                        if (a[2] == "IRON_SHOVEL")
                        {
                            await SetItem(a[1], ncr.GetBitmap("IRON_SHOVEL"), ncr.GetBitmapFlipped("IRON_SHOVEL"), a[2]);
                        }

                        // REM - Алмазные инструменты
                        if (a[2] == "DIAMOND_PICKAXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("DIAMOND_PICKAXE"), ncr.GetBitmapFlipped("DIAMOND_PICKAXE"), a[2]);
                        }

                        if (a[2] == "DIAMOND_AXE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("DIAMOND_AXE"), ncr.GetBitmapFlipped("DIAMOND_AXE"), a[2]);
                        }

                        if (a[2] == "DIAMOND_SWORD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("DIAMOND_SWORD"), ncr.GetBitmapFlipped("DIAMOND_SWORD"), a[2]);
                        }

                        if (a[2] == "DIAMOND_SHOVEL")
                        {
                            await SetItem(a[1], My.Resources.Resources.DIAMOND_SHOVEL, My.Resources.Resources.DIAMOND_SHOVEL_FLIPPED, a[2]);
                        }

                        // REM - Блоки
                        if (a[2] == "GRASS_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("grass_side"), ncr.GetBitmap("grass_side"), a[2]);
                        }

                        if (a[2] == "SNOWY_GRASS_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("grass_block_snow"), ncr.GetBitmap("grass_block_snow"), a[2]);
                        }

                        if (a[2] == "NETCRAFT_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("NETCRAFT_BLOCK"), ncr.GetBitmap("NETCRAFT_BLOCK"), a[2]);
                        }

                        if (a[2] == "SNOWY_NETCRAFT_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("NETCRAFT_BLOCK"), My.Resources.Resources.snowylogo, a[2]);
                        }

                        if (a[2] == "WOOD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("wood"), ncr.GetBitmap("wood"), a[2]);
                        }

                        if (a[2] == "COBBLESTONE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("COBBLESTONE"), ncr.GetBitmap("COBBLESTONE"), a[2]);
                        }

                        if (a[2] == "STONE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("stone"), ncr.GetBitmap("stone"), a[2]);
                        }

                        if (a[2] == "GRAVEL")
                        {
                            await SetItem(a[1], ncr.GetBitmap("gravel"), ncr.GetBitmap("gravel"), a[2]);
                        }

                        if (a[2] == "END_STONE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("end_stone"), ncr.GetBitmap("end_stone"), a[2]);
                        }

                        if (a[2] == "PLANKS")
                        {
                            await SetItem(a[1], ncr.GetBitmap("planks"), ncr.GetBitmap("planks"), a[2]);
                        }

                        if (a[2] == "DIRT")
                        {
                            await SetItem(a[1], ncr.GetBitmap("dirt"), ncr.GetBitmap("dirt"), a[2]);
                        }

                        if (a[2] == "OBSIDIAN")
                        {
                            await SetItem(a[1], ncr.GetBitmap("obsidian"), ncr.GetBitmap("obsidian"), a[2]);
                        }

                        if (a[2] == "SAND")
                        {
                            await SetItem(a[1], ncr.GetBitmap("sand"), ncr.GetBitmap("sand"), a[2]);
                        }

                        if (a[2] == "GLASS")
                        {
                            await SetItem(a[1], ncr.GetBitmap("glass"), ncr.GetBitmap("glass"), a[2]);
                        }

                        if (a[2] == "FURNACE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("FURNACE"), ncr.GetBitmap("FURNACE"), a[2]);
                        }

                        if (a[2] == "LEAVES")
                        {
                            await SetItem(a[1], ncr.GetBitmap("leaves"), ncr.GetBitmap("leaves"), a[2]);
                        }

                        if (a[2] == "SAPLING")
                        {
                            await SetItem(a[1], ncr.GetBitmap("sapling"), ncr.GetBitmap("sapling"), a[2]);
                        }

                        if (a[2] == "CHEST")
                        {
                            await SetItem(a[1], ncr.GetBitmap("chest"), ncr.GetBitmap("sapling"), a[2]);
                        }

                        // REM - Драгоценные блоки
                        if (a[2] == "IRON_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("iron_block"), ncr.GetBitmap("iron_block"), a[2]);
                        }

                        if (a[2] == "DIAMOND_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("diamond_block"), ncr.GetBitmap("diamond_block"), a[2]);
                        }

                        if (a[2] == "GOLD_BLOCK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("gold_block"), ncr.GetBitmap("gold_block"), a[2]);
                        }

                        if (a[2] == "GOLD_ORE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("gold_ore"), ncr.GetBitmap("gold_ore"), a[2]);
                        }

                        if (a[2] == "IRON_ORE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("iron_ore"), ncr.GetBitmap("iron_ore"), a[2]);
                        }

                        // REM - Еда
                        if (a[2] == "RAW_BEEF")
                        {
                            await SetItem(a[1], ncr.GetBitmap("beef"), ncr.GetBitmap("beef"), a[2]);
                        }
                        if (a[2] == "COOKED_BEEF")
                        {
                            await SetItem(a[1], ncr.GetBitmap("cooked_beef"), ncr.GetBitmap("cooked_beef"), a[2]);
                        }
                        if (a[2] == "BREAD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("bread"), ncr.GetBitmap("bread"), a[2]);
                        }

                        // REM - Инструменты
                        if (a[2] == "FIRE")
                        {
                            await SetItem(a[1], ncr.GetBitmap("flint_and_steel"), ncr.GetBitmap("flint_and_steel"), a[2]);
                        }

                        // REM - Разное
                        if (a[2] == "STICK")
                        {
                            await SetItem(a[1], ncr.GetBitmap("STICK"), ncr.GetBitmap("STICK"), a[2]);
                        }

                        if (a[2] == "COAL")
                        {
                            await SetItem(a[1], ncr.GetBitmap("coal"), ncr.GetBitmap("coal"), a[2]);
                        }

                        if (a[2] == "RED_FLOWER")
                        {
                            await SetItem(a[1], ncr.GetBitmap("poppy"), ncr.GetBitmap("poppy"), a[2]);
                        }

                        if (a[2] == "YELLOW_FLOWER")
                        {
                            await SetItem(a[1], ncr.GetBitmap("dandelion"), ncr.GetBitmap("dandelion"), a[2]);
                        }

                        if (a[2] == "IRON")
                        {
                            await SetItem(a[1], ncr.GetBitmap("IRON"), ncr.GetBitmap("IRON"), a[2]);
                        }

                        if (a[2] == "GOLD")
                        {
                            await SetItem(a[1], ncr.GetBitmap("GOLD"), ncr.GetBitmap("GOLD"), a[2]);
                        }

                        if (a[2] == "DIAMOND")
                        {
                            await SetItem(a[1], ncr.GetBitmap("DIAMOND"), ncr.GetBitmap("DIAMOND"), a[2]);
                        }

                        if (a[2] == "TNT")
                        {
                            await SetItem(a[1], ncr.GetBitmap("TNT"), ncr.GetBitmap("TNT"), a[2]);
                        }

                        if (a[2] == "BUCKET")
                        {
                            await SetItem(a[1], ncr.GetBitmap("bucket"), ncr.GetBitmap("bucket"), a[2]);
                        }

                        if (a[2] == "WATER_BUCKET")
                        {
                            await SetItem(a[1], ncr.GetBitmap("water_bucket"), ncr.GetBitmap("water_bucket"), a[2]);
                        }

                        if (a[2] == "LAVA_BUCKET")
                        {
                            await SetItem(a[1], ncr.GetBitmap("lava_bucket"), ncr.GetBitmap("lava_bucket"), a[2]);
                        }

                        if (a[2] == "WHEAT")
                        {
                            await SetItem(a[1], ncr.GetBitmap("wheat"), ncr.GetBitmap("wheat"), a[2]);
                        }

                        if (a[2] == "SEEDS")
                        {
                            await SetItem(a[1], ncr.GetBitmap("wheat_seeds"), ncr.GetBitmap("wheat_seeds"), a[2]);
                        }

                        // REM - Зелья
                        if(a[2] == "HEALING_POTION")
                        {
                            await SetItem(a[1], ncr.GetBitmap("potion_bottle_drinkable"), ncr.GetBitmap("potion_bottle_drinkable"), a[2]);
                        }

                        // REM - Nothing
                        if (a[2] == "nothing")
                        {
                            await SetItem(a[1], null, null, a[2]);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                        FancyMessage.Show($"Error while setting texture in hand of player '{Utils.IIf(a[1] == "@", "local", a[1]).ToString()}' to: \"{a[2]}\".\r\n\r\n{ex.ToString()}", "Exception", FancyMessage.Icon.Warning);
                    }
                }
            }
        }

        public int moveInterval = 10;

        public delegate Task xsetSkyColor(Color a);

        public async Task SetSkyColor(Color a)
        {
            BackColor = a;
        }

        public delegate Task xSetItem(string n, Image i, Image iflipped, string str);

        public async Task SetItem(string n, Image i, Image iflipped, string str)
        {
            if (InvokeRequired)
            {
                Invoke(new xSetItem(SetItem), n, i, iflipped, str);
            }
            else
            {
                if (n == "@")
                {
                    await SetItemInHand(i, iflipped, str);
                    return;
                }

                foreach (var p in players)
                {
                    if ((p.Name ?? "") == (n ?? ""))
                    {
                        await p.SetItemInHand(i, iflipped, str);
                    }
                }
            }
        }

        public async Task SetItemInHand(Image a, Image b, string c)
        {
            ItemInImage = a;
            ItemInImageFlipped = b;
            localPlayer.Update();
            R1.Update();
        }

        private void furnaceInteract(object sender, MouseEventArgs e)
        {
        }

        public bool NoClip { get; set; } = false;
        public string SetText { get; set; } = "";

        public async Task UpdateWindowText()
        {
            Text = SetText;
        }

        public delegate Task xAddItem(string s);

        public async Task AddItem(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new xAddItem(AddItem), s);
            }
            else
            {
                ListBox1.Items.Add(s);
            }
        }

        public delegate Task AddBlock(PictureBox b);

        public void ApplyBrightness(ref Bitmap bmp, int brightnessValue)
        {
            log($"API WARNING: ApplyBrightness is absolute");
            try
            {
                var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                var ptr = bmpData.Scan0;
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;
                int val = 0;
                while ((int)ptr != stopAddress)
                {
                    val = Marshal.ReadByte(ptr + 2) + brightnessValue;
                    if (val < 0)
                    {
                        val = 0;
                    }
                    else if (val > 255)
                    {
                        val = 255;
                    }

                    Marshal.WriteByte(ptr + 2, (byte)val);
                    val = Marshal.ReadByte(ptr + 2) + brightnessValue;
                    if (val < 0)
                    {
                        val = 0;
                    }
                    else if (val > 255)
                    {
                        val = 255;
                    }

                    Marshal.WriteByte(ptr + 1, (byte)val);
                    val = Marshal.ReadByte(ptr) + brightnessValue;
                    if (val < 0)
                    {
                        val = 0;
                    }
                    else if (val > 255)
                    {
                        val = 255;
                    }

                    Marshal.WriteByte(ptr, (byte)val);
                    ptr = ptr + 3;
                }

                bmp.UnlockBits(bmpData);
            } catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
            }
        }

        Dictionary<Keys, Action> keybinds = new Dictionary<Keys, Action>();

        public void AddKeybind(Keys key, Action action)
        {
            if(keybinds.ContainsKey(key))
            {
                throw new Exception();
                return;
            }
            log($"API: Added keybind '{key.ToString()}' with action '{action.ToString()}'");
            keybinds.Add(key, action);
        }

        public void RemoveKeybind(Keys key)
        {
            if (!keybinds.ContainsKey(key))
            {
                throw new Exception();
                return;
            }
            log($"API: Removed keybind '{key.ToString()}'");
            keybinds.Remove(key);
        }

        public async Task CreateBlock(PictureBox b)
        {
            if (InvokeRequired)
            {
                Invoke(new AddBlock(CreateBlock), b);
            }
            else
            {
                Controls.Add(b);
                try
                {
                    if (b.Tag != null && Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                    {
                        Bitmap bmp = new Bitmap((Image)b.BackgroundImage.Clone());
                        setDarked(ref bmp);
                        b.BackgroundImage = bmp;
                    }
                //        // b.CreateGraphics.FillRectangle(New SolidBrush(Color.FromArgb(29, 0, 0, 0)), 0, 0, b.Width, b.Height)
                //        // AddHandler b.Paint, AddressOf bgBlockPaint
                //        // b.BackgroundImage = ColorProcessing(b.BackgroundImage, 0, 0, 0, 0, 0, -1, -1, 0)
                //        var g = b.BackgroundImage.Clone();
                //        Bitmap argbmp = (Bitmap)g;
                //        ApplyBrightness(ref argbmp, -30);
                //        b.BackgroundImage = (Image)g;
                //    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                }

                if (b.Tag.ToString().Contains("furnace"))
                {
                    // Throw New Exception
                    // AddHandler b.MouseUp, AddressOf furnaceInteract
                }
                b.MouseDown += OnBlockClick;
                b.KeyDown += Form1_KeyDown;
                b.KeyUp += Form1_KeyUp;
                b.MouseMove += Form1_MouseMove;
                b.MouseEnter += blockMouseEnter;
                b.MouseLeave += blockMouseLeave;
                b.SendToBack();
            }
        }

        private void B_Paint(object sender, PaintEventArgs e)
        {
            
        }

        void setDarked(ref Bitmap origin, double percent = 0.40)
        {
            using (Graphics G = Graphics.FromImage(origin))
            {
                G.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                
                Color darken = Color.FromArgb((int)(255 * percent), Color.Black);
                using (Brush brsh = new SolidBrush(darken))
                {
                    G.FillRectangle(brsh, new Rectangle(Point.Empty, origin.Size));
                }
            }
        }

        private void B_MouseEnter(object sender, EventArgs e)
        {
            var c = (Control)sender;
            var rect = ClientRectangle;
            rect.Width = 32;
            rect.Height = 32;
            var clr = Color.DodgerBlue;
            int width = 2;

            var g = c.CreateGraphics();
            ControlPaint.DrawBorder(g, rect,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid);
        }

        private void blockMouseEnter(object sender, EventArgs e)
        {
            B_MouseEnter(sender, e);
        }

        private void blockMouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).Invalidate();
        }

        public delegate Task AddPlayer(string name, int x, int y, string url);

        public async Task CreatePlayer(string name, int x, int y, string url)
        {
            if (InvokeRequired)
            {
                Invoke(new AddPlayer(CreatePlayer), name, x, y, url);
            }
            else
            {
                Bitmap bmp;
                log($"Creating '{name}' player entity");
                try
                {
                    System.Net.WebRequest request =
        System.Net.WebRequest.Create(
        url);
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream =
                        response.GetResponseStream();
                    bmp = new Bitmap(responseStream);
                } catch(Exception ex)
                {
                    bmp = My.Resources.Resources.sprite;
                }
                var b = new TransparentPicBox();
                Controls.Add(b);
                b.Tag = name;
                b.Image = bmp;
                b.SizeMode = PictureBoxSizeMode.StretchImage;
                b.Size = localPlayer.Size;
                b.Left = x - HorizontalScroll.Value;
                b.Top = y - VerticalScroll.Value;
                b.BackColor = Color.Transparent;
                b.KeyDown += Form1_KeyDown;
                b.KeyUp += Form1_KeyUp;
                b.Click += AttackPlayer;
                ToolTip1.SetToolTip(b, String.Format(lang.get("game.tooltip.playername"), name));
                playerEntities.Add(b);
                EntityPlayer en = new EntityPlayer(name, "", new Point(x, y), b);
                en.Sprite = bmp;
                players.Add(en);
                b.BringToFront();
                log($"Created '{name}' player entity");
            }
        }

        public Dictionary<string, Entity.Entity> entities = new Dictionary<string, Entity.Entity>();

        public delegate Task AddEntity(string uuid, string type);
        public async Task CreateEntity(string uuid, string type)
        {
            if(InvokeRequired)
            {
                Invoke(new AddEntity(CreateEntity), uuid, type);
            } else
            {
                TransparentPicBox pic = new TransparentPicBox();
                log($"Creating '{uuid}' entity");
                if (type == "test")
                {
                    pic.SetBounds(0, 0, 32, 32);
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Image = My.Resources.Resources.bedrock;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                if (type == "Cow")
                {
                    pic.Size = new Size(64, 40);
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Image = My.Resources.Resources.cow;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                entities.Add(uuid, new Entity.Entity(uuid, pic, new Point(0 - HorizontalScroll.Value, 0 - VerticalScroll.Value), pic.Size, type));
                Controls.Add(entities[uuid].Renderer);
                log($"Created '{uuid}' '{type}' entity");
            }
        }

        public delegate Task _moveEntity(string uuid, Point point);
        public async Task MoveEntity(string uuid, Point point)
        {
            if(InvokeRequired)
            {
                Invoke(new _moveEntity(MoveEntity), uuid, point);
            } else
            {
                Entity.Entity e = entities[uuid];
                if(e.Type == "Cow")
                {
                    if(point.X < e.Position.X)
                    {
                        e.Renderer.Image = GetFlipped(My.Resources.Resources.cow);
                    } else
                    {
                        e.Renderer.Image = My.Resources.Resources.cow;
                    }
                }
                e.Position = new Point(point.X - HorizontalScroll.Value, point.Y - VerticalScroll.Value);
                e.Renderer.Location = e.Position;
            }
        }

        public delegate Task RemoveEntity(string uuid);
        public async Task DeleteEntity(string uuid)
        {
            if(InvokeRequired)
            {
                Invoke(new RemoveEntity(DeleteEntity), uuid);
            } else
            {
                Controls.Remove(entities[uuid].Renderer);
                entities.Remove(uuid);
                log($"Deleted '{uuid}' entity from the world");
            }
        }

        private async void AttackPlayer(object sender, EventArgs e)
        {
            await SendPacket("pvp", ((TransparentPicBox)sender).Tag.ToString());
            log($"Sending 'ATTACK_PLAYER' packet with arguments ['{((TransparentPicBox)sender).Tag.ToString()}']");
        }

        public delegate Task DoWarn(string n);

        public async Task DoWarning(string n)
        {
            if (InvokeRequired)
            {
                Invoke(new DoWarn(DoWarning), n);
            }
            else
            {
                Warning.ForeColor = Color.Red;
                Warning.Text = n;
                d = 1;
                rd = 500;
            }
        }

        public delegate Task UpdatePlayer(string name, int x, int y);

        public async Task MovePlayer(string name, int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdatePlayer(MovePlayer), name, x, y);
            }
            else
            {
                foreach (var p in playerEntities)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(p.Tag, name, false)))
                    {
                        p.Left = x;
                        p.Top = y;
                    }
                }
                
                foreach (var p in players)
                {
                    if ((p.Name ?? "") == (name ?? ""))
                    {
                        if (p.Location.X > x)
                            p.LastWalk = 0;
                        if (p.Location.X < x)
                            p.LastWalk = 1;
                        p.Location = new Point(x, y);
                        double t = DistanceBetween((Point)Normalize(p.Location), (Point)Normalize(localPlayer.Location));
                        if (t < 6)
                        {
                            nearestPlayer = p;
                        }
                    }
                }

                if (!Information.IsNothing(nearestPlayer))
                {
                    if (DistanceBetween((Point)Normalize(nearestPlayer.Location), (Point)Normalize(localPlayer.Location)) > 6)
                        nearestPlayer = null;
                }
                if (!My.MyProject.Forms.Gamesettings.CheckBox1.Checked) return;
                if (!Information.IsNothing(nearestPlayer))
                {
                    ButtonAttack.Show();
                }
                else
                {
                    ButtonAttack.Hide();
                }
            }
        }

        public double DistanceBetween(double x1, double y1, double x2, double y2)
        {
            //return Task<Math.Sqrt(Math.Pow(x2 - x1, 2d) + Math.Pow(y2 - y1, 2d))> ();
            return Math.Sqrt(Math.Pow(x2 - x1, 2d) + Math.Pow(y2 - y1, 2d));
        }

        public double DistanceBetween(Point p1, Point p2)
        {
            int x1 = p1.X;
            int x2 = p2.X;
            int y1 = p1.Y;
            int y2 = p2.Y;
            return Math.Sqrt(Math.Pow(x2 - x1, 2d) + Math.Pow(y2 - y1, 2d));
        }

        public Point Normalize(Point arg0)
        {
            int x;
            int y;
            x = Utils.IntDivide(arg0.X, 32);
            y = Utils.IntDivide(arg0.Y, 32);
            return new Point(x, y);
        }

        public delegate Task RemoveBlock(int x, int y);

        public async Task BreakBlock(int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new RemoveBlock(BreakBlock), x, y);
            }
            else
            {
                try
                {
                    for (int i = 0; i < blocks.Count; i++)
                    {
                        if (blocks.Count < i) break;
                        PictureBox b = blocks[i];
                        if ((b.Name ?? "") == ($"{x}B{y}" ?? ""))
                        {
                            blocks.Remove(b);
                            Controls.Remove(b);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    log(ex.ToString());
                }
            }
        }

        public class Block
        {
            string type;
            Rectangle rectangle;
            Point location;
            bool isBg;
            bool notSolid;

            public Block(string a, Rectangle b, bool bg, bool ns)
            {
                rectangle = b;
                location = b.Location;
                type = a;
                isBg = bg;
                notSolid = ns;
            } 
        }

        public Point GetZeroScreen()
        {
            return PointToScreen(new Point(0, 0));
        }

        public delegate Task RemovePlayer(string x);

        public async Task DelPlayer(string x)
        {
            if (InvokeRequired)
            {
                Invoke(new RemovePlayer(DelPlayer), x);
            }
            else
            {
                try
                {
                    foreach (var p in playerEntities)
                    {
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(p.Tag, x, false)))
                        {
                            playerEntities.Remove(p);
                            Controls.Remove(p);
                            break;
                        }
                    }

                    foreach (var p in players)
                    {
                        if ((p.Name ?? "") == (x ?? ""))
                        {
                            players.Remove(p);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    log(ex.ToString());
                }
            }
        }

        private void OnBlockClick(object sender, MouseEventArgs e)
        {
            // Send("block_break?" + (sender.Left / 32).ToString + "?" + (sender.Top / 32).ToString)
            if (e.Button == MouseButtons.Left)
            {
                BreakBlock(sender);
                return;
            }

            if (((Control)sender).Tag.ToString().Contains("furnace"))
            {
                if (e.Button == MouseButtons.Right)
                {
                    Control c = ((Control)sender);
                    SendPacket("furnace", c.Name.Split('B')[0], c.Name.Split('B')[1]);
                    log($"Sending 'USE_FURNACE' packet with arguments ['{c.Name.Split('B')[0]}', '{c.Name.Split('B')[1]}']");
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Control c = ((Control)sender);
                SendPacket("rightclick", c.Name.Split('B')[0], c.Name.Split('B')[1]);
                log($"Sending 'RIGHT_CLICK_BLOCK' packet with arguments ['{c.Name.Split('B')[0]}', '{c.Name.Split('B')[1]}']");
            }
            // MsgBox((sender.left / 32).ToString + " " + (sender.top / 32).ToString)
        }

        public event BlockEvent BlockPreBreakEvent;
        public event BlockEvent BlockBrokenEvent;

        private async void BreakBlock(object sender)
        {
            // Dim a As String() = {sender.Name.Split("B")(0), sender.Name.Split("B")(1)}
            // Invoke(New xSendPacket(AddressOf SendPacket), "block_break", a)
            BlockPreBreakEvent?.Invoke(((Control)sender).Location);
            await this.SendPacket("block_break", Operators.DivideObject(((Control)sender).Left + HorizontalScroll.Value, 32).ToString() + "?" + Operators.DivideObject(((Control)sender).Top + VerticalScroll.Value, 32).ToString()); // sender.Name.Split("B")(0), sender.Name.Split("B")(1))

            log($"Sending 'BLOCK_BREAK' packet with arguments ['{Operators.DivideObject(((Control)sender).Left + HorizontalScroll.Value, 32).ToString()}', '{Operators.DivideObject(((Control)sender).Top + VerticalScroll.Value, 32).ToString()}']");
            BlockBrokenEvent?.Invoke(((Control)sender).Location);                                                                                                                                    // Text = sender.Name.Split("B")(0) + " " + sender.Name.Split("B")(1)
        }

        public delegate Task xSendPacket(string packetType, string[] a);

        public async Task SendPacket(string packetType, params string[] a)
        {
            // Client.Send(Encode.Encrypt(packetType + "?" + String.Join("?", a)))
            await Send(packetType + "?" + string.Join("?", a));
        }

        public async Task SendSinglePacket(string packet)
        {
            await Send(packet);
        }

        public int walking = 0;

        private void moveThreadLoop1()
        {
            
        }

        private async void moveLoop()
        {
            while (true)
            {
                try
                {

                    Thread.Sleep(moveInterval);
                    if (!makeItDark.Visible && walking == 1)
                    {
                        localPlayer.Left -= 1;
                        bool collision = false;
                        try
                        {
                            //foreach (var b in blocks)
                            //{
                            //    if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) > 4) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) < -4) continue;
                            //    if (b.Top > localPlayer.Top + localPlayer.Height - 10)
                            //        continue;
                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            //    {
                            //        continue;
                            //    }

                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                            //        continue;
                            //    if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            //    {
                            //        collision = true;
                            //        break;
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                            log(ex.ToString());
                        }

                        if (collision)
                        {
                            localPlayer.Left += 1;
                        }
                        else
                        {
                            await UpdatePlayerPosition();
                        }
                    }
                    else if (!makeItDark.Visible && walking == 2)
                    {
                        localPlayer.Left += 1;
                        bool collision = false;
                        try
                        {
                            //foreach (var b in blocks)
                            //{
                            //    if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) > 4) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) < -4) continue;
                            //    if (b.Top > localPlayer.Top + localPlayer.Height - 10)
                            //        continue;
                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            //    {
                            //        continue;
                            //    }

                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                            //        continue;
                            //    if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            //    {
                            //        collision = true;
                            //        break;
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                            log(ex.ToString());
                        }

                        if (collision)
                        {
                            localPlayer.Left -= 1;
                        }
                        else
                        {
                            await UpdatePlayerPosition();
                        }
                    }

                    if (JumpStep > -1 && !makeItDark.Visible)
                    {
                        bool grounded = true;
                        JumpStep += 1;
                        localPlayer.Top -= 10;
                        if (JumpStep == 5)
                        {
                            JumpStep = -1;
                        }

                        try
                        {
                            //foreach (var b in blocks)
                            //{
                            //    if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) > 4) continue;
                            //    if ((b.Left / 32 - localPlayer.Left / 32) < -4) continue;
                            //    if (b.Top > localPlayer.Top + localPlayer.Height - 10)
                            //        continue;
                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            //    {
                            //        continue;
                            //    }

                            //    if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                            //        continue;
                            //    if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            //    {
                            //        grounded = true;
                            //        break;
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                            log(ex.ToString());
                        }

                        if (grounded)
                        {
                            await UpdatePlayerPosition();
                        }
                        else
                        {
                            localPlayer.Top += 10;
                            JumpStep = -1;
                        }
                    }
                } catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    log(ex.ToString());
                }
            }
        }

        public async Task UpdatePlayerPosition()
        {
            if (!IsBlink)
                await SendPacket("entityplayermove", (localPlayer.Left + HorizontalScroll.Value).ToString(), (localPlayer.Top + VerticalScroll.Value).ToString());
        }
        bool hideGui = false;
        bool fullscreen = false;
        //KEYWORD:KD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                My.MyProject.Forms.HelpWindow.Show();
                return;
            }

            if(e.KeyCode == Keys.F2)
            {
                hideGui = !hideGui;
                if(hideGui)
                {
                    progressBar1.Hide();
                    ProgressBar1.Hide();
                    ChatButton.Hide();
                    InventoryButton.Hide();
                    MenuButton.Hide();
                    debuginfo.Hide();
                } else {
                    progressBar1.Show();
                    ProgressBar1.Show();
                    ChatButton.Show();
                    InventoryButton.Show();
                    MenuButton.Show();
                }
                return;
            }

            if(e.KeyCode == Keys.F3)
            {
                debuginfo.Visible = !debuginfo.Visible;
                return;
            }

            if(e.KeyCode == Keys.F4)
            {
                //Bitmap screenGrab = new Bitmap(Bounds.Width, Bounds.Height);
                //DrawToBitmap(screenGrab, new Rectangle(Point.Empty, Size));
                //Clipboard.SetImage((Image)screenGrab);
                Size sz = ClientSize;
                if (e.Shift)
                {
                    int w = 0;
                    int h = 0;
                    foreach(PictureBox b in blocks)
                    {
                        if (b.Left + b.Width > w) w = b.Left + b.Width;
                        if (b.Top + b.Height > h) h = b.Top + b.Height;
                    }
                    sz = new Size(w, h);
                }
                Bitmap bmp = new Bitmap(sz.Width, sz.Height);
                using (Graphics G = Graphics.FromImage(bmp))
                {
                    G.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    // Rework screenshot system
                    
                    // Draw background
                    G.FillRectangle(new SolidBrush(BackColor), new Rectangle(Point.Empty, bmp.Size));

                    // Draw blocks
                    foreach(var b in blocks)
                    {
                        try
                        {
                            G.DrawImage(b.BackgroundImage, b.Bounds);
                        } catch(Exception)
                        {

                        }
                    }

                    // Draw local player
                    try
                    {
                        G.DrawImage(localPlayer.Image, localPlayer.Bounds);
                    }
                    catch (Exception)
                    {

                    }

                    // Draw local player's item in hand
                    try
                    {
                        G.DrawImage(R1.Image, R1.Bounds);
                    }
                    catch (Exception)
                    {

                    }

                    // Draw other players
                    foreach (var p in players)
                    {
                        try
                        {
                            G.DrawImage(p.Render.Image, p.Render.Bounds);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    // Draw other players' items in hand
                    foreach (var p in players)
                    {
                        try
                        {
                            G.DrawImage(p.R1.Image, p.R1.Bounds);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    // Draw entities
                    foreach(var id in entities.Keys)
                    {
                        try
                        {
                            G.DrawImage(entities[id].Renderer.Image, entities[id].Renderer.Bounds);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    G.DrawString($"Player name: {pName}\nNetcraft Version: {MainMenu.GetInstance().Ver}", new Font("Consolas", 12, FontStyle.Regular), new SolidBrush(Color.White), new Point(0, 0));
                }
                if (!Directory.Exists("./screenshots")) Directory.CreateDirectory("./screenshots");
                string path = $"./screenshots/{DateTime.Now.ToString().Replace(":", "-").Replace(".", "-")}.png";
                bmp.Save(path, ImageFormat.Png);
                log($"Screenshot saved at '{path}'");
                //if (FancyMessage.Show(lang.get("question.screenshot.open"), "", FancyMessage.Icon.Warning, FancyMessage.Buttons.OKCancel) == FancyMessage.Result.OK)
                //{
                //    Process.Start(path);
                //}
                Clipboard.SetImage((Image)bmp);
                return;
            }

            if(e.KeyCode == Keys.Enter)
            {
                if(e.Alt)
                {
                    fullscreen = !fullscreen;
                    if(fullscreen)
                    {
                        FormBorderStyle = FormBorderStyle.None;
                        WindowState = FormWindowState.Maximized;
                        log($"Full screen mode activated");
                    } else
                    {
                        FormBorderStyle = FormBorderStyle.FixedSingle;
                        WindowState = FormWindowState.Normal;
                        Size = new Size(1182, 615);
                        log($"Full screen mode deactivated");
                    }
                    Form1_Scroll(this, null);
                    return;
                }
            }

            if(e.KeyCode == Keys.C && !chatPanel1.Visible)
            {
                ChatButton.PerformClick();
                return;
            }

            if (NoClip)
            {
                if (e.KeyCode == Keys.D)
                {
                    localPlayer.Left += 2;
                    UpdatePlayerPosition();
                    lastWalk = 1;
                    return;
                }
                else if (e.KeyCode == Keys.A)
                {
                    localPlayer.Left -= 2;
                    lastWalk = 2;
                    UpdatePlayerPosition();
                    return;
                }
                else if (e.KeyCode == Keys.W)
                {
                    localPlayer.Top -= 2;
                    UpdatePlayerPosition();
                    return;
                }
                else if (e.KeyCode == Keys.S)
                {
                    localPlayer.Top += 2;
                    UpdatePlayerPosition();
                    return;
                }
                else if (e.KeyCode == Keys.E)
                {
                    Button1.PerformClick();
                    return;
                }
            }
            

            if (e.KeyCode == Keys.D)
            {
                walking = 2;
                lastWalk = 1;
                return;
            }
            else if (e.KeyCode == Keys.A)
            {
                walking = 1;
                lastWalk = 2;
                return;
            }
            else if (e.KeyCode == Keys.W)
            {
                if (JumpStep < 6)
                {
                    if (JumpStep > -1)
                        return;
                }

                JumpStep = 0;
                return;
            }
            else if (e.KeyCode == Keys.E)
            {
                Button1.PerformClick();
                return;
            }
            foreach(Keys k in keybinds.Keys)
            {
                if (e.KeyCode != k) continue;
                log($"Calling keybind function '{k.ToString()}'");
                keybinds[k]();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                walking = 0;
            }
            else if (e.KeyCode == Keys.A)
            {
                walking = 0;
            }
        }
        //private List<Panel> nearestBlocks = new List<Panel>();
        private void Ticker_Tick(object sender, EventArgs e)
        {
            //nearestBlocks.Clear();
            File.WriteAllText("./logs/" + logFileName, gamelog);

            Ticker.Stop();
            Ticker.Start();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public enum ProgressBarColor
        {
            Green = 0x1,
            Red = 0x2,
            Yellow = 0x3
        }

        private static void ChangeProgBarColor(ProgressBar ProgressBar_name, ProgressBarColor ProgressBar_Color)
        {
            SendMessage(ProgressBar_name.Handle, 0x410, (int)ProgressBar_Color, 0);
        }

        private int d = 60;
        private int rd = 15;
        private bool effectPlaying = false;
        string targetted = "";

        public async Task Tick()
        {
            if (d == 60)
            {
                if (Warning.ForeColor == Color.Yellow)
                {
                    Warning.ForeColor = Color.Red;
                }
                else
                {
                    Warning.ForeColor = Color.Yellow;
                }

                d -= 1;
            }
            else if (d != 0)
            {
                d -= 1;
            }
            else if (d == 0)
            {
                d = 60;
            }

            if (rd != 0)
            {
                rd -= 1;
            }
            else
            {
                try
                {
                    Warning.Text = "";
                    this.Text = "Netcraft " + MainMenu.GetInstance().Ver;
                } catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                    log(ex.ToString());
                }
            }

            try
            {
                Point playerPos = Normalize(localPlayer.Location);
                debuginfo.Text = $"Netcraft {MainMenu.GetInstance().Ver}\r\nServer IP: \"{ip}\"\r\nPlayer position: {playerPos.X.ToString()}, {playerPos.Y.ToString()}\r\nOnline players: {players.Count}\r\nAll blocks: {blocks.Count}\r\nTarget: [{targetted}]";
                if (!NoClip)
                {
                    bool collision = false;
                    try
                    {
                        blocks.Any(b =>
                        {
                            if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32) return false;
                            if ((b.Left / 32 - localPlayer.Left / 32) > 4) return false;
                            if ((b.Left / 32 - localPlayer.Left / 32) < -4) return false;
                            if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            {
                                return false;
                            }

                            if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                                return false;
                            if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            {
                                collision = true;
                                return true;
                            }
                            return false;
                        });
                    } catch(ArgumentOutOfRangeException)
                    {
                        //выходим из цикла
                        //хорошо это можно было без exceptionов
                    }
                    //foreach (var b in blocks)
                    //{
                        
                    //}

                    if (!collision)
                    {
                        localPlayer.Top += 1;
                        await UpdatePlayerPosition();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                log(ex.ToString());
            }
        }

        public bool IsCollision()
        {
            bool collision = false;
            foreach (var b in blocks)
            {
                if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32) continue;
                if ((b.Left / 32 - localPlayer.Left / 32) > 4) continue;
                if ((b.Left / 32 - localPlayer.Left / 32) < -4) continue;
                if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                {
                    continue;
                }

                if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                    continue;
                if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                {
                    collision = true;
                    break;
                }
            }
            return collision;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (chatPanel1.Visible) return;
            SendSinglePacket("update_inventory");
            invPanel.BringToFront();
            if(invPanel.Visible)
            {
                invPanel.Hide();
                Lighten();
            } else
            {
                Darker();
                invPanel.Show();
            }
        }

        private void ListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(ListBox1.SelectedItem, null, false)))
                {
                    SendPacket("splititems", ListBox1.SelectedItem.ToString());
                    log($"Sending 'SPLIT_ITEMS' packet with arguments ['{(ListBox1.SelectedItem).ToString()}']");
                }
            }
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(ListBox1.SelectedItem, null, false)))
                {
                    SendPacket("selectitem", ListBox1.SelectedItem.ToString());
                    log($"Sending 'SELECT_ITEM' packet with arguments ['{(ListBox1.SelectedItem).ToString()}']");
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                BlockPrePlaceEvent?.Invoke(new Point((e.X + HorizontalScroll.Value), (e.Y + VerticalScroll.Value)));
                SendPacket("block_place", (e.X + HorizontalScroll.Value).ToString(), (e.Y + VerticalScroll.Value).ToString());
                BlockPlacedEvent?.Invoke(new Point((e.X + HorizontalScroll.Value), (e.Y + VerticalScroll.Value)));
                log($"Sending 'BLOCK_PLACE' packet with arguments ['{(e.X + HorizontalScroll.Value).ToString()}', '{(e.Y + VerticalScroll.Value).ToString()}']");
            }
            else if (e.Button == MouseButtons.Middle)
            {
                BlockPrePlaceEventBg?.Invoke(new Point((e.X + HorizontalScroll.Value), (e.Y + VerticalScroll.Value)));
                SendPacket("block_place_bg", (e.X + HorizontalScroll.Value).ToString(), (e.Y + VerticalScroll.Value).ToString());
                BlockPlacedEventBg?.Invoke(new Point((e.X + HorizontalScroll.Value), (e.Y + VerticalScroll.Value)));
                log($"Sending 'BLOCK_PLACE_BG' packet with arguments ['{(e.X + HorizontalScroll.Value).ToString()}', '{(e.Y + VerticalScroll.Value).ToString()}']");
            }
        }

        public delegate void BlockEvent(Point point);
        public event BlockEvent BlockPrePlaceEvent;
        public event BlockEvent BlockPlacedEvent;
        public event BlockEvent BlockPrePlaceEventBg;
        public event BlockEvent BlockPlacedEventBg;

        public async Task SendPlace(int x, int y)
        {
            await SendPacket("block_place", (x + HorizontalScroll.Value).ToString(), (y + VerticalScroll.Value).ToString());
            log($"Sending 'BLOCK_PLACE' packet with arguments ['{(x + HorizontalScroll.Value).ToString()}', '{(y + VerticalScroll.Value).ToString()}']");
        }

        public async Task SendPlaceBack(int x, int y)
        {
            await SendPacket("block_place_bg", (x + HorizontalScroll.Value).ToString(), (y + VerticalScroll.Value).ToString());
            log($"Sending 'BLOCK_PLACE_BG' packet with arguments ['{(x + HorizontalScroll.Value).ToString()}', '{(y + VerticalScroll.Value).ToString()}']");
        }

        public int JumpStep { get; set; } = -1;

        private int lastWalk = 1;
        private Image ItemInImage;
        private Image ItemInImageFlipped;

        private async Task Test()
        {
            localPlayer.Update();
            if (lastWalk == 1)
            {
                localPlayer.Image = playerSkin;
            }
            else
            {
                localPlayer.Image = playerSkinFlip;
            }

            try
            {
                if (Information.IsNothing(ItemInImageFlipped))
                {
                    R1.Hide();
                    return;
                }

                if (Information.IsNothing(ItemInImage))
                {
                    R1.Hide();
                    return;
                }

                R1.Show();
                var lc = localPlayer.Location;
                if (ItemInImage.Equals(null))
                    return;
                if (lastWalk == 1)
                {
                    lc.X += localPlayer.Width - 5;
                    R1.Image = ItemInImage;
                }
                else
                {
                    lc.X -= R1.Width - 5;
                    R1.Image = ItemInImageFlipped;
                }
                //R1.Update();

                lc.Y = (int)(lc.Y + (45d - R1.Height / 2d));
                R1.Size = new Size(24, 24);
                R1.SizeMode = PictureBoxSizeMode.StretchImage;
                R1.BringToFront();
                R1.Location = lc;
            }
            catch (Exception ex)
            {
                R1.Hide();
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                log(ex.ToString());
                log($"Exception thrown:\r\n" + ex.ToString());
            }
        }

        

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Height = 619;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //My.MyProject.Forms.Chat.Show();
            if (invPanel.Visible) return;
            chatPanel1.BringToFront();
            if(!chatPanel1.Visible)
            {
                Darker();
                chatPanel1.Show();
            } else
            {
                Lighten();
                chatPanel1.Hide();
            }
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            EnableScroll();
            Timer3.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!connected)
            //{
            //    MainMenu.GetInstance().Show();
            //    if(toNotice != null)
            //    {
            //        MainMenu.GetInstance().notice(toNotice, toNoticeType);
            //    }
            //    My.MyProject.Forms.Chat.Close();
            //    MainMenu.GetInstance().presence.Details = "";
            //    MainMenu.GetInstance().presence.State = lang.get("rpc.menu");
            //    MainMenu.GetInstance().dRPC.SetPresence(MainMenu.GetInstance().presence);
            //    log($"Updating presence state to '', details to '{lang.get("rpc.menu")}'");
            //    leave();
            //    return;
            //}
            if(FancyMessage.Show(lang.get("question.exit"), "", FancyMessage.Icon.Warning, FancyMessage.Buttons.OKCancel) == FancyMessage.Result.OK)
            {
                StopServer();
                leave();
                MainMenu.Restart();

            } else
            {
                e.Cancel = true;
            }
        }

        public void StopServer()
        {
            if(IsSingleplayer)
            {
                if (!ServerProcess.HasExited) ServerProcess.Kill();
            }
        }

        internal void leave()
        {
            WriteChat("Client message: Вы вышли с сервера");
            if(cTicker != null) cTicker.Abort();
            foreach(PictureBox b in blocks)
            {
                Controls.Remove(b);
            }
            blocks.Clear();
            MainMenu.GetInstance().presence.State = "";
            MainMenu.GetInstance().presence.Details = lang.get("rpc.menu");
            MainMenu.GetInstance().dRPC.SetPresence(MainMenu.GetInstance().presence);
            log($"Updating presence state to '', details to '{lang.get("rpc.menu")}'");
            Hide();
            
            MainMenu.instance.Show();
            if(client != null && client.Connected) Disconnect();
        }

        public bool IsBlink { get; set; } = false;

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            IsBlink = !IsBlink;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            localPlayer.Location = new Point(239, 0);
            UpdatePlayerPosition();
        }

        private void localPlayer_LocationChanged(object sender, EventArgs e)
        {
        }

        private void Button3_MouseDown(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.A);
            Form1_KeyDown(this, ev);
        }

        private void Button3_MouseUp(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.A);
            Form1_KeyUp(this, ev);
        }

        private void Button6_MouseDown(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.D);
            Form1_KeyDown(this, ev);
        }

        private void Button6_MouseUp(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.D);
            Form1_KeyUp(this, ev);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            My.MyProject.Forms.Gamesettings.Show(this);
            if (IsSingleplayer)
            {
            }
            Update();
        }

        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (My.MyProject.Forms.Gamesettings.Visible)
            {
                My.MyProject.Forms.Gamesettings.Activate();
            }
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            //My.MyProject.Forms.Gamesettings.Move();
        }

        private void ButtonJump_MouseDown(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.W);
            Form1_KeyDown(this, ev);
        }

        private EntityPlayer nearestPlayer;

        private async void ButtonAttack_Click(object sender, EventArgs e)
        {
            if (!Information.IsNothing(nearestPlayer))
            {
                await AttackPlayer(nearestPlayer);
            }
            else
            {
                ButtonAttack.Hide();
            }
        }

        public async Task AttackPlayer(EntityPlayer arg0)
        {
            if (Information.IsNothing(arg0))
            {
                throw new NullReferenceException();
            }

            await SendPacket("pvp", arg0.Name);
            log($"Sending 'ATTACK_PLAYER' packet with arguments ['{arg0.Name}']");
        }

        private void localPlayer_Click(object sender, EventArgs e)
        {
        }

        private void localPlayer_Paint(object sender, PaintEventArgs e)
        {
            
        }

       

        private async void ListBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Information.IsNothing(ListBox2.SelectedItem))
            {
                await SendPacket("craft", Conversions.ToString(ListBox2.SelectedItem));
                log($"Sending 'CRAFT' packet with arguments ['{ListBox2.SelectedItem.ToString()}']");
            }
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            ListBox2.Visible = !ListBox2.Visible;
        }

        private void _localPlayer_Move(object sender, EventArgs e)
        {
            Test();
            
        }

        private void hScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {

        }

        public void ToggleButtonControls()
        {
            _ButtonLeft.Visible = !_ButtonLeft.Visible;
            _ButtonRight.Visible = !_ButtonRight.Visible;
            _ButtonAttack.Visible = !_ButtonAttack.Visible;
            _ButtonJump.Visible = !_ButtonJump.Visible;
        }

        //KEYWORD:SCRL
        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {
            InventoryButton.Location = new Point(Width - InventoryButton.Width - 30, 0);
            InventoryButton.BringToFront();
            MenuButton.Location = new Point(Width - MenuButton.Width - 30, 29);
            MenuButton.BringToFront();
            invPanel.Location = new Point(172, 79);
            ProgressBar1.Location = new Point(84, 0);
            progressBar1.Location = new Point(84, 25);
            progressBar1.BringToFront();
            ProgressBar1.BringToFront();
            ChatButton.Location = new Point(0, 0);
            ChatButton.BringToFront();
            makeItDark.Location = new Point(0, 0);
            //CraftButton.Location = new Point(0, 29);
            debuginfo.Location = new Point(5, 53);
            _Warning.Location = new Point(239, 0);
            _Warning.BringToFront();
            _ButtonLeft.Location = new Point(8, 503);
            _ButtonLeft.BringToFront();
            _ButtonRight.Location = new Point(84, 503);
            _ButtonRight.BringToFront(); 
            _ButtonJump.Location = new Point(1084, 500);
            _ButtonJump.BringToFront();
            _ButtonAttack.Location = new Point(1008, 500);
            _ButtonAttack.BringToFront();
            makeItDark.BringToFront();
            chatPanel1.Location = new Point(5, 124);
            chatPanel1.BringToFront();
            invPanel.BringToFront();
        }
        int effectPlayingDirection = -1;
        int d1 = 0;
        

        //KEYWORD:FRMPAINT
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = CreateGraphics();
            g.Clear(BackColor);
            //if (Debugger.IsAttached)
            //{
            //    for (int i = 1; i < 64; i++)
            //    {
            //        //Draw verticle line
            //        g.DrawLine(Pens.Red,
            //            (this.ClientSize.Width / 32) * i,
            //            0,
            //            (this.ClientSize.Width / 32) * i,
            //            this.ClientSize.Height);

            //        //Draw horizontal line
            //        g.DrawLine(Pens.Red,
            //            0,
            //            (this.ClientSize.Height / 32) * i,
            //            this.ClientSize.Width,
            //            (this.ClientSize.Height / 32) * i);
            //    }
            //}
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //Point p1 = new Point(localPlayer.Left + localPlayer.Width / 2, localPlayer.Top + 15);
            //Point p2 = PointToClient(Cursor.Position);
            //if(DistanceBetween(p1, p2) > 192)
            //{

            //    double dcur = DistanceBetween(p1, p2);
            //    var coff = dcur / 192;
            //    Point p21 = new Point(p2.X - p1.X, p2.Y - p1.Y);
            //    var p3 = new Point((int)(p21.X / coff), (int)(p21.Y / coff)) + (Size)p1;


            //    g.DrawLine(Pens.Yellow, p1, p3);
            //    g.DrawLine(Pens.Red, p2, p3);
            //} else
            //{
            //    g.DrawLine(Pens.Yellow, p1, p2);
            //}
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            targetted = Normalize(PointToClient(Cursor.Position)).ToString();
            Form1_Paint(this, new PaintEventArgs(CreateGraphics(), Bounds));
        }

        public Point ToClientPoint(Point point) => PointToClient(point);
        
        public static void Cmd(string label)
        {
            string[] args = label.Split(' ');
            string cmd = args[0].ToLower();
            if (cmd == "help")
            {
                Console.WriteLine("help => shows this list\r\nsetwalking <int: value> => set player walking");
                return;
            }
            if(cmd == "setwalking")
            {
                GetInstance().walking = int.Parse(args[1]);
                return;
            }
        }

        public static void consoleThread()
        {
            while(true)
            {
                Cmd(Console.ReadLine());
            }
        }

        int d6 = 5;
        public string Username { get; private set; }

        private void timer2_Tick(object sender, EventArgs e)
        {
            List<string> players = new List<string>();
            if (Username == null) return;
            players.Add(Username);

            this.players.All(p =>
            {
                players.Add(p.Name);
                return true;
            });

            timer2.Stop();
            timer2.Start();
        }

        public void sendEval(string text)
        {
            SendPacket("eval", text);
        }

        delegate void logResult(string res);
        string gamelog = "";
        public void log(string res)
        {
            if(InvokeRequired)
            {
                Invoke(new logResult(log), res);
            } else
            {
                try
                {
                    res = res.Replace("\r", "\n");
                    res = res.Replace("\n\n", "\n");
                    foreach(string a in res.Split('\n'))
                    {
                        gamelog += $"[{DateTime.Now.ToShortTimeString()}]: {a}\n";
                        NConsole.instance.winFormsConsole1.WriteLine($"[{DateTime.Now.ToShortTimeString()}]: {a}");
                    }
                    //NConsole.instance.richTextBox1.AppendText($"[{DateTime.Now.ToString()}]: {res}\r\n");
                    //NConsole.instance.richTextBox1.Select(NConsole.instance.richTextBox1.TextLength, 0);
                    //NConsole.instance.richTextBox1.ScrollToCaret();
                } catch(Exception)
                {

                }
            }
        }

        NConsole nConsole = null;
        private void debuginfo_Click(object sender, EventArgs e)
        {
            if (nConsole == null) nConsole = NConsole.instance;
            nConsole.Show();
            debuginfo.Hide();
            //panel1.Show();
            //panel1.Location = new Point(15, 15);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Lighten();
            invPanel.Hide();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Select(richTextBox1.TextLength, 0);
                richTextBox1.ScrollToCaret();
                ColorAA();
            } catch(Exception)
            {

            }
        }

        public delegate void PreChat(string text, ref bool cancel);
        public event PreChat PreChatEvent;
        public Dictionary<string, string> Stats { get; internal set; }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                bool c = false;
                PreChatEvent?.Invoke(textBox1.Text, ref c);


                if (!c)
                {
                    string text = textBox1.Text;
                    if(text.StartsWith("%") && IsSingleplayer)
                    {
                        await ServerProcess.StandardInput.WriteLineAsync(text.Substring(1));
                        textBox1.Clear();
                        return;
                    }
                    if(text == ".stats")
                    {
                        string st = "";
                        foreach(string k in Stats.Keys)
                        {
                            st += k + " = " + Stats[k] + "\n";
                        }
                        FancyMessage.Show(st.TrimEnd('\n'), "Player stats", FancyMessage.Icon.Info, FancyMessage.Buttons.OK);
                        textBox1.Clear();
                        return;
                    }
                    await Send("chat?" + textBox1.Text);
                }
                textBox1.Clear();
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            chatPanel1.Hide();
            Lighten();
            Focus();
        }

        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbghover;
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbg;
        }

        private void invPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void пасхалкажмиСюдаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://cdn.discordapp.com/attachments/789961408632586260/790309073425268776/pXbQ8xrEaXE.jpg");
        }

        private void _ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void debuginfo_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(RightToLeft == RightToLeft.Yes)
                {
                    RightToLeft = RightToLeft.No;
                    RightToLeftLayout = false;
                } else
                {
                    RightToLeft = RightToLeft.Yes;
                    RightToLeftLayout = true;
                }
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 5;
            await Tick();
        }

        private void richTextBox1_DoubleClick(object sender, EventArgs e)
        {

        }
        //private void Form1_MouseDown(object sender, Global.System.Windows.Forms.MouseEventArgs e)
        //{
        //    pt = new Point(e.X, e.Y); // запоминает положение курсора относительно формы
        //}

        //private void Form1_MouseMove(object sender, Global.System.Windows.Forms.MouseEventArgs e)
        //{
        //    if (e.Button == Windows.Forms.MouseButtons.Left) // Проверяем, нажата ли левая кнопка мыши 
        //    {
        //        Location = new Point(Location.X + e.X - pt.X, Location.Y + e.Y - pt.Y); // Меняем координаты формы в зависимости от положения курсора с учетом переменной pt
        //    }
        //}
    }

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
                a = Encoding.UTF8.GetBytes(Strings.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
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
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                return "";
            }
        }

        public static string e(string stringToEncrypt)
        {
            try
            {
                a = Encoding.UTF8.GetBytes(Strings.Left(Encoding.ASCII.GetString(Convert.FromBase64String(c)), 8));
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
                Console.WriteLine("Error: " + ex.Message + "\r\n" + ex.ToString());
                return "";
            }
        }
    }
}