Imports System.Net.Sockets
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading

Public Class NetworkPlayer
    Inherits CommandSender

    Property Username As String = Nothing
    Property UUID As String = Guid.NewGuid.ToString
    Property Position As Point = New Point(0, 0)
    Property PlayerInventory As Inventory
    Property SelectedItem As ItemStack
    Property Health As Integer = 100
    Property IsAdmin As Boolean = False
    Property PacketQueue As SendQueue
    Property World As WorldServer
    Property IsSpectator As Boolean = False
    Property DisableMovementCheck As Boolean = False
    Property IsLoaded As Boolean = False
    Property PlayerRectangle As Rectangle = New Rectangle(Position, New Size(47, 92))
    Property NoClip As Boolean = False
    Property FallDistance As Integer = 0
    'LastNotSpectatorPosition
    Dim field_01931 As Point = Position
    Sub Spectator()
        IsSpectator = True
        field_01931 = Position
        Form1.Send("removeplayer?" + Username, Username)
        SetNoClip(True)
    End Sub
    Sub Survival()
        IsSpectator = False
        Teleport(field_01931.X, field_01931.Y)

        Form1.Send("addplayer?" + Username, Username)
        SetNoClip(False)

    End Sub
    Sub SetNoClip(arg0 As Boolean)
        If arg0 Then
            Send("noclip")
            NoClip = True
        Else
            Send("clip")
            NoClip = False
        End If
    End Sub
    'getMessage
    Public Event a(ByVal str As String, n As NetworkPlayer)
    'clientLogout
    Public Event b(ByVal client As NetworkPlayer)
    'SendMessage
    Private c As StreamWriter
    'ListClient
    Private d As TcpClient
    Sub New(ByVal forClient As TcpClient)
        MyBase.New("", False)
        d = forClient
        d.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf e, Nothing)
        PacketQueue = New SendQueue(Me)
        World = Form1.World
    End Sub
    Sub Give(m As Material, Optional count As Integer = 1)
        For Each g In PlayerInventory.Items
            If g.Type = m Then
                g.Count += count
                UpdateInventory()
                Exit Sub
            End If
        Next
        PlayerInventory.AddItem(New ItemStack(m, count))
        UpdateInventory()
    End Sub
    Sub UpdateHealth(h As Integer, Optional d As String = "died")
        h = Math.Round(h)
        Dim ev = New netcraft.server.api.events.PlayerHealthEventArgs(Me, Health, h)
        netcraft.server.api.NCSApi.REPlayerHealthEvent(ev)
        If ev.GetCancelled Then Exit Sub
        If h < 1 Then
            Kill(d)
            Exit Sub
        End If
        Health = h
        Send("health?" + h.ToString)
    End Sub
    Sub DoWarning(n As String)
        Send("dowarn?" + n)
    End Sub
    Overloads Sub Damage(d As Integer, Optional damager As NetworkPlayer = Nothing)
        If IsNothing(damager) Then
            UpdateHealth(Health - d, "damaged to death")
        Else
            UpdateHealth(Health - d, $"was slain by {damager.Username}")
        End If
    End Sub
    Sub SendSkyColorChange(color As Color)
        Send($"sky?{color.R}?{color.G}?{color.B}")
    End Sub
    Overloads Sub Damage(d As Integer, Optional a As String = "died")
        UpdateHealth(Health - d, a)
    End Sub
    Sub Kill(Optional deathMessage = "died")
        'Form1.Send("chat?" + Username + " " + deathMessage)
        Dim ev = New netcraft.server.api.events.PlayerDeathEventArgs(Me, Username + " " + deathMessage, New Point(0, 0))
        netcraft.server.api.NCSApi.REPlayerDeathEvent(ev)

        If ev.GetCancelled Then
            Health = 1
            Send("health?1")
            Exit Sub
        End If
        netcraft.server.api.NCSApi.Netcraft.Broadcast(ev.GetDeathMessage)
        Teleport(ev.GetSpawn.X, ev.GetSpawn.Y)
        Health = 100
        Send("health?100")
    End Sub
    Sub Teleport(x As Integer, y As Integer)
        Position = New Point(x, y)
        Send("teleport?" + x.ToString + "?" + y.ToString)
    End Sub
    Sub Chat(arg0 As String)
        Send("chat?" + arg0)
    End Sub
    Sub Craft(m As Material)
        If m = Material.STICK Then
            If PlayerInventory.CountOf(Material.PLANKS) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.PLANKS, 2)
            Give(Material.STICK, 4)
        End If
        If m = Material.PLANKS Then
            If PlayerInventory.CountOf(Material.WOOD) < 1 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.WOOD)
            Give(Material.PLANKS, 4)
        End If
        If m = Material.STONE_AXE Then
            If PlayerInventory.CountOf(Material.COBBLESTONE) < 3 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.COBBLESTONE, 3)
            Give(Material.STONE_AXE)
        End If
        If m = Material.STONE_PICKAXE Then
            If PlayerInventory.CountOf(Material.COBBLESTONE) < 3 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.COBBLESTONE, 3)
            Give(Material.STONE_PICKAXE)
        End If
        If m = Material.STONE_SHOVEL Then
            If PlayerInventory.CountOf(Material.COBBLESTONE) < 1 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.COBBLESTONE, 1)
            Give(Material.STONE_SHOVEL)
        End If
        If m = Material.STONE_SWORD Then
            If PlayerInventory.CountOf(Material.COBBLESTONE) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 1 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.COBBLESTONE, 3)
            Give(Material.STONE_SWORD)
        End If

        If m = Material.IRON_AXE Then
            If PlayerInventory.CountOf(Material.IRON) < 3 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.IRON, 3)
            Give(Material.IRON_AXE)
        End If
        If m = Material.IRON_PICKAXE Then
            If PlayerInventory.CountOf(Material.IRON) < 3 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.IRON, 3)
            Give(Material.IRON_PICKAXE)
        End If
        If m = Material.IRON_SHOVEL Then
            If PlayerInventory.CountOf(Material.IRON) < 1 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.IRON, 1)
            Give(Material.IRON_SHOVEL)
        End If
        If m = Material.IRON_SWORD Then
            If PlayerInventory.CountOf(Material.IRON) < 2 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            If PlayerInventory.CountOf(Material.STICK) < 1 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.STICK, 2)
            RemoveItem(Material.IRON, 3)
            Give(Material.IRON_SWORD)
        End If
        If m = Material.FURNACE Then
            If PlayerInventory.CountOf(Material.COBBLESTONE) < 8 Then
                Send("msgerror?У Вас недостаточно материалов для крафта.")
                Exit Sub
            End If
            RemoveItem(Material.COBBLESTONE, 8)
            Give(Material.FURNACE)
        End If
    End Sub
    Overloads Sub SendBlockChange(x As Integer, y As Integer, m As Material, Optional nonsolid As Boolean = False, Optional packetQueue As Boolean = False, Optional isBackground As Boolean = False)
        Dim color As Color
        Dim t = ""
        If m = EnumBlockType.BEDROCK Then t = "bedrock"
        If m = EnumBlockType.STONE Then t = "stone"
        If m = EnumBlockType.DIRT Then t = "dirt"
        If m = EnumBlockType.PLANKS Then t = "planks"
        If m = EnumBlockType.WOOD Then t = "wood"
        If m = EnumBlockType.GRASS_BLOCK Then t = "grass_block"
        If m = EnumBlockType.COBBLESTONE Then t = "cobble"
        If m = EnumBlockType.LEAVES Then t = "leaves"
        If m = EnumBlockType.COAL_ORE Then t = "coal_ore"
        If m = EnumBlockType.IRON_ORE Then t = "iron_ore"
        If m = EnumBlockType.DIAMOND_ORE Then t = "diamond_ore"
        If m = EnumBlockType.OBSIDIAN Then t = "obsidian"
        If m = EnumBlockType.SAND Then t = "sand"
        If m = EnumBlockType.GLASS Then t = "glass"
        If m = EnumBlockType.GOLD_ORE Then t = "gold_ore"
        If m = EnumBlockType.FURNACE Then t = "furnace"
        If m = EnumBlockType.WATER Then
            t = "water"
            nonsolid = True
        End If
        If Not packetQueue Then
            Send($"blockchange?{x}?{y}?" + If(t <> "", t, color.Name) + If(isBackground, "?bg", "?fg") + If(nonsolid, "-non-solid", "solid"))
        Else
            Me.PacketQueue.AddQueue($"blockchange?{x}?{y}?" + If(t <> "", t, color.Name) + If(isBackground, "?bg", "?fg") + If(nonsolid, "-non-solid", "solid"))
        End If
    End Sub

    Overloads Sub SendBlockChange(x As Point, m As Material, Optional nonsolid As Boolean = False, Optional packetQueue As Boolean = False, Optional isBackground As Boolean = False)
        Dim color As Color
        Dim t = ""
        If m = EnumBlockType.BEDROCK Then t = "bedrock"
        If m = EnumBlockType.STONE Then t = "stone"
        If m = EnumBlockType.DIRT Then t = "dirt"
        If m = EnumBlockType.PLANKS Then t = "planks"
        If m = EnumBlockType.WOOD Then t = "wood"
        If m = EnumBlockType.GRASS_BLOCK Then t = "grass_block"
        If m = EnumBlockType.COBBLESTONE Then t = "cobble"
        If m = EnumBlockType.LEAVES Then t = "leaves"
        If m = EnumBlockType.COAL_ORE Then t = "coal_ore"
        If m = EnumBlockType.IRON_ORE Then t = "iron_ore"
        If m = EnumBlockType.DIAMOND_ORE Then t = "diamond_ore"
        If m = EnumBlockType.OBSIDIAN Then t = "obsidian"
        If m = EnumBlockType.SAND Then t = "sand"
        If m = EnumBlockType.GLASS Then t = "glass"
        If m = EnumBlockType.GOLD_ORE Then t = "gold_ore"
        If m = EnumBlockType.FURNACE Then t = "furnace"
        If m = EnumBlockType.WATER Then
            t = "water"
            nonsolid = True
        End If
        If Not packetQueue Then
            Send($"blockchange?{x.X}?{x.Y}?" + If(t <> "", t, color.Name) + If(isBackground, "?bg", "?fg") + If(nonsolid, "-non-solid", "solid"))
        Else
            Me.PacketQueue.AddQueue($"blockchange?{x.X}?{x.Y}?" + If(t <> "", t, color.Name) + If(isBackground, "?bg", "?fg") + If(nonsolid, "-non-solid", "solid"))
        End If
    End Sub
    Sub Furnace(a As Block, b As Material, c As Material)
        If a.Type <> EnumBlockType.FURNACE Then
            Kick("Invalid packet")
            Exit Sub
        End If
        If PlayerInventory.CountOf(b) < 1 Then Exit Sub

        If c = Material.SAND Then
            RemoveItem(b)
            RemoveItem(c)
            Give(Material.GLASS)
            Exit Sub
        End If
        If c = Material.COBBLESTONE Then
            RemoveItem(b)
            RemoveItem(c)
            Give(Material.STONE)
        End If
    End Sub

    Sub UpdateInventory()
        Send("clearinventory")
        Thread.Sleep(100)
        For Each i In PlayerInventory.Items
            PacketQueue.AddQueue("additem?" + i.Type.ToString + " x " + i.Count.ToString)
        Next
        PacketQueue.SendQueue()
    End Sub
    Sub RemoveItem(m As Material, Optional count As Integer = 1)
        Dim itemsToRemove As New List(Of ItemStack)
        For Each item In PlayerInventory.Items
            If item.Type <> m Then Continue For
            If item.Count > count Then
                item.Count -= count
            End If
            If (item.Count < 1) Or (item.Count <= count) Then
                itemsToRemove.Remove(item)
            End If
        Next
        For Each item In itemsToRemove
            PlayerInventory.Items.Remove(item)
        Next
        If PlayerInventory.Items.IndexOf(SelectedItem) = -1 Then
            For Each i In netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers
                If i.UUID <> UUID Then
                    i.Send($"itemset?{Username}?nothing")
                Else
                    i.Send($"itemset?@?nothing")
                End If
            Next
        End If
        UpdateInventory()
    End Sub
    ReadOnly Property IsOnGround As Boolean

        Get

            For Each o In Form1.World.WorldBlocks
                Dim bpos = New Point(o.Position.X * 32, o.Position.Y * 32)
                Dim brec = New Rectangle(bpos, New Size(32, 32))
                If Form1.DistanceBetween(bpos, Position) > 10 * 32 Then Continue For
                If brec.IntersectsWith(PlayerRectangle) Then
                    If o.IsBackground Then Continue For
                    If o.Type = EnumBlockType.WATER Then Continue For
                    If NoClip Then Return True
                    Return True
                Else : Continue For
                End If
            Next
            Return False
        End Get

    End Property
    Private Sub e()
        PlayerRectangle = New Rectangle(Position, New Size(47, 92))
        
        Try
            RaiseEvent a(Encode.Decrypt(New StreamReader(d.GetStream).ReadLine), Me)
        Catch ex As SocketException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)

            RaiseEvent b(Me)
        Catch ex As IOException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)

            RaiseEvent b(Me)
        Catch ex As ObjectDisposedException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)
        Catch ex As ArgumentNullException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)
        Catch ex As ArgumentOutOfRangeException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)
        Catch ex As ArgumentException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)
        Catch ex As OutOfMemoryException
            Form1.LogError(ex)
            Form1.UpdateList(ex.ToString)
        Catch ex As NullReferenceException
            Form1.LogError(ex)
        Catch ex As InvalidOperationException
            RaiseEvent b(Me)
        Catch ex As Exception
            Form1.CrashReport(ex)
        End Try
        Try
            d.GetStream.BeginRead(New Byte() {0}, 0, 0, New AsyncCallback(AddressOf e), Nothing)
        Catch ex As NullReferenceException
        Catch ex As InvalidOperationException
            RaiseEvent b(Me)
        End Try
    End Sub
    Public Sub Send(ByVal Messsage As String)
        Try
            c = New StreamWriter(d.GetStream)
            c.WriteLine(Encode.Encrypt(Messsage))
            c.Flush()
        Catch ex As Exception
            Form1.LogError(ex)
            Console.Error.WriteLine(ex.ToString)
            Form1.UpdateList(ex.ToString)
        End Try
    End Sub
    Public Sub Kick(Optional kickMessage As String = "You have been kicked out from server.", Optional bcKick As Boolean = False)
        If bcKick Then
            For Each cl In Form1.clientList
                If cl.IsLoaded Then
                    cl.Chat($"{Username} был выгнан из игры! Причина: {kickMessage}")
                End If
            Next
        End If
        Send("msg?" + kickMessage)
        d.Client.Close()
        d = Nothing
    End Sub
End Class
#Disable Warning
Public Class Encode
    Private Shared key() As Byte = {62, 59, 25, 19, 37}
    Private Shared IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
    Friend Const EncryptionKey As String = "81iSifdf" '"HOMECLOUD" & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString ' & New Random().Next(11111111, 99999999).ToString & New Random().Next(11111111, 99999999).ToString

    Public Shared Function Decrypt(ByVal stringToDecrypt As String) As String
        Try
            Dim inputByteArray(stringToDecrypt.Length) As Byte
            key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(EncryptionKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(stringToDecrypt)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            'oops - add your exception logic
            'MsgBox("ошибка")
        End Try
    End Function

    Public Shared Function Encrypt(ByVal stringToEncrypt As String) As String
        Try
            key = System.Text.Encoding.UTF8.GetBytes(Left(EncryptionKey, 8))
            Dim des As New DESCryptoServiceProvider
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(stringToEncrypt)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            'oops - add your exception logic
            'MsgBox("ошибка")
        End Try
    End Function
End Class