Imports NetcraftServer
Public Class Plugin

    Property Name As String = "ChatMuffler"
    Dim mutelist As Specialized.StringCollection = New Specialized.StringCollection
    Sub OnLoad()
        netcraft.server.api.Logger.Info("[ChatMuffler] Плагин успешно загружен")
        AddHandler netcraft.server.api.NCSApi.PlayerChatEvent, AddressOf OnChat
        AddHandler netcraft.server.api.NCSApi.PlayerJoinEvent, AddressOf OnJoin
    End Sub
    Private Sub OnJoin(e As netcraft.server.api.events.PlayerJoinEventArgs)
        e.GetPlayer.Chat("Этот сервер использует официальный плагин от разработчика Netcraft: ChatMuffler")
    End Sub
    Private Sub OnChat(e As netcraft.server.api.events.PlayerChatEventArgs)

        Dim p As NetworkPlayer = e.GetPlayer
        If mutelist.Contains(p.Username) Then
            p.Chat("Вы не можете отправлять сообщения.")
            e.SetCancelled(True)
        End If
        Dim arr = e.GetMessage.Split(" ")
        If arr(0) = "/mute" Then
            If p.IsAdmin Then
                mutelist.Add(arr(1))
                p.Chat(arr(1) + " больше не может отправлять сообщения.")
            Else
                p.Chat("У Вас недостаточно разрешений для выполнения данной команды.")
            End If
            e.SetCancelled(True)
        End If
        If arr(0) = "/unmute" Then
            If p.IsAdmin Then
                mutelist.Remove(arr(1))
                p.Chat(arr(1) + " снова может отправлять сообщения.")
            Else
                p.Chat("У Вас недостаточно разрешений для выполнения данной команды.")
            End If
            e.SetCancelled(True)
        End If
    End Sub

End Class
