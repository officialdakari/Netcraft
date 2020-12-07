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
using Microsoft.VisualBasic.CompilerServices;
using NCore.netcraft.server.api;

namespace NCore
{
    public class NetcraftPlayer : CommandSender
    {
        public NetcraftPlayer(TcpClient forClient) : base("", false)
        {
            field_01931 = Position;
            d = forClient;
            d.GetStream().BeginRead(new byte[] { 0 }, 0, 0, (_) => e(), null);
            var argarg0 = this;
            PacketQueue = new SendQueueType(ref argarg0);
            World = NCore.GetNCore().World;
            ip = ((IPEndPoint)d.Client.RemoteEndPoint).Address.ToString();
            PlayerRectangle = new Rectangle(Position, new Size(47, 92));
        }

        public string Username { get; internal set; } = null;
        public string UUID { get; internal set; } = Guid.NewGuid().ToString();
        public Point Position { get; set; } = new Point(0, 0);
        public Inventory PlayerInventory { get; internal set; }
        public ItemStack SelectedItem { get; internal set; }
        public int Health { get; set; } = 100;
        public int Hunger { get; set; } = 20;
        public int Saturation { get; set; } = 5;
        public bool IsAdmin { get; set; } = false;
        public SendQueueType PacketQueue { get; internal set; }
        public WorldServer World { get; internal set; }
        public bool IsSpectator { get; internal set; } = false;
        public bool DisableMovementCheck { get; set; } = false;
        public bool IsLoaded { get; internal set; } = false;
        public Rectangle PlayerRectangle { get; internal set; }
        public bool NoClip { get; private set; } = false;
        public int FallDistance { get; set; } = 0;
        public int MovedInAir { get; set; } = 0;
        public bool IsAuthorized { get; set; } = false;
        public int AntiFlyWarnings { get; set; } = 0;
        public BlockChest OpenChest { get; set; } = null;
        internal NCore.Lang lang;
        public DateTime LastKeepAlivePacket { get; internal set; }

        public string GetIp()
        {
            return ip;
        }

        public override string ToString()
        {
            string t = "";
            t += $"username='{Username}';";
            t += $"uuid='{UUID}';";
            t += $"position={Position.X.ToString()},{Position.Y.ToString()};";
            t += $"selecteditem='{SelectedItem.ToString()}';";
            t += $"world='NCore.WorldServer';";
            t += $"isSpectator='{IsSpectator.ToString()}';";
            t += $"nomovecheck='{DisableMovementCheck.ToString()}';";
            t += $"isLoaded='{IsLoaded.ToString()}';";
            t += $"playerRectangle='{PlayerRectangle.ToString()}';";
            t += $"noclip='{NoClip.ToString()}';";
            t += $"falldistance='{FallDistance.ToString()}';";
            t += $"antiflywarnings='{AntiFlyWarnings.ToString()}';";
            return t;
        }

        public TcpClient GetConnection()
        {
            return d;
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

        public async Task Chest(BlockChest chest)
        {
            OpenChest = chest;
            string items = "";
            foreach(ItemStack item in chest.items)
            {
                items += $"{item.Type.ToString()} x {item.Count.ToString()}?";
            }
            items = items.TrimEnd('?');
            await Send("chestopen?" + items);
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

        public async Task Give(Material m, int count = 1)
        {
            foreach (var g in PlayerInventory.Items)
            {
                if (g.Type == m)
                {
                    g.Count += count;
                    await UpdateInventory();
                    return;
                }
            }

            PlayerInventory.AddItem(new ItemStack(m, count));
            await UpdateInventory();
        }

        public async Task UpdateHealth(int h, string d = "died", string[] dmFormat = null)
        {
            this.Health = h;
            var ev = new netcraft.server.api.events.PlayerHealthEventArgs(this, Health, h);
            netcraft.server.api.NCSApi.REPlayerHealthEvent(ev);
            if (ev.GetCancelled())
                return;
            if (h < 1)
            {
                await Kill(d, dmFormat, true);
                return;
            }

            Health = h;
            await Send("health?" + h.ToString());
        }

        public async Task UpdateHunger(int h)
        {
            this.Hunger = h;
            await Send("hunger?" + (h > 20 ? "20" : h.ToString()));
        }

        public async Task DoWarning(string n)
        {
            await Send("dowarn?" + n);
        }

        public async Task SendLog(string t)
        {
            await Send("evalresult?" + t);
        }

        public async Task Damage(int d, NetcraftPlayer damager = null)
        {
            if (NCore.IsNothing(damager))
            {
                await UpdateHealth(Health - d, "deathmessage.out");
            }
            else
            {
                await UpdateHealth(Health - d, "");
            }
        }

        public async Task SendSkyColorChange(Color color)
        {
            await Send($"sky?{color.Name}");
        }

        public async Task Damage(int d, string a = "died", string[] vs = null)
        {
            if (vs == null) vs = new string[] { };
            await UpdateHealth(Health - d, a, vs);
        }

        public async Task Kill(string deathMessage = "died", string[] vs = null, bool isTranslation = false)
        {
            // Form1.Send("chat?" + Username + " " + deathMessage)
            var ev = new netcraft.server.api.events.PlayerDeathEventArgs(this, null, new Point(0, 0));
            
            netcraft.server.api.NCSApi.REPlayerDeathEvent(ev);
            if (ev.GetCancelled())
            {
                Health = 1;
                await Send("health?1");
                return;
            }
            if(isTranslation)
            {
                string[] v = new string[] {Username};
                if(vs != null)
                {
                    foreach (var g in vs)
                    {
                        v = v.Append(g).ToArray();
                    }
                }
                await NCore.GetNCore().BroadcastChatTranslation(deathMessage, v, null, true);
            } else
            {
                await NCore.GetNCore().Chat(Username + deathMessage);
            }
            Teleport(ev.GetSpawn().X, ev.GetSpawn().Y);
            Position = ev.GetSpawn();
            await PacketQueue.AddQueue($"teleport?{ev.GetSpawn().X.ToString()}?{ev.GetSpawn().Y.ToString()}");
            Health = 100;
            await PacketQueue.AddQueue("health?100");
            await PacketQueue.SendQueue();
            await UpdateHunger(20);
        }

        public async Task Teleport(int x, int y)
        {
            Position = new Point(x, y);
            await Send("teleport?" + x.ToString() + "?" + y.ToString());
        }

        public async Task Chat(string arg0)
        {
            await Send("chat?" + arg0);
        }

        public async Task Heal(int h = 1)
        {
            if(Health + h > 100)
            {
                await UpdateHealth(100);
                return;
            }
            await UpdateHealth(Health + h);
        }

        public async Task Craft(Material m)
        {
            if (m == Material.STICK)
            {
                if (PlayerInventory.CountOf(Material.PLANKS) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.PLANKS, 2);
                await Give(Material.STICK, 4);
            }

            if (m == Material.BUCKET)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.IRON, 3);
                await Give(Material.BUCKET, 3);
            }

            if (m == Material.PLANKS)
            {
                if (PlayerInventory.CountOf(Material.WOOD) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.WOOD);
                await Give(Material.PLANKS, 4);
            }

            if (m == Material.STONE_AXE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.COBBLESTONE, 3);
                await Give(Material.STONE_AXE);
            }

            if (m == Material.STONE_PICKAXE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.COBBLESTONE, 3);
                await Give(Material.STONE_PICKAXE);
            }

            if (m == Material.STONE_SHOVEL)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.COBBLESTONE, 1);
                await Give(Material.STONE_SHOVEL);
            }

            if (m == Material.STONE_SWORD)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.COBBLESTONE, 3);
                await Give(Material.STONE_SWORD);
            }

            if (m == Material.IRON_AXE)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.IRON, 3);
                await Give(Material.IRON_AXE);
            }

            if (m == Material.IRON_PICKAXE)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.IRON, 3);
                await Give(Material.IRON_PICKAXE);
            }

            if (m == Material.IRON_SHOVEL)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.IRON, 1);
                await Give(Material.IRON_SHOVEL);
            }

            if (m == Material.IRON_SWORD)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.IRON, 3);
                await Give(Material.IRON_SWORD);
            }



            if (m == Material.DIAMOND_AXE)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.DIAMOND, 3);
                await Give(Material.DIAMOND_AXE);
            }

            if (m == Material.DIAMOND_PICKAXE)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.DIAMOND, 3);
                await Give(Material.DIAMOND_PICKAXE);
            }

            if (m == Material.DIAMOND_SHOVEL)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.DIAMOND, 1);
                await Give(Material.DIAMOND_SHOVEL);
            }

            if (m == Material.DIAMOND_SWORD)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 2)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                if (PlayerInventory.CountOf(Material.STICK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.STICK, 2);
                await RemoveItem(Material.DIAMOND, 3);
                await Give(Material.DIAMOND_SWORD);
            }


            if (m == Material.FURNACE)
            {
                if (PlayerInventory.CountOf(Material.COBBLESTONE) < 8)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.COBBLESTONE, 8);
                await Give(Material.FURNACE);
            }

            if (m == Material.CHEST)
            {
                if (PlayerInventory.CountOf(Material.PLANKS) < 8)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.PLANKS, 8);
                await Give(Material.CHEST);
            }

            if (m == Material.IRON_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.IRON) < 9)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.IRON, 9);
                await Give(Material.IRON_BLOCK);
            }

            if (m == Material.DIAMOND_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND) < 9)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.DIAMOND, 9);
                await Give(Material.DIAMOND_BLOCK);
            }

            if (m == Material.GOLD_BLOCK)
            {
                if (PlayerInventory.CountOf(Material.GOLD) < 9)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.GOLD, 9);
                await Give(Material.GOLD_BLOCK);
            }

            if (m == Material.IRON)
            {
                if (PlayerInventory.CountOf(Material.IRON_BLOCK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.IRON_BLOCK, 1);
                await Give(Material.IRON, 9);
            }

            if (m == Material.GOLD)
            {
                if (PlayerInventory.CountOf(Material.GOLD_BLOCK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.GOLD_BLOCK, 1);
                await Give(Material.GOLD, 9);
            }

            if (m == Material.DIAMOND)
            {
                if (PlayerInventory.CountOf(Material.DIAMOND_BLOCK) < 1)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.DIAMOND_BLOCK, 1);
                await Give(Material.DIAMOND, 9);
            }
            if (m == Material.BREAD)
            {
                if (PlayerInventory.CountOf(Material.WHEAT) < 3)
                {
                    await Send("msgerror?" + lang.get("error.craft.no-materials"));
                    return;
                }

                await RemoveItem(Material.WHEAT, 3);
                await Give(Material.BREAD, 2);
            }
        }

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

        public async Task Furnace(Block a, Material b, Material c)
        {
            if (a.Type != EnumBlockType.FURNACE)
            {
                await Kick("Invalid packet");
                return;
            }

            if (PlayerInventory.CountOf(b) < 1)
                return;
            if (c == Material.SAND)
            {
                await RemoveItem(b);
                await RemoveItem(c);
                await Give(Material.GLASS);
                return;
            }

            if (c == Material.COBBLESTONE)
            {
                await RemoveItem(b);
                await RemoveItem(c);
                await Give(Material.STONE);
            }

            if(c == Material.RAW_BEEF)
            {
                await RemoveItem(b);
                await RemoveItem(c);
                await Give(Material.COOKED_BEEF);
            }

            if (c == Material.IRON_ORE)
            {
                await RemoveItem(b);
                await RemoveItem(c);
                await Give(Material.IRON, 3);
            }

            if (c == Material.GOLD_ORE)
            {
                await RemoveItem(b);
                await RemoveItem(c);
                await Give(Material.GOLD, 3);
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

        public async Task RemoveItem(Material m, int count = 1)
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

                if ((item.Count < 1) || (item.Count <= count))
                {
                    itemsToRemove.Add(item);
                }
            }

            foreach (var item in itemsToRemove)
                PlayerInventory.Items.Remove(item);
            if (NCore.IsNothing(SelectedItem) || itemsToRemove.Contains(SelectedItem) || !PlayerInventory.Items.Contains(SelectedItem))
            {
                foreach (var i in Netcraft.GetOnlinePlayers())
                {
                    if ((i.UUID ?? "") != (UUID ?? ""))
                    {
                        await i.Send($"itemset?{Username}?nothing");
                    }
                    else
                    {
                        await i.Send($"itemset?@?nothing");
                    }
                }
            }

            await UpdateInventory();
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

        public bool IsOnGround
        {
            get
            {
                foreach (var o in NCore.GetNCore().World.Blocks)
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
                        if (o.Type == EnumBlockType.SAPLING)
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

        private async Task e()
        {
            PlayerRectangle = new Rectangle(Position, new Size(47, 92));
            try
            {
                a?.Invoke(Encode.d(new StreamReader(d.GetStream()).ReadLine()), this);
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
                if (!IsLoaded)
                {
                    b?.Invoke(this);
                }

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
            catch (NullReferenceException ex)
            {
            }
            catch (InvalidOperationException ex)
            {
                b?.Invoke(this);
            }
        }

        public async Task Send(string Messsage)
        {
            try
            {
                c = new StreamWriter(d.GetStream());
                c.WriteLine(Encode.e(Messsage));
                c.Flush();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task Kick(string kickMessage = "You have been kicked out from server.", bool bcKick = false)
        {
            if (bcKick)
            {
                await Netcraft.Broadcast(NCore.GetNCore().lang.get("broadcast.kick", Username, kickMessage));
            }

            if(IsLoaded) NCore.GetNCore().Log($"{Username} kicked from the game: '{kickMessage}'");
            await Send("msgkick?" + kickMessage);
            b?.Invoke(this);
            if (d != null && d.Client.Connected) Disconnect();
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
                return "";
            }
        }
    }
}