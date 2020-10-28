Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ConfigParser
Imports NetcraftServer.netcraft.server.api
Public Class Form1
    Friend Property World As WorldServer

    Dim Listning As TcpListener
    Friend ReadOnly clientList As New List(Of NetworkPlayer)
    Dim pClient As NetworkPlayer
    'Dim npc As EntityPlayerNPC = New EntityPlayerNPC
    ReadOnly worldfile = Application.StartupPath + "\world.txt"
    ReadOnly configfile = Application.StartupPath + "\config.txt"
    ReadOnly sv As SaveLoad = New SaveLoad
    Friend maxPlayers As Integer = 20
    Dim chatFormat As String = "%1 %2"
    Dim everyBodyAdmin As Integer = 0
    Property IsSingleplayerServer As Boolean = False
    Dim worldloadDelay As Integer = 40
    Sub LoadConfig()
        Dim cfg = File.ReadAllText(configfile, Encoding.UTF8)
        maxPlayers = CInt(Config.GetValue("max-players", cfg, "-1"))
        chatFormat = Config.GetValue("chat-format", cfg, "%1 %2")
        everyBodyAdmin = CInt(Config.GetValue("everybody-admin", cfg, "0"))
    End Sub
    Sub OnCrash(sender As Object, e As ThreadExceptionEventArgs)
        CrashReport(e.Exception)
    End Sub
    Sub LoadCommands()
        NCSApi.Netcraft.AddCommand(New Commandhelp)
        NCSApi.Netcraft.AddCommand(New Commandban)
        NCSApi.Netcraft.AddCommand(New Commandunban)
        NCSApi.Netcraft.AddCommand(New Commanditems)
        NCSApi.Netcraft.AddCommand(New Commandcraft)
        NCSApi.Netcraft.AddCommand(New Commandtoggleadmin)
        NCSApi.Netcraft.AddCommand(New Commandkick)
        NCSApi.Netcraft.AddCommand(New Commandkill)
        NCSApi.Netcraft.AddCommand(New Commandtogglespectator)
        NCSApi.Netcraft.AddCommand(New Commandgive)
        NCSApi.Netcraft.AddCommand(New Commandnmc)
        NCSApi.Netcraft.AddCommand(New Commandbroadcast)
        NCSApi.Netcraft.AddCommand(New Commandlist)
    End Sub
    Private Sub eventhandler_a(m As String)
        Log(m)
        Send("chat?" + m)
    End Sub
    Dim th As Thread
    Private Sub FormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        netcraft.server.api.NCSApi.Netcraft.MainWindow = Me
        CheckForIllegalCrossThreadCalls = False
        AddHandler Application.ThreadException, AddressOf OnCrash
        LoadBanlist()

        LoadConfig()
        LoadCommands()

        AddHandler netcraft.server.api.NCSApi.Netcraft.dobc, AddressOf eventhandler_a

        For Each s In Directory.GetFiles(Application.StartupPath + "\plugins")
            Dim p = PluginManager.Load(s)
            Log($"[PLUGIN LOADER] {PluginManager.GetObject(p, "Plugin").Name} is loaded!")
        Next

        If File.Exists(worldfile) Then
            World = sv.Load(File.ReadAllText(worldfile, encoding:=Encoding.UTF8))
        Else
            World = WorldGenerator.Generate
            File.WriteAllText(worldfile, sv.Save(World), Encoding.UTF8)
        End If
        Start()



    End Sub
    ' create a delegate
    Delegate Sub _cUpdate(ByVal str As String, ByVal relay As Boolean)
    Sub UpdateList(ByVal str As String, Optional relay As Boolean = False)
        On Error Resume Next
        If InvokeRequired Then
            Invoke(New _cUpdate(AddressOf UpdateList), str, relay)
        Else
            RichTextBox1.AppendText(str + vbCrLf)
            Console.WriteLine(str)
            ' if relay we will send a string
            If relay Then Send(str)
        End If
    End Sub
    Delegate Sub xSend(ByVal str As String, except As String, readyClientsOnly As Boolean)
    Sub Send(ByVal str As String, Optional except As String = Nothing, Optional readyClientsOnly As Boolean = False)
        If InvokeRequired Then

            Invoke(New xSend(AddressOf Send), str, except, readyClientsOnly)

        Else

            For x As Integer = 0 To clientList.Count - 1
                Try
                    If except <> Nothing Then
                        If clientList(x).Username = except Then Continue For
                    End If
                    If readyClientsOnly Then
                        If clientList(x).IsLoaded Then clientList(x).Send(str)
                    Else
                        clientList(x).Send(str)
                    End If
                Catch ex As Exception
                    clientList.RemoveAt(x)
                End Try
            Next
        End If
    End Sub
    Sub SaveBanlist()
        Dim a = ""
        For Each b In netcraft.server.api.NCSApi.Netcraft.GetBannedPlayers
            a += b + vbLf
        Next
        File.WriteAllText(Application.StartupPath + "\banned-players.txt", a, Encoding.UTF8)
    End Sub
    Sub LoadBanlist()
        For Each a In File.ReadAllText(Application.StartupPath + "\banned-players.txt", Encoding.UTF8).Split(vbLf)
            netcraft.server.api.NCSApi.Netcraft.field_a.Add(a)
        Next
    End Sub
    Sub AcceptClient(ByVal ar As IAsyncResult)
        pClient = New NetworkPlayer(Listning.EndAcceptTcpClient(ar))
        AddHandler(pClient.a), AddressOf MessageReceived
        AddHandler(pClient.b), AddressOf ClientExited
        clientList.Add(pClient)
        netcraft.server.api.NCSApi.Netcraft.clientList = clientList
        Listning.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptClient), Listning)
    End Sub
    Sub Log(arg0 As String, Optional arg1 As String = "INFO")
        UpdateList($"{Date.Now} [{arg1}] {arg0}")
    End Sub
    Sub LogError(ex As Exception)
        Console.Error.WriteLine(ex.ToString)
        Log($"Exception in server thread:" + vbCrLf + ex.ToString)
    End Sub
    Sub MessageReceived(ByVal str As String, n As NetworkPlayer)

        'UpdateList(str, True)
        Dim a As String() = str.Split("?")

        If a(0) = "setname" Then
            If n.Username <> "" Then
                n.Kick()
                Exit Sub
            End If

            If maxPlayers + 1 = clientList.Count Then
                n.Kick("Сервер заполнен!")
                Exit Sub
            End If
            For Each netplayer In clientList

                If netplayer.UUID = n.UUID Then Continue For
                If netplayer.Username = a(1) Then
                    n.Kick("Игрок с таким именем уже играет на сервере.")
                    Exit Sub
                End If

            Next
            n.Username = a(1)
            n.senderName = n.Username
            If Not Regex.Match(n.Username, "^[a-zA-Z0-9_]*").Success Then
                n.Kick("Illegal characters in name")
                Exit Sub
            End If

            If netcraft.server.api.NCSApi.Netcraft.IsBanned(n.Username) Then

                n.Kick("You are banned from this server!")
                Exit Sub
            End If
            Dim ev = New netcraft.server.api.events.PlayerJoinEventArgs(n)

            netcraft.server.api.NCSApi.REPlayerJoinEvent(ev)
            Send("addplayer?" + a(1), n.Username)
            n.PlayerInventory = New Inventory(n)


            If File.Exists(Application.StartupPath + "\playerdata\" + n.Username + ".txt") Then
                PlayerInfoSaveLoad.Load(n, File.ReadAllText(Application.StartupPath + "\playerdata\" + n.Username + ".txt", Encoding.UTF8))
            Else
                n.PlayerInventory.AddItem(New ItemStack(Material.WOODEN_PICKAXE, 1))
                n.PlayerInventory.AddItem(New ItemStack(Material.WOODEN_AXE, 1))
                n.PlayerInventory.AddItem(New ItemStack(Material.WOODEN_SWORD, 1))
                n.PlayerInventory.AddItem(New ItemStack(Material.WOODEN_SHOVEL, 1))
            End If

            Log(n.Username + " присоединился к игре")
            Chat(n.Username + " заходит на сервер")


            'n.Send("blockchange?500?50?Red")
        End If

        If n.Username = "" Then

            n.Kick()
            Exit Sub
            'n.Username = "minecraft123"
        End If
        n.senderAdmin = n.IsAdmin
        If a(0) = "world" Then

            For Each b In World.WorldBlocks
                n.SendBlockChange(b.Position, b.Type, packetQueue:=True, isBackground:=b.IsBackground)
            Next
            n.PacketQueue.SendQueue()

            For Each p In clientList
                Thread.Sleep(500)
                If p.Username = n.Username Then Continue For
                If p.IsSpectator Then Continue For
                n.Send("addplayer?" + p.Username)
                Thread.Sleep(200)
                n.Send("moveplayer?" + p.Username + "?" + p.Position.X.ToString + "?" + p.Position.Y.ToString)
                Try

                    Thread.Sleep(200)
                    n.Send("itemset?" + p.Username + "?" + p.SelectedItem.Type)
                Catch ex As Exception
                    LogError(ex)
                End Try
            Next
            Thread.Sleep(500)
            n.Send("startticking")
            n.IsLoaded = True
            Thread.Sleep(100)
            n.Chat("Добро пожаловать на сервер.")
            If everyBodyAdmin = 1 Then
                n.IsAdmin = True
            End If

            'n.UpdateHealth(100)
            'n.Send("blockchange?500?50?Red")
        End If
        If a(0) = "entityplayermove" Then
            Dim mto = New Point(a(1), a(2))
            n.PlayerRectangle = New Rectangle(mto, New Size(47, 92))

            Dim ev = New netcraft.server.api.events.PlayerMoveEventArgs(n.Position, mto, n)
            If ev.GetCancelled Then
                n.Send($"teleport?{n.Position.X.ToString}?{n.Position.Y.ToString}")
                Exit Sub
            End If
            Dim v = Normalize(mto - n.Position)
            If Not n.DisableMovementCheck Then
                If (v.X > 10) Or (v.Y > 10) Or (v.X < -10) Or (v.Y < -10) Then
                    Log($"{n.Username} moved too fast! {v.X},{v.Y}", "WARNING")
                    n.Send($"teleport?{n.Position.X}?{n.Position.Y}")
                    Exit Sub
                End If

                For Each b In World.WorldBlocks
                    Dim bpos = New Point(b.Position.X * 32, b.Position.Y * 32)
                    Dim brec = New Rectangle(bpos, New Size(32, 32))
                    If DistanceBetween(bpos, n.Position) > 10 * 32 Then Continue For
                    If bpos.Y > n.Position.Y + 85 Then Continue For
                    If brec.IntersectsWith(n.PlayerRectangle) Then
                        If b.IsBackground Then Continue For
                        If b.Type = EnumBlockType.WATER Then Continue For
                        If n.NoClip Then Exit Sub
                        Log($"{n.Username} moved wrongly!", "WARNING")
                        If v.Y > 1 Then
                            n.Teleport(n.Position.X, n.Position.Y - 2)
                        Else
                            n.Send($"teleport?{n.Position.X}?{n.Position.Y}")
                        End If
                        Exit Sub
                    Else : Continue For
                    End If
                Next
            End If
            If mto.Y > n.Position.Y Then
                Dim grounded = False
                Dim inWater = False
                For Each b In World.WorldBlocks
                    Dim bpos = New Point(b.Position.X * 32, b.Position.Y * 32)
                    Dim brec = New Rectangle(bpos, New Size(32, 32))
                    If DistanceBetween(bpos, n.Position) > 10 * 32 Then Continue For
                    If brec.IntersectsWith(n.PlayerRectangle) Then
                        If b.IsBackground Then Continue For
                        If b.Type = EnumBlockType.WATER Then inWater = True
                        grounded = True
                    Else : Continue For
                    End If
                Next
                If Not grounded Then
                    n.FallDistance += 1
                    If inWater Then
                        n.FallDistance = 0
                    End If
                Else

                    If inWater Then
                        n.FallDistance = 0
                    End If

                    If n.FallDistance > 3 * 32 Then
                        n.Damage((n.FallDistance / 16) / 3, "fell from a high place")
                    End If
                    n.FallDistance = 0
                End If
            End If

            n.Position = mto
            If n.IsSpectator Then Exit Sub
            If n.Position.Y > 619 Then
                n.Damage(1, "fell out of the world")
            End If
            Send("updateplayerposition?" + n.Username + "?" + a(1) + "?" + a(2), n.Username)
        End If
        Try

            'If a(0) = "craft" Then
            '    Dim enumMaterial = [Enum].Parse(GetType(Material), a(1))
            '    n.Craft(enumMaterial)
            'End If
            If a(0) = "chat" Then
                Dim message = String.Join("?", a.Skip(1).ToArray)
                Dim ev = New netcraft.server.api.events.PlayerChatEventArgs(n, message)

                netcraft.server.api.NCSApi.REPlayerChatEvent(ev)

                If ev.GetCancelled Then Exit Sub

                If message(0) = "/" Then

                    Dim arr = message.Split(" ")
                    Dim lbl = message.Skip(1).ToArray
                    Dim label = New String(lbl)
                    Dim args = arr.Skip(1).ToArray
                    Dim cmd As Command

                    For Each g In netcraft.server.api.NCSApi.Netcraft.GetCommands
                        If g.Name.ToLower = label.Split(" ")(0).ToLower Then
                            cmd = g
                        End If
                    Next
                    If IsNothing(cmd) Then
                        n.SendMessage("Команда не найдена!")
                        Exit Sub
                    End If
                    Dim y
                    Try
                        y = cmd.OnCommand(n, cmd, args, label)
                    Catch ex As Exception
                        n.SendMessage("Произошла внутренняя ошибка при выполнении данной команды.")
                        LogError(ex)
                        Exit Sub
                    End Try
                    If Not y Then
                        n.SendMessage("Использование: " + cmd.Usage)
                    End If

                Else
                    Send("chat?" + chatFormat.Replace("%1", n.Username).Replace("%2", message))
                End If
                Log(chatFormat.Replace("%1", n.Username).Replace("%2", message))
            End If
        Catch ex As IndexOutOfRangeException
            LogError(ex)

        Catch ex As Exception
            LogError(ex)
            Log(ex.ToString, "ERROR")
            CrashReport(ex)
        End Try
        Try
            If a(0) = "update_inventory" Then
                'n.Send("clearinventory")
                'Thread.Sleep(100)
                'For Each i In n.PlayerInventory.Items
                '    Thread.Sleep(100)
                '    n.Send("additem?" + i.Type.ToString + " x " + i.Count.ToString)
                'Next
                n.UpdateInventory()
            End If
        Catch ex As Exception
            Log(ex.ToString + vbCrLf + ex.Source, "SEVERE")
            LogError(ex)
        End Try
        If a(0) = "selectitem" Then

            For Each i In n.PlayerInventory.Items
                If (i.Type.ToString + " x " + i.Count.ToString) = a(1) Then
                    n.SelectedItem = i
                    Try
                        Send("itemset?" + n.Username + "?" + n.SelectedItem.Type.ToString, n.Username)
                        n.Send("itemset?@?" + n.SelectedItem.Type.ToString)
                    Catch ex As Exception
                        Log(ex.ToString, "SEVERE")
                        LogError(ex)
                    End Try
                    Exit For
                End If
            Next
        End If
        If a(0) = "block_break" Then
            Try
                If IsNothing(n.SelectedItem) Then Exit Sub
                For Each b In World.WorldBlocks
                    If b.Position.X = CInt(a(1)) Then

                        If b.Position.Y = CInt(a(2)) Then
                            If b.Unbreakable Then Exit Sub
                            Dim pos = Normalize(n.Position)
                            If DistanceBetween(pos.X, pos.Y, b.Position.X, b.Position.Y) > 6 Then
                                n.DoWarning("Unreachable block!")
                                Exit Sub
                            End If

                            Dim ev As netcraft.server.api.events.BlockBreakEventArgs = New netcraft.server.api.events.BlockBreakEventArgs(n, b)

                            netcraft.server.api.NCSApi.REBlockBreakEvent(ev)

                            If ev.GetCancelled Then

                                Exit Sub
                            End If


                            Send("removeblock?" + a(1) + "?" + a(2))

                            If b.Type = EnumBlockType.STONE Then
                                If (n.SelectedItem.Type = Material.WOODEN_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.STONE_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.COBBLESTONE)
                            End If
                            If b.Type = EnumBlockType.COAL_ORE Then
                                If (n.SelectedItem.Type = Material.WOODEN_PICKAXE) Then n.Give(Material.COAL, 3)
                                If (n.SelectedItem.Type = Material.STONE_PICKAXE) Then n.Give(Material.COAL, 3)
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.COAL, 3)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.COAL, 3)
                            End If

                            If b.Type = EnumBlockType.IRON_ORE Then
                                If (n.SelectedItem.Type = Material.STONE_PICKAXE) Then n.Give(Material.IRON, 3)
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.IRON, 3)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.IRON, 3)
                            End If

                            If b.Type = EnumBlockType.DIAMOND_ORE Then
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.DIAMOND, 3)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.DIAMOND, 3)
                            End If

                            If b.Type = EnumBlockType.DIRT Then
                                n.Give(Material.DIRT)
                            End If

                            If b.Type = EnumBlockType.COBBLESTONE Then
                                If (n.SelectedItem.Type = Material.WOODEN_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.STONE_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.COBBLESTONE)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.COBBLESTONE)
                            End If

                            If b.Type = EnumBlockType.GOLD_ORE Then
                                If (n.SelectedItem.Type = Material.IRON_PICKAXE) Then n.Give(Material.GOLD)
                                If (n.SelectedItem.Type = Material.DIAMOND_PICKAXE) Then n.Give(Material.GOLD)
                            End If

                            If b.Type = EnumBlockType.WOOD Then
                                n.Give(Material.WOOD)
                            End If

                            If b.Type = EnumBlockType.PLANKS Then
                                n.Give(Material.PLANKS)
                            End If

                            If b.Type = EnumBlockType.SAND Then
                                n.Give(Material.SAND)
                            End If
                            If b.Type = EnumBlockType.FURNACE Then
                                n.Give(Material.FURNACE)
                            End If

                            World.WorldBlocks.Remove(World.GetBlockAt(a(1), a(2)))
                            Exit For
                        Else
                            'UpdateList($"{n.Username} trying to break {a(1)} {a(2)}, but Y was {b.Position.Y}")
                        End If
                    Else
                        'UpdateList($"{n.Username} trying to break {a(1)} {a(2)}, but X was {b.Position.X}")
                    End If
                Next
            Catch ex As Exception
                LogError(ex)
                UpdateList(ex.ToString)
            End Try
        End If
        Try
            If a(0) = "block_place" Then
                Dim placeAt As Point = New Point(a(1), a(2))
                Dim type As EnumBlockType
                Dim pos = Normalize(n.Position)
                placeAt = Normalize(placeAt)
                If DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6 Then
                    n.DoWarning("Unreachable block!")
                    Exit Sub
                End If
                type = [Enum].Parse(GetType(EnumBlockType), n.SelectedItem.Type.ToString)
                Dim b = New Block(placeAt, type, False, False)
                Dim ev As netcraft.server.api.events.BlockPlaceEventArgs = New netcraft.server.api.events.BlockPlaceEventArgs(n, b)

                netcraft.server.api.NCSApi.REBlockPlaceEvent(ev)

                If ev.GetCancelled Then

                    Exit Sub
                End If
                For Each g In clientList
                    g.SendBlockChange(placeAt, type)
                Next
                World.WorldBlocks.Add(b)
                n.SelectedItem.Count -= 1
                If n.SelectedItem.Count <= 0 Then
                    n.PlayerInventory.Items.Remove(n.SelectedItem)
                    n.SelectedItem = Nothing
                End If
                n.UpdateInventory()
            End If


            If a(0) = "block_place_bg" Then
                Dim placeAt As Point = New Point(a(1), a(2))
                Dim type As EnumBlockType
                Dim pos = Normalize(n.Position)
                placeAt = Normalize(placeAt)
                If DistanceBetween(pos.X, pos.Y, placeAt.X, placeAt.Y) > 6 Then
                    n.DoWarning("Unreachable block!")
                    Exit Sub
                End If
                type = [Enum].Parse(GetType(EnumBlockType), n.SelectedItem.Type.ToString)
                Dim b = New Block(placeAt, type, False, True)
                Dim ev As netcraft.server.api.events.BlockPlaceEventArgs = New netcraft.server.api.events.BlockPlaceEventArgs(n, b)

                netcraft.server.api.NCSApi.REBlockPlaceEvent(ev)

                If ev.GetCancelled Then

                    Exit Sub
                End If
                For Each g In clientList
                    g.SendBlockChange(placeAt, type, isBackground:=True)
                Next
                World.WorldBlocks.Add(b)
                n.SelectedItem.Count -= 1
                If n.SelectedItem.Count <= 0 Then
                    n.PlayerInventory.Items.Remove(n.SelectedItem)
                    n.SelectedItem = Nothing
                End If
                n.UpdateInventory()
            End If
        Catch ex As NullReferenceException
        Catch ex As Exception
            LogError(ex)
            'MsgBox(ex.ToString)
            CrashReport(ex)
            Log(ex.ToString, "SEVERE")
        End Try
        Try
            If a(0) = "pvp" Then
                Dim nd As NetworkPlayer
                For Each i In clientList
                    If i.Username = a(1) Then
                        nd = i
                    End If
                Next
                If IsNothing(nd) Then
                    n.Kick("Attempting to attack an invalid player")
                    Exit Sub
                End If
                If nd.Username = n.Username Then
                    n.Kick("Attempting to attack self")
                    Exit Sub
                End If
                If DistanceBetween(Normalize(n.Position), Normalize(nd.Position)) > 5 Then
                    n.DoWarning("Unreachable player!")
                    Exit Sub
                End If

                If Not IsNothing(n.SelectedItem) Then
                    If n.SelectedItem.Type = Material.DIAMOND_SWORD Then
                        nd.Damage(25, n)
                    ElseIf n.SelectedItem.Type = Material.IRON_SWORD Then
                        nd.Damage(20, n)
                    ElseIf n.SelectedItem.Type = Material.STONE_SWORD Then
                        nd.Damage(14, n)
                    ElseIf n.SelectedItem.Type = Material.WOODEN_SWORD Then
                        nd.Damage(9, n)
                    Else
                        nd.Damage(5, n)
                    End If
                Else
                    nd.Damage(3, n)
                End If
            End If
        Catch ex As NullReferenceException
        Catch ex As Exception
            CrashReport(ex)
        End Try

        Try
            If a(0) = "furnace" Then
                If IsNothing(n.SelectedItem) Then Exit Sub
                n.Furnace(World.GetBlockAt(a(1), a(2)), Material.COAL, n.SelectedItem.Type)
            End If
        Catch ex As Exception
            LogError(ex)
        End Try
    End Sub
    Sub CrashReport(ex As Exception)
        Dim crashText = "Netcraft Crash Report" + vbCrLf + $"Server crashed at {Date.Now.ToString}" + vbCrLf + $"{ex.GetType.ToString}: {ex.Message}{vbCrLf}== STACK TRACE =={vbCrLf}{ex.InnerException.StackTrace}{vbCrLf}{vbCrLf}" +
            $"Exception.TargetSite: {ex.TargetSite}" + vbCrLf +
            $"Exception.Source: {ex.Source}"

        File.AppendAllText(Application.StartupPath + "\crash-reports\" + Date.Now.ToString.Replace(" ", "_").Replace(".", "-").Replace(":", "-") + ".txt", crashText)
        'Console.WriteLine($"OOPS!! THE SERVER IS CRASHED{vbCrLf}{vbCrLf}Crash report is saved.{vbCrLf}{vbCrLf}{crashText}")
        Console.Error.WriteLine("OOPS! The server is crashed" + vbCrLf + vbCrLf + crashText)

        End
    End Sub
    Overloads Function DistanceBetween(x1 As Double, y1 As Double, x2 As Double, y2 As Double) As Double
        Return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2))
    End Function
    Overloads Function DistanceBetween(p1 As Point, p2 As Point) As Double
        Dim x1 = p1.X
        Dim x2 = p2.X
        Dim y1 = p1.Y
        Dim y2 = p2.Y
        Return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2))
    End Function
    Function Normalize(p As Point) As Point
        Dim point = New Point With {
            .X = p.X \ 32,
            .Y = p.Y \ 32
        }
        Return point
    End Function
    Sub Chat(arg0 As String)
        Send("chat?" + arg0)
    End Sub
    Sub ClientExited(ByVal client As NetworkPlayer)
        Dim ev = New netcraft.server.api.events.PlayerLeaveEventArgs(client)
        netcraft.server.api.NCSApi.REPlayerLeaveEvent(ev)
        File.WriteAllText(Application.StartupPath + "\playerdata\" + client.Username + ".txt", PlayerInfoSaveLoad.Save(client), Encoding.UTF8)
        clientList.Remove(client)
        Chat(client.Username + " покинул игру")
        Log(client.Username + " покинул игру")
        Send("removeplayer?" + client.Username)
        netcraft.server.api.NCSApi.Netcraft.clientList = clientList
    End Sub
    Dim Listening As Boolean = False
    Private Sub Start()
        Listning = New TcpListener(IPAddress.Any, 6575)
        If Listening = False Then

            Listning.Start()
            Log("Сервер NetCraft 1.1 запускается")
            Listning.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptClient), Listning)
            Listening = True
        Else
            Listning.Stop()
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        File.WriteAllText(worldfile, sv.Save(World), Encoding.UTF8)
        For Each c In clientList
            ClientExited(c)
        Next
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Dim m = TextBox1.Text
            Dim args = m.Split(" ").Skip(1).ToArray
            Dim cmd = m.Split(" ")(0)

            Dim toRun As Command
            For Each local_a In netcraft.server.api.NCSApi.Netcraft.GetCommands
                If local_a.Name.ToLower = cmd.ToLower Then
                    toRun = local_a
                End If
            Next
            If IsNothing(toRun) Then
                Log("Неизвестная команда.")
            Else
                Dim y
                Try
                    y = toRun.OnCommand(netcraft.server.api.NCSApi.Netcraft.ConsoleCommandSender, toRun, args, m)
                Catch ex As Exception
                    Log("Произошла внутренняя ошибка при выполнении данной команды.")
                    LogError(ex)
                    Exit Sub
                End Try
                If Not y Then
                    Log("Использование: " + toRun.Usage)
                End If
            End If

            TextBox1.Clear()
        End If
    End Sub
    Sub ForceCrash()
        Throw New NullReferenceException
    End Sub
    Sub SaveWorld()

        File.WriteAllText(worldfile, sv.Save(World))
        SendCommandFeedback("Мир сохранён", netcraft.server.api.NCSApi.Netcraft.ConsoleCommandSender)

    End Sub
    Sub SendCommandFeedback(a As String, b As CommandSender)
        Log($"{b.GetName}: {a}")
        Send($"chat?{b.GetName}: {a}")
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Timer1.Start()
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        With RichTextBox1
            .Select(.TextLength, 0)
            .ScrollToCaret()
        End With
    End Sub
End Class
