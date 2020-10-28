Imports NetcraftServer

Public Class Commandlist
    Inherits Command
    Sub New()
        MyBase.New("list", "Показывает список игроков", "list")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If args.Length = 0 Then
            If sender.IsPlayer Then
                Dim p As NetworkPlayer = sender
                p.PacketQueue.AddQueue($"chat?Сейчас {Form1.clientList.Count} из {Form1.maxPlayers} игроков на сервере:")
                Dim sc As Specialized.StringCollection = New Specialized.StringCollection
                For Each a In netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers
                    sc.Add(a.Username)
                Next
                p.PacketQueue.AddQueue($"chat?{String.Join(" ", sc)}")
                p.PacketQueue.SendQueue()
            Else
                sender.SendMessage($"Сейчас {Form1.clientList.Count} из {Form1.maxPlayers} игроков на сервере:")
                Dim sc As Specialized.StringCollection = New Specialized.StringCollection
                For Each a In netcraft.server.api.NCSApi.Netcraft.GetOnlinePlayers
                    sc.Add(a.Username)
                Next
                sender.SendMessage($"{String.Join(" ", sc)}")
            End If
            Return True
        End If
        Return False
    End Function
End Class
