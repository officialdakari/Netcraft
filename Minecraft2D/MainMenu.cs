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
using System.Threading.Tasks;
using DiscordRPC;
using System.Runtime.InteropServices;

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

        protected string[] strings = {"Not affiliated with Mojang Studios or Microsoft.", "Happy new year!", "Press F1 for help!", "Netcraft Is In 2D", "By DarkCoder15 and TheNonameee", "Converted to C#", "Join our Discord!", "There aren't too many bugs here..." };
        protected string labelText;
        protected int presenceUpdateDelay = 20;

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

            if(presenceUpdateDelay == 0)
            {
                presenceUpdateDelay = 20;
                dRPC.SetPresence(presence);
            } else
            {
                presenceUpdateDelay--;
            }
            
             
            string t = "[Netcraft:netcraft] " + Strings.Left(labelText, labelPos) + (labelWithCur ? "\u258C" : "");
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
                Interaction.MsgBox("No internet connection!");
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

        public readonly string Ver = "0.1.7a";
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
        // 0 = ignore, exit, 1 = exit, 2 = ignore
        public void notice(string t, int type = 0)
        {
            panel3.Dock = DockStyle.Fill;
            panel3.Show();
            panel3.BringToFront();
            textBox2.Text = t;
            if(type == 0)
            {
                button1.Show();
                button2.Show();
            }
            if(type == 1)
            {
                button1.Hide();
                button2.Show();
                presence.State = "Game is crashed";
            }
            if(type == 2)
            {
                button1.Show();
                button2.Hide();
            }
            panel2.Hide();
            My.MyProject.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Hand);
        }

        void OnUnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            Form1.GetInstance().toNotice = $"OOPS, NETCRAFT IS CRASHED...\r\n\r\nAn unhandled exception in Netcraft occured.\r\n\r\nInformation:\r\n[Game version] {MainMenu.GetInstance().Ver}\r\n[Your OS: Platform] {Environment.OSVersion.Platform}\r\n[Your OS: ServicePack] {Environment.OSVersion.ServicePack}\r\n[Your OS: Version] {Environment.OSVersion.VersionString}\r\n" +
                "\r\nException information\r\n" + e.Exception.ToString();
            
            Form1.GetInstance().toNoticeType = 1;
            Form1.GetInstance().Close();
        }

        void comment(string a)
        {
            
        }

        string cfg;
        Client _client;
        private void MainMenu_Load(object sender, EventArgs e)
        {
            instance = this;
            string[] p = Process.GetCurrentProcess().MainModule.FileName.Split(Path.DirectorySeparatorChar);
            p[p.Length - 1] = "";
            Directory.SetCurrentDirectory(string.Join(Path.DirectorySeparatorChar.ToString(), p));
            button1 = _Button1;
            Button1 = _Button1; 
            comment("ты чё декомпилировал игру быстро удаляй декомпилированный код иначе бан");
            Form1.instance = My.MyProject.Forms.Form1;
            Application.ThreadException += My.MyApplication.threadException;
            AppDomain.CurrentDomain.UnhandledException += My.MyApplication.threadException;
            cfg = File.ReadAllText("./options.txt", Encoding.UTF8);
            Thread uTh = new Thread(() =>
            {
                while(true)
                {
                    Thread.Sleep(1000);
                    dRPC.SetPresence(presence);
                    dRPC.Invoke();
                }
            });
            Utils.LANGUAGE = Utils.GetValue("lang", cfg, "english");
            lang = Lang.FromFile($"./lang/{Utils.LANGUAGE}.txt");
            DoLang();
            SetTitle();
            dRPC = new DiscordRPC.DiscordRpcClient("763782798838071346");
            try
            {
                dRPC.RegisterUriScheme(executable: Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                dRPC.Initialize();

                presence = new DiscordRPC.RichPresence();

                //presence = presence.WithSecrets(new DiscordRPC.Secrets()
                //{
                //    JoinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= "
                //});
                //presence.Party = new Party()
                //{
                //    ID = Secrets.CreateFriendlySecret(new Random()),
                //    Size = 1,
                //    Max = 5
                //};
                presence.State = lang.get("rpc.menu");
                var pr = presence.WithAssets(new DiscordRPC.Assets()).WithParty(new DiscordRPC.Party()).WithTimestamps(new DiscordRPC.Timestamps());
                pr.Assets.LargeImageKey = "netcraft";
                pr.Assets.LargeImageText = "discord.gg/BuKCBP8";
                pr.Details = "Netcraft " + Ver;
                pr.Timestamps.Start = DateTime.UtcNow;

                dRPC.SetPresence(pr);
                dRPC.Invoke();
                //dRPC.SetSubscription(EventType.Join | EventType.Spectate | EventType.JoinRequest);
                //dRPC.UpdateParty(new DiscordRPC.Party()
                //{
                //    ID = Secrets.CreateFriendlySecret(new Random()),
                //    Size = 1,
                //    Max = 4
                //});
                //dRPC.OnJoinRequested += DRPC_OnJoinRequested;
                //dRPC.OnJoin += DRPC_OnJoin;
                //dRPC.OnSpectate += DRPC_OnSpectate;
                uTh.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                FancyMessage.Show(lang.get("rpc.failed"));
            }
            Label5.Text = $"Netcraft {Ver}";
            CheckForIllegalCrossThreadCalls = false;
            foreach (Control c in Controls)
                c.KeyDown += OnKey;
            _Timer1.Start();
        }

        private void DRPC_OnSpectate(object sender, DiscordRPC.Message.SpectateMessage args)
        {

        }

        private void DRPC_OnJoin(object sender, DiscordRPC.Message.JoinMessage args)
        {

        }

        private async void DRPC_OnJoinRequested(object sender, DiscordRPC.Message.JoinRequestMessage args)
        {
            Debug.WriteLine($"{args.User.Username}#{args.User.Discriminator.ToString()} tried to join the game, because this is a test, declined");
            await Task.Delay(2000);
            DiscordRpcClient client = (DiscordRpcClient)sender;
            client.Respond(args, true);
        }

        private void packetreceived(string p)
        {
            string[] u = p.Split('?');
            if(u[0] == "msg")
            {
                FancyMessage.Show(String.Format(lang.get(u[1]), (object[])u.Skip(2).ToArray()));
            }
            if (u[0] == "name")
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
            if (u[0] == "pass")
            {
                string n = Utils.InputBox("Password please.");
                while (n == null)
                {
                    n = Utils.InputBox("Password please.");
                }
                client.f("pass?" + n);
            }
            if (u[0]=="bl")
            {
                foreach(string i in u.Skip(1).ToArray())
                {
                    blocklist.Add(i.ToLower());
                }
            }
        }

        private void DoLang()
        {
            _Button2.Text = lang.get("menu.button.singleplayer");
            _Button1.Text = lang.get("menu.button.network_game");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/BuKCBP8");
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        private async void Button2_Click_1(object sender, EventArgs e)
        {
            StartSingleplayer();
            //AllocConsole();
            new Logs().Show();
            await Task.Run(async () => {
                await Task.Delay(1000);
                BeginInvoke(new Action(() =>
                {
                    My.MyProject.Forms.Form1.ip = "127.0.0.1";
                    My.MyProject.Forms.Form1.Show();
                    Hide();
                }));
            });
        }

        public void StartSingleplayer()
        {
            if(!File.Exists(@"C:\Program Files\dotnet\dotnet.exe") && !File.Exists(@"C:\Program Files (x86)\dotnet\dotnet.exe"))
            {
                if(FancyMessage.Show("You have not installed .NET Core. Do you wanted to open download website?", "Ошибка", FancyMessage.Icon.Error, FancyMessage.Buttons.OKCancel) == FancyMessage.Result.OK) {
                    Process.Start(@"https://dotnet.microsoft.com/download/dotnet-core/2.1");
                }
                return;
            }
            My.MyProject.Forms.Form1.IsSingleplayer = true;
            My.MyProject.Forms.Form1.ServerProcess = new Process();
            My.MyProject.Forms.Form1.ServerProcess.StartInfo.Arguments = @"/c run.cmd";
            My.MyProject.Forms.Form1.ServerProcess.StartInfo.WorkingDirectory = @".\server\netcoreapp2.1";
            My.MyProject.Forms.Form1.ServerProcess.StartInfo.FileName = "cmd.exe";
            Form1.GetInstance().ServerProcess.StartInfo.StandardErrorEncoding = Encoding.Default;
            Form1.GetInstance().ServerProcess.StartInfo.StandardOutputEncoding = Encoding.Default;
            Form1.GetInstance().ServerProcess.StartInfo.RedirectStandardError = true;
            Form1.GetInstance().ServerProcess.StartInfo.RedirectStandardOutput = true;
            Form1.GetInstance().ServerProcess.StartInfo.RedirectStandardInput = true;
            Form1.GetInstance().ServerProcess.StartInfo.UseShellExecute = false;
            Form1.GetInstance().ServerProcess.OutputDataReceived += Form1.GetInstance().onServerProcessDataReceived;
            Form1.GetInstance().ServerProcess.ErrorDataReceived += Form1.GetInstance().onServerProcessDataReceived;
            My.MyProject.Forms.Form1.ServerProcess.Start();
            Form1.GetInstance().ServerProcess.BeginErrorReadLine();
            Form1.GetInstance().ServerProcess.BeginOutputReadLine();
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
                throw new EasterEggFoundException();
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

            }
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FancyMessage.Show(lang.get("text.question.confirm_exit"), "Netcraft", FancyMessage.Icon.Warning, FancyMessage.Buttons.OKCancel) == FancyMessage.Result.OK)
            {
                Environment.Exit(0);
                dRPC.ClearPresence();
                dRPC.Deinitialize();
            } else
            {
                e.Cancel = true;
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel3.Hide();
            panel2.Show();
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Paint(object sender, PaintEventArgs e)
        {
            //Image image = My.Resources.Resources.cobblestone4;
            //Graphics g = CreateGraphics();
            //g.Clear(BackColor);
            //for (int x = 0; x < (Width / 64) * 64; x++)
            //{
            //    for (int y = 0; y < (Height / 64) * 64; y++)
            //    {
            //        Rectangle rect = new Rectangle(x * 64, y * 64, 64, 64);
            //        g.DrawImage(image, rect);
            //    }
            //}
        }

        private void PictureBox2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void _Button2_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbghover;
            var c = (Control)sender;
            var rect = ClientRectangle;
            rect.Width = 32;
            rect.Height = 32;
            var clr = Color.DodgerBlue;
            int width = 1;

            var g = c.CreateGraphics();
            ControlPaint.DrawBorder(g, rect,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid,
                 clr, width, ButtonBorderStyle.Solid);
        }

        private void _Button2_MouseHover(object sender, EventArgs e)
        {

        }

        private void _Button2_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackgroundImage = My.Resources.Resources.buttonbg;
            ((Control)sender).Invalidate();
        }

        private void _Button5_MouseEnter(object sender, EventArgs e)
        {
            _Button2_MouseEnter(sender, e);
        }

        private void _Button5_MouseHover(object sender, EventArgs e)
        {

        }

        private void _Button5_MouseLeave(object sender, EventArgs e)
        {
            _Button2_MouseLeave(sender, e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void LabelPlayers_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }

    public class EasterEggFoundException : Exception
    {
        public override string Message
        {
            get
            {
                return "y u do this?";
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
                Interaction.MsgBox("Не удалось установить соединение с сервером!" + Constants.vbCrLf + Constants.vbCrLf + $"{ex.GetType().ToString()}: {ex.Message}");
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