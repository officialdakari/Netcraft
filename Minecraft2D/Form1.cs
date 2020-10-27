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

namespace Minecraft2D
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
            _Button1.Name = "Button1";
            _ListBox1.Name = "ListBox1";
            _Button2.Name = "Button2";
            _ProgressBar1.Name = "ProgressBar1";
            _localPlayer.Name = "localPlayer";
            _Warning.Name = "Warning";
            _ButtonLeft.Name = "ButtonLeft";
            _ButtonJump.Name = "ButtonJump";
            _ButtonRight.Name = "ButtonRight";
            _Button3.Name = "Button3";
            _ButtonAttack.Name = "ButtonAttack";
            _Button4.Name = "Button4";
            _ListBox2.Name = "ListBox2";
        }

        private TcpClient client;
        private StreamWriter sWriter;

        public delegate void OnMessageReceivedEventHandler(string msg);

        public delegate void _xUpdate(string str);

        private void Read(IAsyncResult ar)
        {
            try
            {
                var x = Encode.Decrypt(new StreamReader(client.GetStream()).ReadLine()).Split(Conversions.ToChar(Constants.vbLf));
                // Packet(x)
                handlePackets(x);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception ex)
            {
                // xUpdate("You have disconnecting from server")
                // Exit Sub
                Interaction.MsgBox(ex.ToString());
            }
        }

        public void handlePackets(string[] x)
        {
            foreach (var a in x)
                Packet(a);
        }

        public void Send(string str)
        {
            try
            {
                sWriter = new StreamWriter(client.GetStream());
                sWriter.WriteLine(Encode.Encrypt(str));
                sWriter.Flush();
            }
            catch (Exception ex)
            {
                Environment.Exit(0);
            }
        }
        /// <summary>
    /// Пытается подключиться к указанному серверу по указанному порту.
    /// </summary>
    /// <param name="ip">IP-адрес сервера</param>
    /// <param name="port">Порт сервера</param>

        public void Connect(string ip, int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback(Read), null);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Не удалось подключится к серверу!" + Constants.vbCrLf + Constants.vbCrLf + $"{ex.GetType().ToString()}: {ex.Message}");
                Close();
            }
        }

        public void Disconnect()
        {
            client.Client.Close();
            client = null;
        }

        private readonly List<Panel> blocks = new List<Panel>();
        private readonly List<PictureBox> playerEntities = new List<PictureBox>();
        private readonly List<EntityPlayer> players = new List<EntityPlayer>();
        internal DiscordRPC.DiscordRpcClient dRPC;
        internal DiscordRPC.RichPresence presence;
        private string pName;

        public bool IsOfficialServer { get; set; } = false;

        private Process _ServerProcess;

        public Process ServerProcess
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ServerProcess;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ServerProcess != null)
                {
                    _ServerProcess.OutputDataReceived -= onServerProcessDataReceived;
                    _ServerProcess.ErrorDataReceived -= onServerProcessErrorDataReceived;
                }

                _ServerProcess = value;
                if (_ServerProcess != null)
                {
                    _ServerProcess.OutputDataReceived += onServerProcessDataReceived;
                    _ServerProcess.ErrorDataReceived += onServerProcessErrorDataReceived;
                }
            }
        }

        internal string ip = "127.0.0.1";
        internal int port = 6575;

        private void onServerProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private void onServerProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine("[ERROR DATA RECEIVED]: " + e.Data);
        }

        public Image playerSkin { get; set; } = My.Resources.Resources.sprite;
        public Image playerSkinFlip { get; set; } = My.Resources.Resources.sprite;

        public enum Material
        {
            PLANKS, // DONE
            WOODEN_PICKAXE, // DONE
            WOODEN_SWORD, // DONE
            WOODEN_AXE, // DONE
            WOODEN_SHOVEL, // DONE
            STONE_PICKAXE, // DONE
            STONE_SWORD, // DONE
            STONE_AXE, // DONE
            STONE_SHOVEL, // DONE
            IRON_PICKAXE, // DONE
            IRON_SWORD, // DONE
            IRON_AXE, // DONE
            IRON_SHOVEL, // DONE
            DIAMOND_PICKAXE, // DONE
            DIAMOND_SWORD, // DONE
            DIAMOND_AXE, // DONE
            DIAMOND_SHOVEL, // DONE
            STICK, // DONE
            FURNACE, // DONE :D
            IRON, // Done
            GOLD, // Done
            DIAMOND, // Done
            IRON_BLOCK, // Done
            GOLD_BLOCK, // DONE!!!
            DIAMOND_BLOCK // Done :)
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
        private void Form1_Load(object sender, EventArgs e)
        {
            instance = this;
            dRPC = new DiscordRPC.DiscordRpcClient("763782798838071346");
            Button4.SendToBack();
            playerSkinFlip.RotateFlip(RotateFlipType.Rotate180FlipY);
            try
            {
                dRPC.Initialize();
            }
            catch (Exception ex)
            {
            }

            if (!Directory.Exists(@".\mods"))
            {
                Directory.CreateDirectory(@".\mods");
            }

            
            Button1.BringToFront();
            localPlayer.SendToBack();
            // RichTextBox1.SendToBack()
            // ListBox1.BackColor = Color.FromArgb(50, 0, 0, 0)
            R1.BringToFront();
            ListBox1.BringToFront();
            Text = "NetCraft " + My.MyProject.Forms.MainMenu.Ver;
            if (IsOfficialServer)
            {
                Text = $"NetCraft {My.MyProject.Forms.MainMenu.Ver} (Official Server)";
            }

            Connect(ip, port);
            if (My.MyProject.Forms.LoginForm1.ShowDialog() != DialogResult.OK)
                Environment.Exit(0);
            pName = My.MyProject.Forms.LoginForm1.UsernameTextBox.Text;
            SendPacket("setname", pName);
            Thread.Sleep(900);
            SendSinglePacket("world");
            My.MyProject.Forms.Chat.Show();
            try
            {
                if (dRPC.IsInitialized)
                {

                    presence = new DiscordRPC.RichPresence();
                    presence.Details = $"Имя игрока: {pName}";
                    if (!IsOfficialServer)
                    {
                        presence.State = "Играет в сетевой игре";
                    }
                    else
                    {
                        presence.State = "Играет на официальном сервере";
                    }

                    var pr = presence.WithAssets(new DiscordRPC.Assets()).WithParty(new DiscordRPC.Party()); // .WithSecrets(New DiscordRPC.Secrets())
                    pr.Assets.LargeImageKey = "logonc";
                    pr.Assets.LargeImageText = "NetCraft " + My.MyProject.Forms.MainMenu.Ver;
                    // dRPC.RegisterUriScheme(Nothing, Nothing)

                    dRPC.SetPresence(presence);
                    dRPC.Invoke();
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox("Не удалось загрузить Discord Rich Presence");
            }

            foreach (var i in Enum.GetNames(typeof(Material)))
                ListBox2.Items.Add(i.ToLower());
            cTicker = new Thread(tickLoop);
            cTicker.Start();
            mThread = new Thread(moveLoop);
            mThread.Start();
            foreach (var pluginPath in Directory.GetFiles(@".\mods"))
                PluginManager.Load(pluginPath);
        }

        public void tickLoop()
        {
            while (true)
            {
                Thread.Sleep(10);
                Tick();
            }
        }

        public Thread cTicker;
        private Thread mThread;

        private void OnJoinRequested()
        {
            Debug.WriteLine(Constants.vbCrLf + "Someone tried to join game" + Constants.vbCrLf);
        }

        public delegate void xChat(string arg0);

        public void WriteChat(string arg0)
        {
            if (InvokeRequired)
            {
                Invoke(new xChat(WriteChat), arg0);
                return;
            }

            My.MyProject.Forms.Chat.RichTextBox1.AppendText(arg0 + Constants.vbCrLf);
            My.MyProject.Forms.Chat.RichTextBox1.Select(My.MyProject.Forms.Chat.RichTextBox1.TextLength, 0);
            My.MyProject.Forms.Chat.RichTextBox1.ScrollToCaret();
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
                // SendMessage(ProgressBar1.Handle, 1040, 2, 0)
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

        public void Packet(string p)
        {
            var a = p.Split('?');
            // p = Encode.Decrypt(p)
            // MsgBox(p)
            if (a[0] == "blockchange")
            {
                var b = new Panel();
                bool g = false;
                // b.Parent = Me
                if (a.Length == 5)
                {
                    b.Tag = a[4];
                }

                b.Name = a[1] + "B" + a[2];
                b.Size = new Size(32, 32);
                // b.Left = a(1)
                // b.Top = a(2)
                b.Location = new Point(Conversions.ToInteger(a[1]) * 32, Conversions.ToInteger(a[2]) * 32);
                if (a[3] == "iron_ore")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.iron_ore;
                }
                else if (a[3] == "grass_block")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.grass_side;
                }
                else if (a[3] == "dirt")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.dirt1;
                }
                else if (a[3] == "stone")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.stone1;
                }
                else if (a[3] == "bedrock")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.bedrock;
                }
                else if (a[3] == "wood")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.log_oak;
                }
                else if (a[3] == "leaves")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.leaves;
                }
                else if (a[3] == "cobble")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.cobblestone4;
                }
                else if (a[3] == "planks")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.planks_oak;
                }
                else if (a[3] == "diamond_ore")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.diamond_ore;
                }
                else if (a[3] == "gold_ore")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.gold_ore;
                }
                else if (a[3] == "obsidian")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.obsidian1;
                }
                else if (a[3] == "water")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.water_still;
                }
                else if (a[3] == "sand")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.sand;
                }
                else if (a[3] == "glass")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.glass;
                }
                else if (a[3] == "furnace")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.furnace_front_off;
                }
                else if (a[3] == "iron_block")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.iron_block;
                }
                else if (a[3] == "diamond_block")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.diamond_block;
                }
                else if (a[3] == "gold_block")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.gold_block;
                    b.Tag += "furnace";
                }
                else if (a[3] == "coal_ore")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.coal_ore;
                }
                else if (a[3] == "sapling")
                {
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                    b.BackgroundImage = My.Resources.Resources.Grid_Sapling;
                }
                else
                {
                    b.BackColor = Color.FromName(a[3]);
                }

                b.Visible = true;
                b.Text = "";
                // MsgBox("Added block with color " + a(3) + " at " + a(1) + " " + a(2))
                blocks.Add(b);
                CreateBlock(b);
            }

            if (a[0] == "removeblock")
            {
                BreakBlock(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
            }

            if (a[0] == "addplayer")
            {
                CreatePlayer(a[1], 0, 0);
            }

            if (a[0] == "removeplayer")
            {
                DelPlayer(a[1]);
            }

            if (a[0] == "updateplayerposition")
            {
                MovePlayer(a[1], Conversions.ToInteger(a[2]), Conversions.ToInteger(a[3]));
            }

            if (a[0] == "startticking")
            {
                Ticker.Start();
            }

            if (a[0] == "teleport")
            {
                TeleportLocalPlayer(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
            }

            if (a[0] == "clearinventory")
            {
                Invoke(new MethodInvoker(ListBox1.Items.Clear));
            }

            if (a[0] == "additem")
            {
                AddItem(a[1]);
            }

            if (a[0] == "msgerror")
            {
                Interaction.MsgBox(string.Join("?", a.Skip(1).ToArray()), Constants.vbCritical);
            }

            if (a[0] == "msgwarn")
            {
                Interaction.MsgBox(string.Join("?", a.Skip(1).ToArray()), Constants.vbExclamation);
            }

            if (a[0] == "msg")
            {
                Interaction.MsgBox(string.Join("?", a.Skip(1).ToArray()), Constants.vbInformation);
            }

            if (a[0] == "chat")
            {
                // Invoke(New MethodInvoker1(AddressOf Chat.RichTextBox1.AppendText), String.Join("?", a.Skip(1).ToArray) + vbCrLf)
                // MsgBox(a(1))
                WriteChat(string.Join("?", a.Skip(1).ToArray()));
            }
            // SetText = p
            // Invoke(New MethodInvoker(AddressOf UpdateWindowText))
            // 'Exit Sub
            if (a[0] == "health")
            {
                SetHealth(Conversions.ToInteger(a[1]));
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
                DoWarning(string.Join("?", a.Skip(1).ToArray()));
            }

            if (a[0] == "sky")
            {
                BackColor = Color.FromName(a[1]);
                Update();
            }

            if (a[0] == "itemset")
            {
                try
                {
                    // REM - Деревянные инструменты
                    if (a[2] == "WOODEN_PICKAXE")
                    {
                        SetItem(a[1], My.Resources.Resources.WOODEN_PICKAXE, My.Resources.Resources.WOODEN_PICKAXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "WOODEN_AXE")
                    {
                        SetItem(a[1], My.Resources.Resources.WOODEN_AXE, My.Resources.Resources.WOODEN_AXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "WOODEN_SWORD")
                    {
                        SetItem(a[1], My.Resources.Resources.WOODEN_SWORD, My.Resources.Resources.WOODEN_SWORD_FLIPPED, a[2]);
                    }

                    if (a[2] == "WOODEN_SHOVEL")
                    {
                        SetItem(a[1], My.Resources.Resources.WOODEN_SHOVEL, My.Resources.Resources.WOODEN_SHOVEL_FLIPPED, a[2]);
                    }


                    // REM - Каменные инструменты
                    if (a[2] == "STONE_PICKAXE")
                    {
                        SetItem(a[1], My.Resources.Resources.STONE_PICKAXE, My.Resources.Resources.STONE_PICKAXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "STONE_AXE")
                    {
                        SetItem(a[1], My.Resources.Resources.STONE_AXE, My.Resources.Resources.STONE_AXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "STONE_SWORD")
                    {
                        SetItem(a[1], My.Resources.Resources.STONE_SWORD, My.Resources.Resources.STONE_SWORD_FLIPPED, a[2]);
                    }

                    if (a[2] == "STONE_SHOVEL")
                    {
                        SetItem(a[1], My.Resources.Resources.STONE_SHOVEL, My.Resources.Resources.STONE_SHOVEL_FLIPPED, a[2]);
                    }

                    // REM - Железные инструменты
                    if (a[2] == "IRON_PICKAXE")
                    {
                        SetItem(a[1], My.Resources.Resources.IRON_PICKAXE, My.Resources.Resources.IRON_PICKAXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "IRON_AXE")
                    {
                        SetItem(a[1], My.Resources.Resources.IRON_AXE, My.Resources.Resources.IRON_AXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "IRON_SWORD")
                    {
                        SetItem(a[1], My.Resources.Resources.IRON_SWORD, My.Resources.Resources.IRON_SWORD_FLIPPED, a[2]);
                    }

                    if (a[2] == "IRON_SHOVEL")
                    {
                        SetItem(a[1], My.Resources.Resources.IRON_SHOVEL, My.Resources.Resources.IRON_SHOVEL_FLIPPED, a[2]);
                    }

                    // REM - Алмазные инструменты
                    if (a[2] == "DIAMOND_PICKAXE")
                    {
                        SetItem(a[1], My.Resources.Resources.DIAMOND_PICKAXE, My.Resources.Resources.DIAMOND_PICKAXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "DIAMOND_AXE")
                    {
                        SetItem(a[1], My.Resources.Resources.DIAMOND_AXE, My.Resources.Resources.DIAMOND_AXE_FLIPPED, a[2]);
                    }

                    if (a[2] == "DIAMOND_SWORD")
                    {
                        SetItem(a[1], My.Resources.Resources.DIAMOND_SWORD, My.Resources.Resources.DIAMOND_SWORD_FLIPPED, a[2]);
                    }

                    if (a[2] == "DIAMOND_SHOVEL")
                    {
                        SetItem(a[1], My.Resources.Resources.DIAMOND_SHOVEL, My.Resources.Resources.DIAMOND_SHOVEL_FLIPPED, a[2]);
                    }

                    // REM - Блоки
                    if (a[2] == "WOOD")
                    {
                        SetItem(a[1], My.Resources.Resources.WOOD, My.Resources.Resources.WOOD, a[2]);
                    }

                    if (a[2] == "COBBLESTONE")
                    {
                        SetItem(a[1], My.Resources.Resources.COBBLESTONE, My.Resources.Resources.COBBLESTONE, a[2]);
                    }

                    if (a[2] == "STONE")
                    {
                        SetItem(a[1], My.Resources.Resources.STONE, My.Resources.Resources.STONE, a[2]);
                    }

                    if (a[2] == "PLANKS")
                    {
                        SetItem(a[1], My.Resources.Resources.WOOD, My.Resources.Resources.WOOD, a[2]);
                    }

                    if (a[2] == "DIRT")
                    {
                        SetItem(a[1], My.Resources.Resources.DIRT, My.Resources.Resources.DIRT, a[2]);
                    }

                    if (a[2] == "OBSIDIAN")
                    {
                        SetItem(a[1], My.Resources.Resources.OBSIDIAN, My.Resources.Resources.OBSIDIAN, a[2]);
                    }

                    if (a[2] == "SAND")
                    {
                        SetItem(a[1], My.Resources.Resources.SANDBLOCK, My.Resources.Resources.SANDBLOCK, a[2]);
                    }

                    if (a[2] == "GLASS")
                    {
                        SetItem(a[1], My.Resources.Resources.GLASSBLOCK, My.Resources.Resources.GLASSBLOCK, a[2]);
                    }

                    if (a[2] == "FURNACE")
                    {
                        SetItem(a[1], My.Resources.Resources.furnace_front_off, My.Resources.Resources.furnace_front_off, a[2]);
                    }

                    if (a[2] == "LEAVES")
                    {
                        SetItem(a[1], My.Resources.Resources.leaves, My.Resources.Resources.leaves, a[2]);
                    }

                    if (a[2] == "SAPLING")
                    {
                        SetItem(a[1], My.Resources.Resources.Grid_Sapling, My.Resources.Resources.Grid_Sapling, a[2]);
                    }

                    // REM - Драгоценные блоки
                    if (a[2] == "IRON_BLOCK")
                    {
                        SetItem(a[1], My.Resources.Resources.iron_block, My.Resources.Resources.iron_block, a[2]);
                    }

                    if (a[2] == "DIAMOND_BLOCK")
                    {
                        SetItem(a[1], My.Resources.Resources.diamond_block, My.Resources.Resources.diamond_block, a[2]);
                    }

                    if (a[2] == "GOLD_BLOCK")
                    {
                        SetItem(a[1], My.Resources.Resources.gold_block, My.Resources.Resources.gold_block, a[2]);
                    }

                    // REM - Разное
                    if (a[2] == "STICK")
                    {
                        SetItem(a[1], My.Resources.Resources.STICK, My.Resources.Resources.STICK, a[2]);
                    }

                    if (a[2] == "COAL")
                    {
                        SetItem(a[1], My.Resources.Resources.coal, My.Resources.Resources.coal, a[2]);
                    }

                    if (a[2] == "IRON")
                    {
                        SetItem(a[1], My.Resources.Resources.IRON, My.Resources.Resources.IRON, a[2]);
                    }

                    if (a[2] == "GOLD")
                    {
                        SetItem(a[1], My.Resources.Resources.GOLD, My.Resources.Resources.GOLD, a[2]);
                    }

                    if (a[2] == "DIAMOND")
                    {
                        SetItem(a[1], My.Resources.Resources.DIAMOND, My.Resources.Resources.DIAMOND, a[2]);
                    }

                    // REM - Nothing
                    if (a[2] == "nothing")
                    {
                        SetItem(a[1], null, null, a[2]);
                    }
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.ToString());
                }

                try
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int moveInterval = 30;

        public delegate void xsetSkyColor(Color a);

        public void SetSkyColor(Color a)
        {
            BackColor = a;
        }

        public delegate void xSetItem(string n, Image i, Image iflipped, string str);

        public void SetItem(string n, Image i, Image iflipped, string str)
        {
            if (InvokeRequired)
            {
                Invoke(new xSetItem(SetItem), n, i, iflipped, str);
            }
            else
            {
                if (n == "@")
                {
                    SetItemInHand(i, iflipped, str);
                    return;
                }

                foreach (var p in players)
                {
                    if ((p.Name ?? "") == (n ?? ""))
                    {
                        p.SetItemInHand(i, iflipped, str);
                    }
                }
            }
        }

        public void SetItemInHand(Image a, Image b, string c)
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

        public void UpdateWindowText()
        {
            Text = SetText;
        }

        public delegate void xAddItem(string s);

        public void AddItem(string s)
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

        public delegate void AddBlock(Panel b);

        public void ApplyBrightness(ref Bitmap bmp, int brightnessValue)
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
        }

        public void CreateBlock(Panel b)
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
                    if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                    {
                        // b.CreateGraphics.FillRectangle(New SolidBrush(Color.FromArgb(29, 0, 0, 0)), 0, 0, b.Width, b.Height)
                        // AddHandler b.Paint, AddressOf bgBlockPaint
                        // b.BackgroundImage = ColorProcessing(b.BackgroundImage, 0, 0, 0, 0, 0, -1, -1, 0)
                        var g = b.BackgroundImage.Clone();
                        Bitmap argbmp = (Bitmap)g;
                        ApplyBrightness(ref argbmp, -30);
                        b.BackgroundImage = (Image)g;
                    }
                }
                catch (Exception ex)
                {
                }

                if (b.Tag.ToString().Contains("furnace"))
                {
                    // Throw New Exception
                    // AddHandler b.MouseUp, AddressOf furnaceInteract
                }

                b.MouseDown += OnBlockClick;
                b.KeyDown += Form1_KeyDown;
                b.KeyUp += Form1_KeyUp;
                b.MouseEnter += blockMouseEnter;
                b.MouseLeave += blockMouseLeave;
            }
        }

        private void blockMouseEnter(object sender, EventArgs e)
        {
            if (!My.MyProject.Forms.Gamesettings.CheckBox2.Checked)
                return;
            Panel s = (Panel)sender;
            s.BorderStyle = BorderStyle.FixedSingle;
        }

        private void blockMouseLeave(object sender, EventArgs e)
        {
            if (!My.MyProject.Forms.Gamesettings.CheckBox2.Checked)
                return;
            Panel s = (Panel)sender;
            s.BorderStyle = BorderStyle.None;
        }

        public delegate void AddPlayer(string name, int x, int y);

        public void CreatePlayer(string name, int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new AddPlayer(CreatePlayer), name, x, y);
            }
            else
            {
                var b = new TransparentPicBox();
                Controls.Add(b);
                b.Tag = name;
                b.Image = playerSkin;
                b.SizeMode = PictureBoxSizeMode.StretchImage;
                b.Size = localPlayer.Size;
                b.Left = x;
                b.Top = y;
                b.BackColor = Color.Transparent;
                b.KeyDown += Form1_KeyDown;
                b.KeyUp += Form1_KeyUp;
                b.Click += AttackPlayer;
                ToolTip1.SetToolTip(b, "Игрок: " + name);
                playerEntities.Add(b);
                players.Add(new EntityPlayer(name, "", new Point(x, y), b));
            }
        }

        private void AttackPlayer(object sender, EventArgs e)
        {
            SendPacket("pvp", ((TransparentPicBox)sender).Tag.ToString());
        }

        public delegate void DoWarn(string n);

        public void DoWarning(string n)
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

        public delegate void UpdatePlayer(string name, int x, int y);

        public void MovePlayer(string name, int x, int y)
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
                        if (t < 5d)
                        {
                            if (!Information.IsNothing(nearestPlayer))
                            {
                                double u = DistanceBetween((Point)Normalize(nearestPlayer.Location), (Point)Normalize(localPlayer.Location));
                                if (t < u)
                                {
                                    nearestPlayer = p;
                                }
                            }
                            else
                            {
                                nearestPlayer = p;
                            }
                        }
                    }
                }

                if (!Information.IsNothing(nearestPlayer))
                {
                    if (DistanceBetween((Point)Normalize(nearestPlayer.Location), (Point)Normalize(localPlayer.Location)) > 5d)
                        nearestPlayer = null;
                }

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

        public object Normalize(Point arg0)
        {
            return new Point(arg0.X / 32, arg0.Y / 32);
        }

        public delegate void RemoveBlock(int x, int y);

        public void BreakBlock(int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new RemoveBlock(BreakBlock), x, y);
            }
            else
            {
                try
                {
                    foreach (var b in blocks)
                    {
                        if ((b.Name ?? "") == ($"{x}B{y}" ?? ""))
                        {
                            blocks.Remove(b);
                            Controls.Remove(b);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public delegate void RemovePlayer(string x);

        public void DelPlayer(string x)
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
                    SendPacket("furnace", Operators.DivideObject(((Control)sender).Left, 32).ToString(), Operators.DivideObject(((Control)sender).Top, 32).ToString());
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                SendPacket("rightclick", Operators.DivideObject(((Control)sender).Left, 32).ToString(), Operators.DivideObject(((Control)sender).Top, 32).ToString());
            }
            // MsgBox((sender.left / 32).ToString + " " + (sender.top / 32).ToString)
        }

        private void BreakBlock(object sender)
        {
            // Dim a As String() = {sender.Name.Split("B")(0), sender.Name.Split("B")(1)}
            // Invoke(New xSendPacket(AddressOf SendPacket), "block_break", a)
            this.SendPacket("block_break", Operators.DivideObject(((Control)sender).Left, 32).ToString() + "?" + Operators.DivideObject(((Control)sender).Top, 32).ToString()); // sender.Name.Split("B")(0), sender.Name.Split("B")(1))
                                                                                                                                                          // Text = sender.Name.Split("B")(0) + " " + sender.Name.Split("B")(1)
        }

        public delegate void xSendPacket(string packetType, string[] a);

        public void SendPacket(string packetType, params string[] a)
        {
            // Client.Send(Encode.Encrypt(packetType + "?" + String.Join("?", a)))
            Send(packetType + "?" + string.Join("?", a));
        }

        public void SendSinglePacket(string packet)
        {
            Send(packet);
        }

        public int walking = 0;

        private void moveLoop()
        {
            while (true)
            {
                if (My.MyProject.Forms.Gamesettings.Visible)
                {
                    CreateGraphics().FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), 0, 0, Width, Height);
                }

                Thread.Sleep(moveInterval);
                if (walking == 1)
                {
                    localPlayer.Left -= 1;
                    bool collision = false;
                    try
                    {
                        foreach (var b in blocks)
                        {
                            if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            {
                                if (b.Top > localPlayer.Top + localPlayer.Height - 5)
                                    continue;
                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                                {
                                    continue;
                                }

                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                                    continue;
                                collision = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (collision)
                    {
                        localPlayer.Left += 1;
                    }
                    else
                    {
                        UpdatePlayerPosition();
                    }
                }
                else if (walking == 2)
                {
                    localPlayer.Left += 1;
                    bool collision = false;
                    try
                    {
                        foreach (var b in blocks)
                        {
                            if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            {
                                if (b.Top > localPlayer.Top + localPlayer.Height - 5)
                                    continue;
                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                                {
                                    continue;
                                }

                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                                    continue;
                                collision = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (collision)
                    {
                        localPlayer.Left -= 1;
                    }
                    else
                    {
                        UpdatePlayerPosition();
                    }
                }

                if (JumpStep > -1)
                {
                    bool collision = false;
                    JumpStep += 1;
                    localPlayer.Top -= 10;
                    if (JumpStep == 5)
                    {
                        JumpStep = -1;
                    }

                    try
                    {
                        foreach (var b in blocks)
                        {
                            if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32)
                                continue;
                            if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                            {
                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                                {
                                    continue;
                                }

                                if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                                    continue;
                                if (b.Top + 5 > localPlayer.Top)
                                    continue;
                                collision = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Stop
                    }

                    if (!collision)
                    {
                        UpdatePlayerPosition();
                    }
                    else
                    {
                        localPlayer.Top += 10;
                        JumpStep = -1;
                    }
                }
            }
        }

        public void UpdatePlayerPosition()
        {
            if (!IsBlink)
                SendPacket("entityplayermove", localPlayer.Left.ToString(), localPlayer.Top.ToString());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                My.MyProject.Forms.HelpWindow.Show();
            }

            if (NoClip)
            {
                if (e.KeyCode == Keys.D)
                {
                    localPlayer.Left += 2;
                    UpdatePlayerPosition();
                }
                else if (e.KeyCode == Keys.A)
                {
                    localPlayer.Left -= 2;
                    UpdatePlayerPosition();
                }
                else if (e.KeyCode == Keys.W)
                {
                    localPlayer.Top -= 2;
                    UpdatePlayerPosition();
                }
                else if (e.KeyCode == Keys.S)
                {
                    localPlayer.Top += 2;
                    UpdatePlayerPosition();
                }
                else if (e.KeyCode == Keys.E)
                {
                    Button1.PerformClick();
                }

                return;
            }

            if (e.Control)
            {
                moveInterval = 10;
            }
            else
            {
                moveInterval = 30;
            }

            if (e.KeyCode == Keys.D)
            {
                walking = 2;
                lastWalk = 1;
            }
            else if (e.KeyCode == Keys.A)
            {
                walking = 1;
                lastWalk = 2;
            }
            else if (e.KeyCode == Keys.W)
            {
                if (JumpStep < 6)
                {
                    if (JumpStep > -1)
                        return;
                }

                JumpStep = 0;
            }
            else if (e.KeyCode == Keys.E)
            {
                Button1.PerformClick();
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

        private void Ticker_Tick(object sender, EventArgs e)
        {
            // If Visible Then Tick()
            // RichTextBox1.BackColor = Color.FromArgb(0, 0, 0, 0)
            Ticker.Stop();
            // Ticker.Start()
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

        public void Tick()
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
                Warning.Text = "";
            }

            try
            {
                if (!NoClip)
                {
                    bool collision = false;
                    foreach (var b in blocks)
                    {
                        if (DistanceBetween(b.Location, localPlayer.Location) > 5 * 32)
                            continue;
                        if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                        {
                            if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            {
                                continue;
                            }

                            if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                                continue;
                            collision = true;
                            break;
                        }
                    }

                    if (!collision)
                    {
                        localPlayer.Top += 1;
                        UpdatePlayerPosition();
                    }
                }
            }
            catch (Exception ex)
            {
                // Stop
            }
        }

        public bool IsCollision()
        {
            bool collision = false;
            foreach (var b in blocks)
            {
                if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                {
                    if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                        continue;
                    if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                        continue;
                    collision = true;
                    break;
                }
            }

            return collision;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SendSinglePacket("update_inventory");
            ListBox1.Visible = !ListBox1.Visible;
            if (ListBox1.Visible)
            {
            }
            // RichTextBox1.Hide()
            // Button2.Hide()
            // TextBox1.Hide()
            else
            {
                // RichTextBox1.Show()
                // Button2.Show()
            }
        }

        private void ListBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectNotEqual(ListBox1.SelectedItem, null, false)))
                SendPacket("selectitem", ListBox1.SelectedItem.ToString());
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SendPacket("block_place", e.X.ToString(), e.Y.ToString());
            }
            else if (e.Button == MouseButtons.Middle)
            {
                SendPacket("block_place_bg", e.X.ToString(), e.Y.ToString());
            }
        }

        public int JumpStep { get; set; } = -1;

        private int lastWalk = 1;
        private Image ItemInImage;
        private Image ItemInImageFlipped;

        private void Test()
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
                    lc.X += localPlayer.Width - 10;
                    R1.Image = ItemInImage;
                }
                else
                {
                    lc.X -= R1.Width - 10;
                    R1.Image = ItemInImageFlipped;
                }

                lc.Y = (int)(lc.Y + (55d - R1.Height / 2d));
                R1.Size = new Size(32, 32);
                R1.SizeMode = PictureBoxSizeMode.StretchImage;
                R1.BringToFront();
                R1.Location = lc;
            }
            catch (Exception ex)
            {
                R1.Hide();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (Conversions.ToString(JumpStep < 5) + (JumpStep > -1) == "TrueTrue")
            {
                if (JumpStep == -1)
                {
                    Timer2.Stop();
                    return;
                }

                bool collision = false;
                localPlayer.Top -= 10;
                foreach (var b in blocks)
                {
                    if (b.Bounds.IntersectsWith(localPlayer.Bounds))
                    {
                        if (b.Top > localPlayer.Top)
                            continue;
                        if (Conversions.ToBoolean(b.Tag.ToString().Contains("non-solid")))
                            continue;
                        if (Conversions.ToBoolean(b.Tag.ToString().Contains("bg")))
                            continue;
                        collision = true;
                        break;
                    }
                }

                if (!collision)
                {
                    UpdatePlayerPosition();
                }
                else
                {
                    localPlayer.Top += 10;
                }

                JumpStep += 1;
                Timer2.Stop();
                Timer2.Start();
                return;
            }
            else
            {
                JumpStep = -1;
            }

            Timer2.Stop();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            Height = 619;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // TextBox1.Visible = Not TextBox1.Visible
            My.MyProject.Forms.Chat.Show();
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            // Label1.ForeColor = MainMenu.rainbow
            Timer3.Stop();
            // Timer3.Start()
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            // StopServer()

            dRPC.ClearPresence();
            Environment.Exit(0);
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
            if (IsOfficialServer)
            {
                ProcessSuspend.SuspendProcess(ServerProcess);
                Ticker.Stop();
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
            My.MyProject.Forms.Gamesettings.Move();
        }

        private void ButtonJump_MouseDown(object sender, MouseEventArgs e)
        {
            var ev = new KeyEventArgs(Keys.W);
            Form1_KeyDown(this, ev);
        }

        private EntityPlayer nearestPlayer;

        private void ButtonAttack_Click(object sender, EventArgs e)
        {
            if (!Information.IsNothing(nearestPlayer))
            {
                AttackPlayer(nearestPlayer);
            }
            else
            {
                ButtonAttack.Hide();
            }
        }

        public void AttackPlayer(EntityPlayer arg0)
        {
            if (Information.IsNothing(arg0))
            {
                throw new NullReferenceException();
            }

            SendPacket("pvp", arg0.Name);
        }

        private void localPlayer_Click(object sender, EventArgs e)
        {
        }

        private void localPlayer_Paint(object sender, PaintEventArgs e)
        {
            var r = new Rectangle(0, 0, localPlayer.Width, localPlayer.Height);
            Bitmap i = (Bitmap)My.Resources.Resources.Player1Texture.Clone();
            i.MakeTransparent();
            localPlayer.CreateGraphics().DrawImage(i, r);
        }

       

        private void ListBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Information.IsNothing(ListBox2.SelectedItem))
            {
                SendPacket("craft", Conversions.ToString(ListBox2.SelectedItem));
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

        

    }

    public class Encode
    {
        private static byte[] key = new byte[] { 62, 59, 25, 19, 37 };
        private static readonly byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        internal const string EncryptionKey = "81iSifdf"; // "HOMECLOUD" & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString ' & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                var inputByteArray = new byte[stringToDecrypt.Length + 1];
                key = Encoding.UTF8.GetBytes(Strings.Left(EncryptionKey, 8));
                var des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                // oops - add your exception logic
                // MsgBox("ошибка")
                return "";
            }
        }

        public static string Encrypt(string stringToEncrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(Strings.Left(EncryptionKey, 8));
                var des = new DESCryptoServiceProvider();
                var inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                // oops - add your exception logic
                // MsgBox("ошибка")
                return "";
            }
        }
    }
}