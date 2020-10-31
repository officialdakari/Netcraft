using System;
using System.Collections.Generic;
using global::System.Collections.Specialized;
using global::System.IO;
using global::System.Net;
using global::System.Net.Sockets;
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic

public partial class Server
{
    private const int MaxActiveConnections = 15;
    private const int MaxQueueLength = 20;
    private static TcpListener Listning;
    // Shared Allclient As TcpClient
    /// <summary>
    /// Все клиенты.
    /// </summary>
    internal static List<User> clientList = new List<User>();
    private static User pClient;
    // Dim Data As Specialized.StringCollection = New Specialized.StringCollection
    /// <summary>
    /// Происходит, когда сервер получил данные от клиента. Чтобы клиент отправлял данные всем, добавьте в обработчик события VBTCP.Server.Broadcast("[ваш формат]")
    /// </summary>
    /// <param name="msg">Сообщение, полученное от клиента.</param>
    /// <param name="sender">Класс клиента, который отправил сообщение.</param>
    public static event OnMessageReceivedEventHandler OnMessageReceived;

    public delegate void OnMessageReceivedEventHandler(string msg, ref User sender);
    /// <summary>
    /// Не рекомендуется. Используйте Broadcast(string msg)
    /// </summary>
    /// <param name="msg"></param>
    private static event BroadcastmsgEventHandler Broadcastmsg;

    private delegate void BroadcastmsgEventHandler(string msg);
    /// <summary>
    /// Происходит, когда к серверу подключается новый клиент.
    /// </summary>
    /// <param name="client">Предоставляет класс клиента который подключился.</param>
    public static event OnClientConnectEventHandler OnClientConnect;

    public delegate void OnClientConnectEventHandler(User client);
    /// <summary>
    /// Происходит, когда клиент отключается.
    /// </summary>
    /// <param name="client">Класс клиента который отключился.</param>
    public static event OnClientDisconnectEventHandler OnClientDisconnect;

    public delegate void OnClientDisconnectEventHandler(User client);

    // create a delegate
    public delegate void _cUpdate(string str, bool relay);

    public static void UpdateList(string str, bool relay)
    {
    }
    /// <summary>
    /// Удаляет всех неподключенных клиентов. Автоматически вызывается после отключения клиента.
    /// </summary>
    public static void RemoveUnconnectedClients()
    {
        for (int x = 0, loopTo = clientList.Count - 1; x <= loopTo; x++)
        {
            try
            {
                clientList[x].Send("");
            }
            catch (Exception ex)
            {
                clientList.RemoveAt(x);
            }
        }
    }

    public static void Broadcast(string msg)
    {
        Broadcastmsg?.Invoke(msg);
    }

    private static void send(string str)
    {
        for (int x = 0, loopTo = clientList.Count - 1; x <= loopTo; x++)
        {
            try
            {
                clientList[x].Send(str);
            }
            catch (Exception ex)
            {
                clientList.RemoveAt(x);
            }
        }
    }

    public static void AcceptClient(IAsyncResult ar)
    {
        pClient = new User(Listning.EndAcceptTcpClient(ar));
        // End If
        pClient.getMessage += MessageReceived;
        pClient.clientLogout += ClientExited;
        clientList.Add(pClient);
        Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
        OnClientConnect?.Invoke(pClient);
    }

    private static void MessageReceived(string str, ref User sender)
    {
        OnMessageReceived?.Invoke(str, ref sender);
    }

    private static void ClientExited(User client)
    {
        try
        {
            OnClientDisconnect?.Invoke(client);
            // client.Disconnect()
            clientList.Remove(client);
            // UpdateList("Client Exited!", True)
            RemoveUnconnectedClients();
        }
        catch (Exception ex)
        {
            try
            {
                client.Disconnect();
            }
            catch (Exception ex2)
            {
                try
                {
                    clientList.Remove(client);
                }
                catch (Exception ex3)
                {
                    try
                    {
                        RemoveUnconnectedClients();
                    }
                    catch (Exception ex4)
                    {
                    }
                }
            }
        }
    }

    private static bool Listening = false;

    public static void StartServer(int port)
    {
        // EncryptionKey = encodeKey
        Listning = new TcpListener(IPAddress.Any, port);
        // If Listening = False Then

        Listning.Start();
        UpdateList("Server Starting", false);
        Listning.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), Listning);
        Listening = true;
        // Else
        // Listning.Stop()
        // End If
    }
    /// <summary>
    /// Отключает клиента с указанном CID.
    /// </summary>
    /// <param name="cid"></param>
    public void Disconnect(int cid)
    {
        FindByCID(cid).Disconnect();
        // clientList.Remove(FindByCID)
    }
    /// <summary>
    /// Отправляет указанное сообщение только клиентам у которых есть опция opt
    /// </summary>
    /// <param name="opt">Требуемая опция.</param>
    /// <param name="str">Сообщение</param>
    public static void BroadcastOnly(int opt, string str)
    {
        bconly?.Invoke(opt, str);
    }
    /// <summary>
    /// Ищет клиента с указанном CID. Если клиент не найден, возвращается Nothing (null).
    /// </summary>
    /// <param name="cid"></param>
    /// <returns></returns>
    public static User FindByCID(int cid)
    {
        foreach (var client in clientList)
        {
            if (Conversions.ToDouble(client.CID) == cid)
            {
                return client;
            }
        }

        return null;
        // Return Nothing
    }

    private static event BCCEventHandler BCC;

    private delegate void BCCEventHandler(string msg, string channel, bool chatNotMuted, string user);

    internal static void BroadcastChannel(string msg, string channel, bool chatNotMuted = false, string user = "SYSTEM T")
    {
        BCC?.Invoke(msg, channel, chatNotMuted, user);
    }

    /// <summary>
    /// Ищет клиента с указанном Username. Если клиент не найден, возвращается Nothing (null).
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static User FindByName(string name)
    {
        foreach (var client in clientList)
        {
            if ((client.Username ?? "") == (name ?? ""))
            {
                return client;
            }
        }

        return null;
        // Return Nothing
    }

    private static List<User> FindByCIDRange(int cid)
    {
        var result = new List<User>();
        foreach (var client in clientList)
        {
            if (Conversions.ToDouble(client.CID) == cid)
            {
                result.Add(client);
            }
        }

        return result;
    }

    public static void StopServer()
    {
        Listning.Stop();
        Listning = null;
    }

    private static event bconlyEventHandler bconly;

    private delegate void bconlyEventHandler(int opt, string str);

    public enum UserActive
    {
        Chat = 1,
        Console = 2,
        Auth = 3,
        PluginCommand = 4
    }

    public enum ServerRedirect
    {
        No = 0,
        Queue = 1
    }

    public enum KickReason
    {
        None = 0,
        ServerError = 1
    }

    public partial class User
    {
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public void Kick(KickReason reason)
        {
            if (reason == KickReason.None)
            {
                Send("msg~error~Disconnected by remote peer");
                Disconnect();
            }
            else if (reason == KickReason.ServerError)
            {
                Send("msg~error~Internal Server Error");
                Disconnect();
            }
        }
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public event getMessageEventHandler getMessage;

        public delegate void getMessageEventHandler(string str, ref User sender);

        public event clientLogoutEventHandler clientLogout;

        public delegate void clientLogoutEventHandler(User client);

        protected void bcceventhandler(string msg, string channel, bool chatNotMuted, string user = "SYSTEM T")
        {
            if (IsLoggedIn)
            {
                Send(msg);
            }
        }

        protected void Broadcast_eventHander(string msg)
        {
            if (IsLoggedIn)
                Send(msg);
        }

        public string Password { get; set; } = "";

        private StreamWriter sendMessage;
        private TcpClient listClient;

        public bool ChatMuted { get; set; } = false;
        /// <summary>
        /// Пользовательская опция.
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn { get; set; } = false;
        public bool Registered { get; set; } = false;

        /// <summary>
        /// Если значение равно True, все полученные данные и отправленные не будут обрабатыватся.
        /// </summary>
        /// <returns></returns>
        public bool Ignored { get; set; } = false;
        /// <summary>
        /// Username может быть любым. Эта опция не используется в коде DLL кроме FindByName(string name)
        /// </summary>
        /// <returns></returns>
        public string Username { get; set; } = "";
        /// <summary>
        /// Предназначено для пользовательский опций.
        /// </summary>
        /// <returns></returns>
        public StringCollection UserOptions { get; set; } = new StringCollection();

        private string cIP = "0.0.0.0";

        public int Group { get; set; } = 1;
        

        public string IP
        {
            get
            {
                return cIP;
            }
        }
        /// <summary>
        /// CID предначначен для поиска клиента через FindByCID(int cid)
        /// </summary>
        /// <returns></returns>
        public string CID { get; set; } = new Random().Next(10000, 999999999).ToString() + new Random().Next(10000, 999999999).ToString() + new Random().Next(10000, 999999999).ToString();

        public User(TcpClient forClient)
        {
            listClient = forClient;
            listClient.GetStream().BeginRead(new byte[] { 0 }, 0, 0, (_) => ReadAllClient(), null);
            cIP = ((IPEndPoint)listClient.Client.RemoteEndPoint).Address.ToString();
        }

        private void BroadcastOnly(int opt, string str)
        {
            if (Group > opt - 1)
            {
                Send(str);
            }
        }
        /// <summary>
        /// Отсоединяет клиента от сервера.
        /// </summary>
        public void Disconnect()
        {
            listClient.Client.Close();
            listClient = null;
            clientList.Remove(this);
        }

        private void ReadAllClient()
        {
            try
            {
                if (!Ignored)
                {
                    var argsender = this;
                    getMessage?.Invoke(Encode.Decrypt(new StreamReader(listClient.GetStream()).ReadLine()), ref argsender);
                    listClient.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback((_) => ReadAllClient()), null);
                }
            }
            catch (Exception ex)
            {
                clientLogout?.Invoke(this);
            }
        }
        /// <summary>
        /// Отправляет клиенту сообщение.
        /// </summary>
        /// <param name="msg">Сообщение, которое будет отправлено.</param>
        public void Send(string msg)
        {
            // Console.WriteLine($"{Username} > {msg}")
            try
            {
                if (!Ignored)
                {
                    sendMessage = new StreamWriter(listClient.GetStream());
                    sendMessage.WriteLine(Encode.Encrypt(msg));
                    sendMessage.Flush();
                }
            }
            catch (Exception e)
            {
                // clientList.Remove(Me)
                RemoveUnconnectedClients();
            }
        }
    }
}

public partial class Client
{
    private static TcpClient client;
    private static StreamWriter sWriter;
    private static int NIckFrefix = new Random().Next(111, 99999);
    /// <summary>
    /// Происходит когда клиент получает новое сообщение.
    /// </summary>
    /// <param name="msg">Сообщение</param>
    public static event OnMessageReceivedEventHandler OnMessageReceived;

    public delegate void OnMessageReceivedEventHandler(string msg);

    public delegate void _xUpdate(string str);
    // 
    // '    '     If InvokeRequired Then
    // 
    // Else
    // '   '      TextBox3.AppendText(str & vbNewLine)
    // End If
    // End Sub

    private static void read(IAsyncResult ar)
    {
        try
        {
            OnMessageReceived?.Invoke(Encode.Decrypt(new StreamReader(client.GetStream()).ReadLine()));
            client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, read, null);
        }
        catch (Exception ex)
        {
            // xUpdate("You have disconnecting from server")
            // Exit Sub
        }
    }

    public static void Send(string str)
    {
        // Try
        sWriter = new StreamWriter(client.GetStream());
        sWriter.WriteLine(Encode.Encrypt(str));
        sWriter.Flush();
        // Catch ex As Exception
        // xUpdate("You're not server")
        // End Try
    }
    /// <summary>
    /// Пытается подключиться к указанному серверу по указанному порту.
    /// </summary>
    /// <param name="ip">IP-адрес сервера</param>
    /// <param name="port">Порт сервера</param>

    public static void Connect(string ip, int port)
    {
        // If Button1.Text = "Connect" Then
        // Try
        // EncryptionKey = encodeKey
        client = new TcpClient(ip, port);
        client.GetStream().BeginRead(new byte[] { 0 }, 0, 0, new AsyncCallback(read), null);
        // Button1.Text = "Disconnect"
        // Catch ex As Exception
        // xUpdate("Can't connect to the server!")
        // End Try

    }

    public static void Disconnect()
    {
        client.Client.Close();
        client = null;
    }
}