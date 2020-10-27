using global::System.Drawing;

namespace NCore.netcraft.server.api
{
    namespace events
    {
        public class BlockBreakEventArgs
        {
            private Block a;
            private NetworkPlayer b;
            private bool c;

            public Block GetBlock()
            {
                return a;
            }

            public NetworkPlayer GetPlayer()
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

            internal BlockBreakEventArgs(NetworkPlayer p, Block b)
            {
                this.b = p;
                a = b;
                c = false;
            }
        }

        public class BlockPlaceEventArgs
        {
            private Block a;
            private NetworkPlayer b;
            private bool c;

            public Block GetBlock()
            {
                return a;
            }

            public NetworkPlayer GetPlayer()
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

            internal BlockPlaceEventArgs(NetworkPlayer p, Block b)
            {
                this.b = p;
                a = b;
                c = false;
            }
        }

        public class PlayerChatEventArgs
        {
            private string a;
            private NetworkPlayer b;
            private bool c;

            public string GetMessage()
            {
                return a;
            }

            public NetworkPlayer GetPlayer()
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

            internal PlayerChatEventArgs(NetworkPlayer p, string m)
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
            private NetworkPlayer c;
            private bool d;

            public Point GetFrom()
            {
                return a;
            }

            public Point GetTo()
            {
                return b;
            }

            public NetworkPlayer GetPlayer()
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

            public PlayerMoveEventArgs(Point from, Point mTo, NetworkPlayer p)
            {
                d = false;
                a = from;
                b = mTo;
                c = p;
            }
        }

        public class PlayerLoginEventArgs
        {
            private NetworkPlayer player;

            public PlayerLoginEventArgs(NetworkPlayer player)
            {
                this.player = player;
            }

            public NetworkPlayer GetPlayer()
            {
                return player;
            }
        }

        public class PlayerJoinEventArgs
        {
            private NetworkPlayer player;

            public NetworkPlayer GetPlayer()
            {
                return player;
            }

            public PlayerJoinEventArgs(NetworkPlayer p)
            {
                player = p;
            }
        }

        public class PlayerLeaveEventArgs
        {
            private NetworkPlayer player;

            public NetworkPlayer GetPlayer()
            {
                return player;
            }

            public PlayerLeaveEventArgs(NetworkPlayer p)
            {
                player = p;
            }
        }

        public class PlayerHealthEventArgs
        {
            private NetworkPlayer a;
            private int b;
            private int c;
            private bool d;

            public NetworkPlayer GetPlayer()
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

            public PlayerHealthEventArgs(NetworkPlayer p, int oldHealth, int newHealth)
            {
                a = p;
                b = oldHealth;
                c = newHealth;
                d = false;
            }
        }

        public class PlayerDeathEventArgs
        {
            private NetworkPlayer a;
            private string b;
            private bool c;
            private Point d;

            public PlayerDeathEventArgs(NetworkPlayer a, string b, Point d)
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

            public NetworkPlayer GetPlayer()
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
            private NetworkPlayer a;
            private Block b;
            private bool c;

            public BlockRightClickEvent(NetworkPlayer a, Block b)
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

            public NetworkPlayer GetPlayer()
            {
                return a;
            }

            public Block GetBlock()
            {
                return b;
            }
        }
    }
}