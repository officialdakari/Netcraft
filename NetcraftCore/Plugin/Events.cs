using global::System.Drawing;

namespace NCore.netcraft.server.api
{
    namespace events
    {
        public class BlockBreakEventArgs
        {
            private Block a;
            private NetcraftPlayer b;
            private bool c;

            public Block GetBlock()
            {
                return a;
            }

            public NetcraftPlayer GetPlayer()
            {
                return b;
            }

            public void SetCancelled(bool arg0)
            {
                c = arg0;
            }

            public bool GetCancelled()
            {
                return c;
            }

            internal BlockBreakEventArgs(NetcraftPlayer p, Block b)
            {
                this.b = p;
                a = b;
                c = false;
            }
        }

        public class BlockPlaceEventArgs
        {
            private Block a;
            private NetcraftPlayer b;
            private bool c;

            public Block GetBlock()
            {
                return a;
            }

            public NetcraftPlayer GetPlayer()
            {
                return b;
            }

            public void SetCancelled(bool arg0)
            {
                c = arg0;
            }

            public bool GetCancelled()
            {
                return c;
            }

            internal BlockPlaceEventArgs(NetcraftPlayer p, Block b)
            {
                this.b = p;
                a = b;
                c = false;
            }
        }

        public class PlayerChatEventArgs
        {
            private string a;
            private NetcraftPlayer b;
            private bool c;

            public string GetMessage()
            {
                return a;
            }

            public void SetMessage(string a)
            {
                this.a = a;
            }

            public NetcraftPlayer GetPlayer()
            {
                return b;
            }

            public void SetCancelled(bool arg0)
            {
                c = arg0;
            }

            public bool GetCancelled()
            {
                return c;
            }

            internal PlayerChatEventArgs(NetcraftPlayer p, string m)
            {
                b = p;
                a = m;
                c = false;
            }
        }

        public class PlayerMoveEventArgs
        {
            private Point a;
            private Point b;
            private NetcraftPlayer c;
            private bool d;

            public Point GetFrom()
            {
                return a;
            }

            public Point GetTo()
            {
                return b;
            }

            public NetcraftPlayer GetPlayer()
            {
                return c;
            }

            public bool GetCancelled()
            {
                return d;
            }

            public void SetCancelled(bool arg0)
            {
                d = arg0;
            }

            public PlayerMoveEventArgs(Point from, Point mTo, NetcraftPlayer p)
            {
                d = false;
                a = from;
                b = mTo;
                c = p;
            }
        }

        public class PlayerLoginEventArgs
        {
            private NetcraftPlayer player;

            public PlayerLoginEventArgs(NetcraftPlayer player)
            {
                this.player = player;
            }

            public NetcraftPlayer GetPlayer()
            {
                return player;
            }
        }

        public class PlayerJoinEventArgs
        {
            private NetcraftPlayer player;

            public NetcraftPlayer GetPlayer()
            {
                return player;
            }

            public PlayerJoinEventArgs(NetcraftPlayer p)
            {
                player = p;
            }
        }

        public class PlayerLeaveEventArgs
        {
            private NetcraftPlayer player;

            public NetcraftPlayer GetPlayer()
            {
                return player;
            }

            public PlayerLeaveEventArgs(NetcraftPlayer p)
            {
                player = p;
            }
        }

        public class PlayerHealthEventArgs
        {
            private NetcraftPlayer a;
            private int b;
            private int c;
            private bool d;

            public NetcraftPlayer GetPlayer()
            {
                return a;
            }

            public int GetOldHealth()
            {
                return b;
            }

            public int GetNewHealth()
            {
                return c;
            }

            public bool GetCancelled()
            {
                return d;
            }

            public void SetCancelled(bool arg0)
            {
                d = arg0;
            }

            public PlayerHealthEventArgs(NetcraftPlayer p, int oldHealth, int newHealth)
            {
                a = p;
                b = oldHealth;
                c = newHealth;
                d = false;
            }
        }

        public class PlayerDeathEventArgs
        {
            private NetcraftPlayer a;
            private string b;
            private bool c;
            private Point d;

            public PlayerDeathEventArgs(NetcraftPlayer a, string b, Point d)
            {
                this.a = a;
                this.b = b;
                c = false;
                this.d = d;
            }

            public void SetDeathMessage(string b)
            {
                this.b = b;
            }

            public string GetDeathMessage()
            {
                return b;
            }

            public NetcraftPlayer GetPlayer()
            {
                return a;
            }

            public void SetCancelled(bool a)
            {
                c = a;
            }

            public bool GetCancelled()
            {
                return c;
            }

            public Point GetSpawn()
            {
                return d;
            }

            public void SetSpawn(Point d)
            {
                this.d = d;
            }
        }

        public class BlockRightClickEvent
        {
            private NetcraftPlayer a;
            private Block b;
            private bool c;

            public BlockRightClickEvent(NetcraftPlayer a, Block b)
            {
                this.a = a;
                this.b = b;
                c = false;
            }

            public void SetCancelled(bool c)
            {
                this.c = c;
            }

            public bool GetCancelled()
            {
                return c;
            }

            public NetcraftPlayer GetPlayer()
            {
                return a;
            }

            public Block GetBlock()
            {
                return b;
            }
        }

        public class PlayerPacketSend
        {
            NetcraftPlayer a;
            string b;
            bool c;

            public NetcraftPlayer GetPlayer() => a;
            public string GetPacket() => b;
            public bool GetCancelled() => c;

            public void SetPacket(string a)
            {
                this.b = a;
            }

            public void SetCancelled(bool c)
            {
                this.c = c;
            }

            internal PlayerPacketSend(NetcraftPlayer a, string b, bool c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }
        }

        public class PlayerPacketReceive
        {
            NetcraftPlayer a;
            string b;
            bool c;

            public NetcraftPlayer GetPlayer() => a;
            public string GetPacket() => b;
            public bool GetCancelled() => c;

            public void SetPacket(string a)
            {
                this.b = a;
            }

            public void SetCancelled(bool c)
            {
                this.c = c;
            }

            internal PlayerPacketReceive(NetcraftPlayer a, string b, bool c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }
        }

        public class SandPhysicsEvent
        {
            Block a;
            Point b;
            Point c;
            bool d;

            public Point GetFrom() => b;
            public Point GetTo() => c;
            public Block GetBlock() => a;
            public bool GetCancelled() => d;

            public void SetTo(Point a)
            {
                this.c = a;
            }

            public void SetCancelled(bool d)
            {
                this.d = d;
            }

            internal SandPhysicsEvent(Block a, Point b, Point c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = false;
            }
        }
    }
}