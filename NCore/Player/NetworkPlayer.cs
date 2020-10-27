using System;
using System.Collections.Generic;
using global::System.Drawing;
using global::System.IO;
using global::System.Net;
using global::System.Net.Sockets;
using global::System.Security.Cryptography;
using global::System.Text;
using global::System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace NCore
{
    public class NetworkPlayer : CommandSender
    {
        public NetworkPlayer(TcpClient forClient) : base("", false)
        {
            // LastNotSpectatorPosition
            field_01931 = Position;
            d = forClient;
            d.GetStream().BeginRead(new byte[] { 0 }, 0, 0, (_) => e(), null);
            var argarg0 = this;
            PacketQueue = new SendQueueType(ref argarg0);
            World = NCore.World;
            ip = ((IPEndPoint)d.Client.RemoteEndPoint).Address.ToString();
            PlayerRectangle = new Rectangle(Position, new Size(47, 92));
        }

        public string Username { get; set; } = null;
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public Point Position { get; set; } = new Point(0, 0);
        public Inventory PlayerInventory { get; set; }
        public ItemStack SelectedItem { get; set; }
        public int Health { get; set; } = 100;
        public bool IsAdmin { get; set; } = false;
        public SendQueueType PacketQueue { get; set; }
        public WorldServer World { get; set; }
        public bool IsSpectator { get; set; } = false;
        public bool DisableMovementCheck { get; set; } = false;
        public bool IsLoaded { get; set; } = false;
        public Rectangle PlayerRectangle { get; set; }
        public bool NoClip { get; set; } = false;
        public int FallDistance { get; set; } = 0;
        public int MovedInAir { get; set; } = 0;
        public bool IsAuthorized { get; set; } = false;

        public string GetIp()
        {
            return ip;
        }

        private string ip;
        private Point field_01931;

        public void Spectator()
        {
            IsSpectator = true;
            field_01931 = Position;
            NCore.Send("removeplayer?" + Username, Username);
            SetNoClip(true);
        }

        public void Survival()
        {
            IsSpectator = false;
            Teleport(field_01931.X, field_01931.Y);
            NCore.Send("addplayer?" + Username, Username);
            SetNoClip(false);
        }

        public void SetNoClip(bool arg0)
        {
            if (arg0)
            {
                Send("noclip");
                NoClip = true;
            }
            else
            {
                Send("clip");
                NoClip = false;
            }
        }
        // getMessage
        public event aEventHandler a;

        public delegate void aEventHandler(string str, NetworkPlayer n);
        // clientLogout
        public event bEventHandler b;

        public delegate void bEventHandler(NetworkPlayer client);
        // SendMessage
        private StreamWriter c;
        // ListClient
        private TcpClient d;

        public void Give(Material m, int count = 1)
        {
            foreach (var g in PlayerInventory.Items)
            {
                if (g.Type == m)
                {
                    g.Count += count;
                    UpdateInventory();
                    return;
                }
            }

            PlayerInventory.AddItem(new ItemStack(m, count));
            UpdateInventory();
        }

        public void UpdateHealth(int h, string d = "died")
        {
            h = (int)Math.Round((decimal)h);
            var ev = new netcraft.server.api.events.PlayerHealthEventArgs(this, Health, h);
            netcraft.server.api.NCSApi.REPlayerHealthEvent(ev);
            if (ev.GetCancelled())
                return;
            if (h < 1)
            {
                Kill(d);
                return;
            }

            Health = h;
            Send("health?" + h.ToString());
        }

        public void DoWarning(string n)
        {
            Send("dowarn?" + n);
        }

        public void Damage(int d, NetworkPlayer damager = null)
        {
            if (NCore.IsNothing(damager))
            {
                UpdateHealth(Health - d, "damaged to death");
            }
            else
            {
                UpdateHealth(Health - d, $"был убит {damager.Username}");
            }
        }

        public void SendSkyColorChange(Color color)
        {
            Send($"sky?{color.Name}");
        }

        public void Damage(int d, string a = "died")
        {
            UpdateHealth(Health - d, a);
        }

        public void Kill(string deathMessage = "died")
        {
            // Form1.Send("chat?" + Username + " " + deathMessage)
            var ev = new netcraft.server.api.events.PlayerDeathEventArgs(this, Conversions.ToString(Operators.AddObject(Username + " ", deathMessage)), new Point(0, 0));
            netcraft.server.api.NCSApi.REPlayerDeathEvent(ev);
            if (ev.GetCancelled())
            {
                Health = 1;
                Send("health?1");
                return;
            }

            netcraft.server.api.Netcraft.Broadcast(ev.GetDeathMessage());
            Teleport(ev.GetSpawn().X, ev.GetSpawn().Y);
            Health = 100;
            Send("health?100");
        }

        public void Teleport(int x, int y)
        {
            Position = new Point(x, y);
            Send("teleport?" + x.ToString() + "?" + y.ToString());
        }

        public void Chat(string arg0)
        {
            Send("chat?" + arg0);
        }

        public void Craft(Material m)
        {
            if (m == Material.STICK)
            {
                if (PlayerInventory.CountOf(Material.PLANKS) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.PLANKS, 2);
                Give(Material.STICK, 4);
            }

            if (m == Material.PLANKS)
            {
                if (PlayerInventory.CountOf(Material.WOOD) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.WOOD);
                Give(Material.PLANKS, 4);
            }

            if (m == Material.STONE_AXE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 3)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.COBBLESTONE, 3);
                Give(Material.STONE_AXE);
            }

            if (m == Material.STONE_PICKAXE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 3)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.COBBLESTONE, 3);
                Give(Material.STONE_PICKAXE);
            }

            if (m == Material.STONE_SHOVEL)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.COBBLESTONE, 1);
                Give(Material.STONE_SHOVEL);
            }

            if (m == Material.STONE_SWORD)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.COBBLESTONE, 3);
                Give(Material.STONE_SWORD);
            }

            if (m == Material.IRON_AXE)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 3)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.IRON, 3);
                Give(Material.IRON_AXE);
            }

            if (m == Material.IRON_PICKAXE)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 3)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.IRON, 3);
                Give(Material.IRON_PICKAXE);
            }

            if (m == Material.IRON_SHOVEL)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.IRON, 1);
                Give(Material.IRON_SHOVEL);
            }

            if (m == Material.IRON_SWORD)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 2)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.STICK, 2);
                RemoveItem(Material.IRON, 3);
                Give(Material.IRON_SWORD);
            }

            if (m == Material.FURNACE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 8)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.COBBLESTONE, 8);
                Give(Material.FURNACE);
            }

            if (m == Material.IRON_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 9)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.IRON, 9);
                Give(Material.IRON_BLOCK);
            }

            if (m == Material.DIAMOND_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 9)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.DIAMOND, 9);
                Give(Material.DIAMOND_BLOCK);
            }

            if (m == Material.GOLD_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.GOLD) < 9)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.GOLD, 9);
                Give(Material.GOLD_BLOCK);
            }

            if (m == Material.IRON)
            {
                if (PlayerInventory.CountOf(Material.IRON_BLOCK) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.IRON_BLOCK, 1);
                Give(Material.IRON, 9);
            }

            if (m == Material.GOLD)
            {
                if (PlayerInventory.CountOf(Material.GOLD_BLOCK) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.GOLD_BLOCK, 1);
                Give(Material.GOLD, 9);
            }

            if (m == Material.DIAMOND)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND_BLOCK) < 1)
                {
                    Send("msgerror?У Вас недостаточно материалов для крафта.");
                    return;
                }

                RemoveItem(Material.DIAMOND_BLOCK, 1);
                Give(Material.DIAMOND, 9);
            }
        }

        public void SendBlockChange(int x, int y, EnumBlockType m, bool nonsolid = false, bool packetQueue = false, bool isBackground = false)
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
            if (m == EnumBlockType.GLASS)
                t = "glass";
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

            if (!packetQueue)
            {
                Send($"blockchange?{x}?{y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
            else
            {
                PacketQueue.AddQueue($"blockchange?{x}?{y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
        }

        public void SendBlockChange(Point x, EnumBlockType m, bool nonsolid = false, bool packetQueue = false, bool isBackground = false)
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
            if (m == EnumBlockType.GLASS)
                t = "glass";
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

            if (!packetQueue)
            {
                Send($"blockchange?{x.X}?{x.Y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
            else
            {
                PacketQueue.AddQueue($"blockchange?{x.X}?{x.Y}?" + (!string.IsNullOrEmpty(t) ? t : color.Name) + (isBackground ? "?bg" : "?fg") + (nonsolid ? "-non-solid" : "solid"));
            }
        }

        public void Furnace(Block a, Material b, Material c)
        {
            if (a.Type != EnumBlockType.FURNACE)
            {
                Kick("Invalid packet");
                return;
            }

            if (PlayerInventory.CountOf(b) < 1)
                return;
            if (c == Material.SAND)
            {
                RemoveItem(b);
                RemoveItem(c);
                Give(Material.GLASS);
                return;
            }

            if (c == Material.COBBLESTONE)
            {
                RemoveItem(b);
                RemoveItem(c);
                Give(Material.STONE);
            }
        }

        public void UpdateInventory()
        {
            Send("clearinventory");
            Thread.Sleep(100);
            foreach (var i in PlayerInventory.Items)
                PacketQueue.AddQueue("additem?" + i.Type.ToString() + " x " + i.Count.ToString());
            PacketQueue.SendQueue();
        }

        public void RemoveItem(Material m, int count = 1)
        {
            var itemsToRemove = new List<ItemStack>();
            foreach (var item in PlayerInventory.Items)
            {
                if (item.Type != m)
                    continue;
                if (item.Count > count)
                {
                    item.Count -= count;
                }

                if (item.Count < 1 | item.Count <= count)
                {
                    itemsToRemove.Remove(item);
                }
            }

            foreach (var item in itemsToRemove)
                PlayerInventory.Items.Remove(item);
            if (NCore.IsNothing(SelectedItem) | itemsToRemove.Contains(SelectedItem) | !PlayerInventory.Items.Contains(SelectedItem))
            {
                foreach (var i in Netcraft.GetOnlinePlayers())
                {
                    if ((i.UUID ?? "") != (UUID ?? ""))
                    {
                        i.Send($"itemset?{Username}?nothing");
                    }
                    else
                    {
                        i.Send($"itemset?@?nothing");
                    }
                }
            }

            UpdateInventory();
        }

        public void Disconnect()
        {
            d.Client.Close();
            d = null;
        }

        public bool IsOnGround
        {
            get
            {
                foreach (var o in NCore.World.Blocks)
                {
                    var bpos = new Point(o.Position.X * 32, o.Position.Y * 32);
                    var brec = new Rectangle(bpos, new Size(32, 32));
                    if (NCore.DistanceBetweenPoint(bpos, Position) > 10 * 32)
                        continue;
                    if (brec.IntersectsWith(PlayerRectangle))
                    {
                        if (o.IsBackground)
                            continue;
                        if (o.Type == EnumBlockType.WATER)
                            continue;
                        if (NoClip)
                            return true;
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }

                return false;
            }
        }

        private void e()
        {
            PlayerRectangle = new Rectangle(Position, new Size(47, 92));
            try
            {
                a?.Invoke(Encode.Decrypt(new StreamReader(d.GetStream()).ReadLine()), this);
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
                NCore.LogError(ex);
            }
            catch (ArgumentNullException ex)
            {
                NCore.LogError(ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                NCore.LogError(ex);
            }
            catch (ArgumentException ex)
            {
                NCore.LogError(ex);
            }
            catch (OutOfMemoryException ex)
            {
                NCore.LogError(ex);
            }
            catch (NullReferenceException ex)
            {
                if (!IsLoaded)
                {
                    b?.Invoke(this);
                }

                NCore.LogError(ex);
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
            catch (NullReferenceException ex)
            {
            }
            catch (InvalidOperationException ex)
            {
                b?.Invoke(this);
            }
        }

        public void Send(string Messsage)
        {
            try
            {
                c = new StreamWriter(d.GetStream());
                c.WriteLine(Encode.Encrypt(Messsage));
                c.Flush();
            }
            catch (Exception ex)
            {
            }
        }

        public void Kick(string kickMessage = "You have been kicked out from server.", bool bcKick = false)
        {
            if (bcKick)
            {
                Netcraft.Broadcast($"{Username} был выгнан из игры. Причина: {kickMessage}");
            }

            if(IsLoaded) NCore.Log($"Kicked {Username} from the game: {kickMessage}");
            Send("msg?" + kickMessage);
            d.Client.Close();
            d = null;
        }
    }
    /* TODO ERROR: Skipped WarningDirectiveTrivia */
    public class Encode
    {
        private static byte[] key = new byte[] { 62, 59, 25, 19, 37 };
        private static byte[] IV = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        internal const string EncryptionKey = "81iSifdf"; // "HOMECLOUD" & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString ' & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                var inputByteArray = new byte[stringToDecrypt.Length + 1];
                key = Encoding.UTF8.GetBytes(NCore.Left(EncryptionKey, 8));
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
            }

            return default;
        }

        public static string Encrypt(string stringToEncrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(NCore.Left(EncryptionKey, 8));
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
            }

            return default;
        }
    }
}