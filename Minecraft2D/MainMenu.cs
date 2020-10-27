using System;
using System.Diagnostics;
using System.Drawing;
using global::System.IO;
using System.Linq;
using global::System.Net.Sockets;
using System.Windows.Forms;
using Microsoft.VisualBasic;

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
        protected bool labelWithCur = true;
        protected string labelText = "converted to c# from vbnet";
        protected int indexPos = 0;
        protected int direction = 1;
        protected Color[] colors = new[] { Color.Red, Color.Orange, Color.Goldenrod, Color.Gold, Color.Yellow, Color.GreenYellow, Color.LightGreen, Color.Green, Color.LightBlue, Color.Blue, Color.DarkBlue, Color.BlueViolet, Color.Violet };

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

            string t = ">" + Strings.Left(labelText, labelPos) + (labelWithCur ? "_" : "");
            if (labelChangeDelay == 0)
            {
                labelPos += labelDirection;
                if (labelPos == 0)
                {
                    labelDirection = 1;
                }
                else if (labelPos == labelText.Length)
                {
                    labelDirection = -1;
                    labelChangeDelay = 20;
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

            My.MyProject.Forms.Form1.ip = TextBox1.Text;
            My.MyProject.Forms.Form1.Show();
            Hide();
        }

        public readonly string Ver = "1.3-ALPHA-U26102020";

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

        private void MainMenu_Load(object sender, EventArgs e)
        {
            Label5.Text = $"Netcraft {Ver}";
            CheckForIllegalCrossThreadCalls = false;
            foreach (Control c in Controls)
                c.KeyDown += OnKey;
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

        private void _onPingComplete()
        {
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
    }

    public class Pinger
    {
        public string Name { get; set; }
        public string Motd { get; set; }
        public string Players { get; set; }

        private TcpClient client;
        private StreamWriter sWriter;

        public delegate void _xUpdate(string str);
        // 
        // '    '     If InvokeRequired Then
        // 
        // Else
        // '   '      TextBox3.AppendText(str & vbNewLine)
        // End If
        // End Sub
        public event OnPingCompleteEventHandler OnPingComplete;

        public delegate void OnPingCompleteEventHandler();

        private void Read(IAsyncResult ar)
        {
            try
            {
                string x = Encode.Decrypt(new StreamReader(client.GetStream()).ReadLine());
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
                sWriter.WriteLine(Encode.Encrypt(str));
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