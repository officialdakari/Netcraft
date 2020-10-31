using System;
using System.Diagnostics;
using System.Drawing;
using global::System.IO;
using System.Linq;
using global::System.Net.Sockets;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text;
using System.Collections.Specialized;
using System.Threading;

namespace Minecraft2D
{
    public partial class MainMenu
    {
        public MainMenu()
        {
            InitializeComponent();
            _Button1.Name = "Button1";
            _Button3.Name = "Button3";
            _Button4.Name = "Button4";
            _Button2.Name = "Button2";
            _Button5.Name = "Button5";
        }

        private Random rnd = new Random();
        public Color rainbow = Color.Red;
        private int delayA = 20;
        private Pinger currentPinger;
        protected int labelPos = 0;
        protected int labelDirection = 1;
        protected int labelChangeDelay = 0;
        protected int labelCurDelay = 0;
        protected int textPos = 0;
        Lang lang;
        protected bool labelWithCur = true;
        protected int indexPos = 0;
        protected int direction = 1;
        protected Color[] colors = new[] { Color.Red, Color.Orange, Color.Goldenrod, Color.Gold, Color.Yellow, Color.GreenYellow, Color.LightGreen, Color.Green, Color.LightBlue, Color.Blue, Color.DarkBlue, Color.BlueViolet, Color.Violet };

        protected string[] strings = {"Not affiliated with Mojang Studios or Microsoft.", "Netcraft Is In 2D", "By GladCypress3030 and TheNonamee", "Converted to C#", "Join our Discord!"};
        protected string labelText;

        private void SetTitle()
        {
            labelText = strings[0];
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (labelCurDelay == 0)
            {
                labelWithCur = !labelWithCur;
                labelCurDelay = 6;
            }
            else
            {
                labelCurDelay -= 1;
            }
            
            string t = "> " + Strings.Left(labelText, labelPos) + (labelWithCur ? "[]" : "");
            if (labelChangeDelay == 0)
            {
                labelPos += labelDirection;
                if (labelPos == 0)
                {
                    labelDirection = 1;
                    textPos++;
                    if (textPos == strings.Length) textPos = 0;
                    labelText = strings[textPos];
                    labelChangeDelay = 20;
                }
                else if (labelPos == labelText.Length)
                {
                    labelDirection = -1;
                    labelChangeDelay = 20;
                    if (textPos == strings.Length) textPos = 0;
                }
                else
                {
                    labelChangeDelay = 2;
                }

            }
            else
            {
                labelChangeDelay -= 1;
            }

            Label3.Text = t;
            rainbow = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            Button1.FlatAppearance.BorderColor = Color.Red;
            Timer1.Stop();
            Timer1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!My.MyProject.Computer.Network.IsAvailable)
            {
                Interaction.MsgBox("Нет подключения к Интернету :(");
                return;
            }
            if(blocklist.Contains(TextBox1.Text.ToLower()))
            {
                FancyMessage.Show(lang.get("text.error.blocked_server"), "Netcraft", FancyMessage.Icon.Error);
                return;
            }
            My.MyProject.Forms.Form1.ip = TextBox1.Text;
            My.MyProject.Forms.Form1.Show();
            Hide();
        }

        public readonly string Ver = "1.3-ALPHA";
        internal static MainMenu instance;
        public static MainMenu GetInstance()
        {
            return instance;
        }

        private void MainMenu_LocationChanged(object sender, EventArgs e)
        {
            // CenterToScreen()
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/BuKCBP8");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCnsheKRiwNM4xQLDs35HK8w");
        }

        private bool isModified = false;
        Client client;
        StringCollection blocklist = new StringCollection();
        internal DiscordRPC.DiscordRpcClient dRPC;
        internal DiscordRPC.RichPresence presence;

        private void MainMenu_Load(object sender, EventArgs e)
        {
            instance = this;

            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            DoLang();
            SetTitle();
            dRPC = new DiscordRPC.DiscordRpcClient("763782798838071346");
            try
            {
                dRPC.Initialize();
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (dRPC.IsInitialized)
                {

                    presence = new DiscordRPC.RichPresence();
                    presence.Details = lang.get("rpc.menu");
                    

                    var pr = presence.WithAssets(new DiscordRPC.Assets()).WithParty(new DiscordRPC.Party());
                    pr.Assets.LargeImageKey = "logonc";
                    pr.Assets.LargeImageText = "NetCraft " + My.MyProject.Forms.MainMenu.Ver;

                    dRPC.SetPresence(presence);
                    dRPC.Invoke();
                }
            }
            catch (Exception ex)
            {
                FancyMessage.Show(lang.get("rpc.failed"));
            }
            Label5.Text = $"Netcraft {Ver}";
            CheckForIllegalCrossThreadCalls = false;
            foreach (Control c in Controls)
                c.KeyDown += OnKey;
            _Timer1.Start();
        }

        private void packetreceived(string p)
        {
            string[] u = p.Split('?');
            if(u[0] == "msg")
            {
                FancyMessage.Show(String.Format(lang.get(u[1]), (object[])u.Skip(2).ToArray()));
            }
            if(u[0]=="name")
            {
                string n = Utils.InputBox("net.requestname");
                while (n == null)
                {
                    n = Utils.InputBox("net.requestname");
                }
                client.f("name?" + n);
                Thread.Sleep(30);
                client.f("blacklist");
            }
            if(u[0]=="bl")
            {
                foreach(string i in u.Skip(1).ToArray())
                {
                    blocklist.Add(i.ToLower());
                }
            }
        }

        private void DoLang()
        {
            _Button2.Text = lang.get("menu.button.official_server");
            _Button1.Text = lang.get("menu.button.network_game");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/BuKCBP8");
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            if (!My.MyProject.Computer.Network.IsAvailable)
            {
                Interaction.MsgBox("Нет подключения к Интернету :(");
                return;
            }

            My.MyProject.Forms.Form1.IsOfficialServer = true;
            My.MyProject.Forms.Form1.ip = "mcblockmine.ddns.net";
            My.MyProject.Forms.Form1.Show();
            Hide();
        }

        public void StartSingleplayer()
        {
            My.MyProject.Forms.Form1.IsOfficialServer = true;
            My.MyProject.Forms.Form1.ServerProcess = new Process();
            {
                var withBlock = My.MyProject.Forms.Form1.ServerProcess;
                withBlock.EnableRaisingEvents = true;
                withBlock.StartInfo.Arguments = "s";
                withBlock.StartInfo.UseShellExecute = false;
                withBlock.StartInfo.RedirectStandardError = true;
                withBlock.StartInfo.RedirectStandardOutput = true;
                withBlock.StartInfo.FileName = Application.StartupPath + @"\server\NetcraftServer.exe";
                withBlock.Start();
                withBlock.BeginErrorReadLine();
                withBlock.BeginOutputReadLine();
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            currentPinger = new Pinger();
            currentPinger.Connect(TextBox1.Text, 6575);
            currentPinger.OnPingComplete += _onPingComplete;
            currentPinger.Send("ping");
        }

        private delegate void onPingComplete();

        private void _onPingComplete()
        {
            if(InvokeRequired)
            {
                Invoke(new onPingComplete(_onPingComplete));
                return;
            }
            {
                var withBlock = currentPinger;
                LabelName.Text = withBlock.Name;
                LabelDesc.Text = withBlock.Motd;
                LabelPlayers.Text = withBlock.Players;
                Panel1.Show();
                withBlock.Disconnect();
            }
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                My.MyProject.Forms.HelpWindow.Show();
            }
        }

        

        int clicks;

        private void Label3_MouseClick(object sender, MouseEventArgs e)
        {
            clicks++;
            if(clicks == 5)
            {
                FancyMessage.Show("Зачем кликать по этой штуке тем более 10 раз?");
            }
        }

        int firstLoad = 1;

        private void MainMenu_VisibleChanged(object sender, EventArgs e)
        {
            if(Visible)
            {
                if(firstLoad == 1)
                {
                    firstLoad = 0;
                    return;
                }
                Form1.GetInstance().Dispose();
            }
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FancyMessage.Show(lang.get("text.question.confirm_exit"), "Netcraft", FancyMessage.Icon.Info, FancyMessage.Buttons.OKCancel) == FancyMessage.Result.OK)
            {
                Environment.Exit(0);
            } else
            {
                e.Cancel = true;
            }
        }
    }

    public class Pinger
    {
        public string Name { get; set; }
        public string Motd { get; set; }
        public string Players { get; set; }

        private TcpClient client;
        private StreamWriter sWriter;

        public delegate void _xUpdate(string str);

        public event OnPingCompleteEventHandler OnPingComplete;

        public delegate void OnPingCompleteEventHandler();

        private void Read(IAsyncResult ar)
        {
            try
            {
                string x = Encode.d(new StreamReader(client.GetStream()).ReadLine());
                Packet(x);
                client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, Read, null);
            }
            catch (Exception ex)
            {
                // Throw ex
                // xUpdate("You have disconnecting from server")
                // Exit Sub
                // MsgBox(ex.ToString)
            }
        }

        public void Packet(string x)
        {
            var a = x.Split('?');
            if (a[0] == "name")
            {
                Name = string.Join("?", a.Skip(1).ToArray());
            }

            if (a[0] == "motd")
            {
                Motd = string.Join("?", a.Skip(1).ToArray());
            }

            if (a[0] == "players")
            {
                Players = string.Join("?", a.Skip(1).ToArray());
            }

            if (Name is object)
            {
                if (Motd is object)
                {
                    if (Players is object)
                    {
                        OnPingComplete?.Invoke();
                    }
                }
            }
        }

        public void Send(string str)
        {
            try
            {
                sWriter = new StreamWriter(client.GetStream());
                sWriter.WriteLine(Encode.e(str));
                sWriter.Flush();
            }
            catch (Exception ex)
            {
                // Throw ex
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
                Interaction.MsgBox("Не удалось Ping сервер!" + Constants.vbCrLf + Constants.vbCrLf + $"{ex.GetType().ToString()}: {ex.Message}");
                // Close()
            }
        }

        public void Disconnect()
        {
            client.Client.Close();
            client = null;
        }
    }
}