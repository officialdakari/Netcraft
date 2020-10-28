Imports NetcraftServer

Public Class Commandhelp
    Inherits Command
    Sub New()
        MyBase.New("help", "Показывает список команд", "help")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If args.Length = 0 Then
            If sender.IsPlayer Then
                Dim p As NetworkPlayer = sender
                For Each a In netcraft.server.api.NCSApi.Netcraft.GetCommands
                    p.PacketQueue.AddQueue($"chat?/{a.Usage} => {a.Description}")
                Next
                p.PacketQueue.SendQueue()
                Return True
            End If
            For Each a In netcraft.server.api.NCSApi.Netcraft.GetCommands
                sender.SendMessage(a.Usage + " => " + a.Description)
            Next
            Return True
        End If
        Return False
    End Function
End Class
