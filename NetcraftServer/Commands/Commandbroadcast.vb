Imports NetcraftServer

Public Class Commandbroadcast
    Inherits Command
    Sub New()
        MyBase.New("broadcast", "Отправляет сообщение в чат", "broadcast <сообщение>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length > 0 Then
            netcraft.server.api.NCSApi.Netcraft.Broadcast("[Broadcast] " + String.Join(" ", args))
            Return True
        End If
        Return False
    End Function
End Class
