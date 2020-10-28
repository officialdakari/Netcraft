Imports NetcraftServer

Public Class Commandunban
    Inherits Command
    Sub New()
        MyBase.New("unban", "Разбанить игрока на сервере", "unban <игрок>")
    End Sub
    Public Overrides Function OnCommand(sender As CommandSender, cmd As Command, args() As String, label As String) As Boolean
        If Not sender.GetAdmin Then
            sender.SendMessage("У Вас недостаточно прав!")
            Return True
        End If
        If args.Length = 1 Then
            Dim a = args(0)
            netcraft.server.api.NCSApi.Netcraft.Broadcast($"{sender.GetName} разбанил игрока {a}.")
            netcraft.server.api.NCSApi.Netcraft.UnbanPlayer(a)
        End If
        Return False
    End Function
End Class
